using MapsterMapper;
using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Exceptions;
using DZDDashboard.Common.Utils;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using DZDDashboard.Data.Entities.History;
using DZDDashboard.Data.Extensions;
using DZDDashboard.Data.History;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;


public class PaymentService(AppDbContext context, IMapper mapper) : IPaymentService
{

    public async Task<EmployeePaymentDto> GetEmployeePaymentAsync(int userId, CancellationToken cancellationToken = default)
    {
        await context.Users.FindRequiredAsync(userId, nameof(User), cancellationToken);

        var salary = await context.Salaries.AsNoTracking()
            .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

        var benefits = await context.BenefitPayments.AsNoTracking()
            .AsSplitQuery()
            .Include(b => (b as HealthInsuranceBenefit)!.Dependents)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.StartDate)
            .ToListAsync(cancellationToken);

        foreach (var benefit in benefits.OfType<HealthInsuranceBenefit>())
            benefit.Dependents = [.. benefit.Dependents.OrderBy(d => d.Id)];

        var additionalPayments = await context.AdditionalPayments.AsNoTracking()
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.StartDate)
            .ToListAsync(cancellationToken);

        var deductions = await context.Deductions.AsNoTracking()
            .Where(d => d.UserId == userId)
            .OrderByDescending(d => d.StartDate)
            .ToListAsync(cancellationToken);

        return new EmployeePaymentDto
        {
            EmployeeId         = userId,
            CurrentSalary      = salary is null ? null : mapper.Map<SalaryRecordDto>(salary),
            SalaryHistory      = await BuildSalaryChangeLogAsync(salary?.Id, cancellationToken),
            Benefits           = mapper.Map<List<BenefitRecordDto>>(benefits),
            AdditionalPayments = mapper.Map<List<AdditionalPaymentDto>>(additionalPayments),
            Deductions         = mapper.Map<List<DeductionDto>>(deductions),
            Summary            = BuildSummary(benefits, additionalPayments, deductions)
        };
    }

    /// <summary>
    /// Salary is 1:1 with User now - there's no "list of past periods" to query. The change
    /// log shown in the UI is read back from Salary's history table instead (every insert/update
    /// is captured automatically, see EntityWithHistory / HistoryEntryFactory).
    /// </summary>
    private async Task<List<SalaryRecordDto>> BuildSalaryChangeLogAsync(int? salaryId, CancellationToken cancellationToken)
    {
        if (salaryId is null) return [];

        var entries = await context.Set<SalaryHistory>()
            .Where(h => h.Id == salaryId)
            .OrderByDescending(h => h.HistoryRecordedAt)
            .ToListAsync(cancellationToken);

        var performedByIds = entries
            .Where(h => h.HistoryRecordedById.HasValue)
            .Select(h => h.HistoryRecordedById!.Value)
            .Distinct()
            .ToList();

        var performedByNames = await context.Users.AsNoTracking()
            .Where(u => performedByIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => AppFormatter.BuildFullName(u.FirstName, u.LastName), cancellationToken);

        return entries.Select(h => new SalaryRecordDto
        {
            Id              = h.Id,
            Amount          = h.Amount,
            PayType         = h.PayType,
            Currency        = h.Currency,
            Period          = h.Period,
            PayrollCycle    = h.PayrollCycle,
            StartDate       = h.StartDate,
            EndDate         = h.EndDate,
            Notes           = h.Notes,
            NotesModifiedAt = h.NotesModifiedAt,
            ModifiedAt      = h.HistoryRecordedAt,
            ModifiedByName  = h.HistoryRecordedById.HasValue && performedByNames.TryGetValue(h.HistoryRecordedById.Value, out var name)
                ? name
                : null
        }).ToList();
    }

    public async Task<SalaryRecordDto> CreateSalaryRecordAsync(int userId, SalaryRecordDto dto, CancellationToken cancellationToken = default)
    {
        await context.Users.FindRequiredAsync(userId, nameof(User), cancellationToken);

        if (await context.Salaries.AnyAsync(s => s.UserId == userId, cancellationToken))
            throw new DomainConflictException("This employee already has a salary record. Edit the existing one instead.");

        EnsureSalaryDatesValid(dto);

        var entity = new Salary { UserId = userId };
        ApplySalaryDto(dto, entity);

        context.Salaries.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<SalaryRecordDto>(entity);
    }

    public async Task UpdateSalaryRecordAsync(int userId, int recordId, SalaryRecordDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await RequireSalaryRecordAsync(userId, recordId, cancellationToken);
        EnsureSalaryDatesValid(dto);
        ApplySalaryDto(dto, entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteSalaryRecordAsync(int userId, int recordId, CancellationToken cancellationToken = default)
    {
        var entity = await RequireSalaryRecordAsync(userId, recordId, cancellationToken);
        context.Salaries.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }


    public async Task<BenefitRecordDto> CreateBenefitRecordAsync(int userId, BenefitRecordDto dto, CancellationToken cancellationToken = default)
    {
        await context.Users.FindRequiredAsync(userId, nameof(User), cancellationToken);
        EnsureDependentsValid(dto);

        var entity = CreateBenefitEntity(dto.BenefitType, userId);
        ApplyBenefitDto(dto, entity);
        if (entity is HealthInsuranceBenefit healthInsurance)
            await ReplaceDependentsAsync(healthInsurance, dto.Dependents, cancellationToken);

        context.BenefitPayments.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<BenefitRecordDto>(entity);
    }

    public async Task UpdateBenefitRecordAsync(int userId, int recordId, BenefitRecordDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await RequireBenefitRecordAsync(userId, recordId, cancellationToken);
        EnsureDependentsValid(dto);

        // TPH: the benefit type dropdown is editable, but a row's concrete CLR type can't
        // change in place - if the type changed, replace the row instead of mutating it.
        if (!MatchesBenefitType(entity, dto.BenefitType))
        {
            context.BenefitPayments.Remove(entity);

            var replacement = CreateBenefitEntity(dto.BenefitType, userId);
            ApplyBenefitDto(dto, replacement);
            if (replacement is HealthInsuranceBenefit healthInsurance)
                await ReplaceDependentsAsync(healthInsurance, dto.Dependents, cancellationToken);

            context.BenefitPayments.Add(replacement);
        }
        else
        {
            ApplyBenefitDto(dto, entity);
            if (entity is HealthInsuranceBenefit healthInsurance)
                await ReplaceDependentsAsync(healthInsurance, dto.Dependents, cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteBenefitRecordAsync(int userId, int recordId, CancellationToken cancellationToken = default)
    {
        var entity = await RequireBenefitRecordAsync(userId, recordId, cancellationToken);
        context.BenefitPayments.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }


    public async Task<AdditionalPaymentDto> CreateAdditionalPaymentAsync(int userId, AdditionalPaymentDto dto, CancellationToken cancellationToken = default)
    {
        await context.Users.FindRequiredAsync(userId, nameof(User), cancellationToken);
        EnsureAdditionalPaymentDatesValid(dto);

        var entity = new AdditionalPayment { UserId = userId };
        ApplyAdditionalPaymentDto(dto, entity);
        entity.PaymentType = dto.PaymentType;

        context.AdditionalPayments.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<AdditionalPaymentDto>(entity);
    }

    public async Task UpdateAdditionalPaymentAsync(int userId, int paymentId, AdditionalPaymentDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await RequireAdditionalPaymentAsync(userId, paymentId, cancellationToken);
        EnsureAdditionalPaymentDatesValid(dto);

        ApplyAdditionalPaymentDto(dto, entity);
        entity.PaymentType = dto.PaymentType;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAdditionalPaymentAsync(int userId, int paymentId, CancellationToken cancellationToken = default)
    {
        var entity = await RequireAdditionalPaymentAsync(userId, paymentId, cancellationToken);
        context.AdditionalPayments.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }


    public async Task<DeductionDto> CreateDeductionAsync(int userId, DeductionDto dto, CancellationToken cancellationToken = default)
    {
        await context.Users.FindRequiredAsync(userId, nameof(User), cancellationToken);
        EnsureDeductionDatesValid(dto);

        var entity = new Deduction { UserId = userId };
        ApplyDeductionDto(dto, entity);
        entity.DeductionType = dto.DeductionType;

        context.Deductions.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<DeductionDto>(entity);
    }

    public async Task UpdateDeductionAsync(int userId, int deductionId, DeductionDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await RequireDeductionAsync(userId, deductionId, cancellationToken);
        EnsureDeductionDatesValid(dto);

        ApplyDeductionDto(dto, entity);
        entity.DeductionType = dto.DeductionType;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteDeductionAsync(int userId, int deductionId, CancellationToken cancellationToken = default)
    {
        var entity = await RequireDeductionAsync(userId, deductionId, cancellationToken);
        context.Deductions.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }


    private async Task<Salary> RequireSalaryRecordAsync(int userId, int recordId, CancellationToken cancellationToken)
        => await context.Salaries.FirstOrDefaultAsync(s => s.Id == recordId && s.UserId == userId, cancellationToken)
           ?? throw new EntityNotFoundException(nameof(Salary), recordId);

    private async Task<BenefitPayment> RequireBenefitRecordAsync(int userId, int recordId, CancellationToken cancellationToken)
        => await context.BenefitPayments.Include(b => (b as HealthInsuranceBenefit)!.Dependents)
               .FirstOrDefaultAsync(b => b.Id == recordId && b.UserId == userId, cancellationToken)
           ?? throw new EntityNotFoundException(nameof(BenefitPayment), recordId);

    private async Task<AdditionalPayment> RequireAdditionalPaymentAsync(int userId, int paymentId, CancellationToken cancellationToken)
        => await context.AdditionalPayments.FirstOrDefaultAsync(p => p.Id == paymentId && p.UserId == userId, cancellationToken)
           ?? throw new EntityNotFoundException(nameof(AdditionalPayment), paymentId);

    private async Task<Deduction> RequireDeductionAsync(int userId, int deductionId, CancellationToken cancellationToken)
        => await context.Deductions.FirstOrDefaultAsync(d => d.Id == deductionId && d.UserId == userId, cancellationToken)
           ?? throw new EntityNotFoundException(nameof(Deduction), deductionId);


    private static void ApplySalaryDto(SalaryRecordDto dto, Salary entity)
    {
        if (entity.Notes != dto.Notes)
            entity.NotesModifiedAt = DateTime.UtcNow;

        entity.Amount       = dto.Amount;
        entity.PayType      = dto.PayType;
        entity.Currency     = dto.Currency;
        entity.Period       = dto.Period;
        entity.PayrollCycle = dto.PayrollCycle;
        entity.StartDate    = dto.StartDate;
        entity.EndDate      = dto.EndDate;
        entity.Notes        = dto.Notes;
    }

    private static BenefitPayment CreateBenefitEntity(string benefitType, int userId) => benefitType switch
    {
        BenefitTypes.PrivateHealthInsurance => new HealthInsuranceBenefit { UserId = userId },
        BenefitTypes.PrivatePension         => new PensionBenefit { UserId = userId },
        _                                   => new OtherBenefit { UserId = userId }
    };

    private static bool MatchesBenefitType(BenefitPayment entity, string benefitType) => entity switch
    {
        HealthInsuranceBenefit => benefitType == BenefitTypes.PrivateHealthInsurance,
        PensionBenefit         => benefitType == BenefitTypes.PrivatePension,
        OtherBenefit           => benefitType == BenefitTypes.Other,
        _                      => false
    };

    private static void ApplyBenefitDto(BenefitRecordDto dto, BenefitPayment entity)
    {
        entity.BenefitName  = dto.BenefitName;
        entity.Amount       = dto.Amount;
        entity.Currency     = dto.Currency;
        entity.Period       = dto.Period;
        entity.StartDate    = dto.StartDate;
        entity.EndDate      = dto.EndDate;
        entity.ProviderName = dto.ProviderName;
        entity.Notes        = dto.Notes;

        if (entity is PensionBenefit pension)
        {
            pension.EmployeeContributionAmount = dto.EmployeeContributionAmount;
            pension.EmployerContributionAmount = dto.EmployerContributionAmount;
            pension.PolicyNumber               = dto.PolicyNumber;
        }
    }

    private static void ApplyAdditionalPaymentDto(AdditionalPaymentDto dto, AdditionalPayment entity)
    {
        entity.Amount       = dto.Amount;
        entity.Currency     = dto.Currency;
        entity.Period       = dto.Period;
        entity.StartDate    = dto.StartDate;
        entity.EndDate      = dto.EndDate;
        entity.Description  = dto.Description;
    }

    private static void ApplyDeductionDto(DeductionDto dto, Deduction entity)
    {
        entity.Amount        = dto.Amount;
        entity.Currency      = dto.Currency;
        entity.Period        = dto.Period;
        entity.StartDate     = dto.StartDate;
        entity.EndDate       = dto.EndDate;
        entity.Notes         = dto.Notes;
    }

    private async Task ReplaceDependentsAsync(HealthInsuranceBenefit entity, List<BenefitDependentDto> dependents, CancellationToken ct)
    {
        if (entity.Dependents.Count > 0)
            context.BenefitPaymentDependents.RemoveRange(entity.Dependents);

        entity.Dependents = [.. dependents.Select(d => new BenefitPaymentDependent
        {
            BenefitPaymentId = entity.Id,
            DependentName    = d.DependentName,
            RelationType     = d.RelationType,
            Amount           = d.Amount,
            StartDate        = d.StartDate,
            EndDate          = d.EndDate
        })];
    }


    private static void EnsureSalaryDatesValid(SalaryRecordDto dto)
    {
        if (dto.EndDate.HasValue && dto.EndDate.Value < dto.StartDate)
            throw new DomainValidationException("End date cannot be before start date.");
    }

    private static void EnsureDependentsValid(BenefitRecordDto dto)
    {
        if (dto.Dependents.Count == 0) return;

        if (dto.Dependents.Count > DZDDashboard.Common.Validation.ValidationConstants.MaxBenefitDependents)
            throw new DomainValidationException($"A benefit record may have at most {DZDDashboard.Common.Validation.ValidationConstants.MaxBenefitDependents} dependents.");

        foreach (var dependent in dto.Dependents)
        {
            if (dependent.EndDate.HasValue && dependent.EndDate.Value < dependent.StartDate)
                throw new DomainValidationException("A dependent's end date cannot be before its start date.");

            if (dependent.StartDate < dto.StartDate || (dto.EndDate.HasValue && dependent.EndDate.HasValue && dependent.EndDate.Value > dto.EndDate.Value))
                throw new DomainConflictException("A dependent's validity period cannot extend beyond the benefit record's period.");
        }
    }

    private static void EnsureAdditionalPaymentDatesValid(AdditionalPaymentDto dto)
    {
        if (dto.EndDate.HasValue && dto.EndDate.Value < dto.StartDate)
            throw new DomainValidationException("End date cannot be before start date.");
    }

    private static void EnsureDeductionDatesValid(DeductionDto dto)
    {
        if (dto.EndDate.HasValue && dto.EndDate.Value < dto.StartDate)
            throw new DomainValidationException("End date cannot be before start date.");
    }


    private static EmployeePaymentSummaryDto BuildSummary(
        List<BenefitPayment> benefits, List<AdditionalPayment> additionalPayments, List<Deduction> deductions)
    {
        var today = DateTime.UtcNow.Date;

        bool IsActive(DateTime start, DateTime? end) => start <= today && (end is null || end >= today);

        var activeBenefits    = benefits.Where(b => IsActive(b.StartDate, b.EndDate)).ToList();
        var activeAdditional = additionalPayments.Where(p => IsActive(p.StartDate, p.EndDate)).ToList();
        var activeDeductions  = deductions.Where(d => IsActive(d.StartDate, d.EndDate)).ToList();

        var benefitsTotal = new Dictionary<string, decimal>();
        foreach (var benefit in activeBenefits)
            benefitsTotal[benefit.Currency] = benefitsTotal.GetValueOrDefault(benefit.Currency) + BenefitTotalAmount(benefit);

        var deductionsTotal = new Dictionary<string, decimal>();
        foreach (var deduction in activeDeductions)
            deductionsTotal[deduction.Currency] = deductionsTotal.GetValueOrDefault(deduction.Currency) + deduction.Amount;

        var additionalTotal = new Dictionary<string, decimal>();
        foreach (var payment in activeAdditional)
            additionalTotal[payment.Currency] = additionalTotal.GetValueOrDefault(payment.Currency) + payment.Amount;

        return new EmployeePaymentSummaryDto
        {
            ActiveBenefitsTotal            = [.. benefitsTotal.Select(kv => new CurrencyAmountDto(kv.Key, kv.Value))],
            ActiveDeductionsTotal          = [.. deductionsTotal.Select(kv => new CurrencyAmountDto(kv.Key, kv.Value))],
            ActiveAdditionalPaymentsTotal  = [.. additionalTotal.Select(kv => new CurrencyAmountDto(kv.Key, kv.Value))]
        };
    }

    private static decimal BenefitTotalAmount(BenefitPayment benefit) => benefit is HealthInsuranceBenefit healthInsurance
        ? benefit.Amount + healthInsurance.Dependents.Sum(d => d.Amount)
        : benefit.Amount;
}
