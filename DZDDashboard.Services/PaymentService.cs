using AutoMapper;
using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Exceptions;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using DZDDashboard.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;


public class PaymentService(AppDbContext context, IMapper mapper) : IPaymentService
{

    public async Task<EmployeePaymentDto> GetEmployeePaymentAsync(int userId, CancellationToken cancellationToken = default)
    {
        await context.Users.FindRequiredAsync(userId, nameof(User), cancellationToken);

        var salaryHistory = await context.SalaryHistories.AsNoTracking()
            .Include(s => s.ModifiedBy)
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.StartDate)
            .ToListAsync(cancellationToken);

        var benefits = await context.BenefitRecords.AsNoTracking()
            .AsSplitQuery()
            .Include(b => b.Dependents)
            .Include(b => b.ModifiedBy)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.StartDate)
            .ToListAsync(cancellationToken);

        foreach (var benefit in benefits)
            benefit.Dependents = [.. benefit.Dependents.OrderBy(d => d.Order)];

        var additionalPayments = await context.AdditionalPayments.AsNoTracking()
            .Include(p => p.ModifiedBy)
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.StartDate ?? p.PaymentDate ?? DateTime.MinValue)
            .ToListAsync(cancellationToken);

        var deductions = await context.Deductions.AsNoTracking()
            .Include(d => d.ModifiedBy)
            .Where(d => d.UserId == userId)
            .OrderByDescending(d => d.StartDate)
            .ToListAsync(cancellationToken);

        return new EmployeePaymentDto
        {
            EmployeeId         = userId,
            SalaryHistory      = mapper.Map<List<SalaryRecordDto>>(salaryHistory),
            Benefits           = mapper.Map<List<BenefitRecordDto>>(benefits),
            AdditionalPayments = mapper.Map<List<AdditionalPaymentDto>>(additionalPayments),
            Deductions         = mapper.Map<List<DeductionDto>>(deductions),
            Summary            = BuildSummary(salaryHistory, benefits, additionalPayments, deductions)
        };
    }

    public async Task<MyPaymentSummaryDto> GetMyPaymentSummaryAsync(int userId, CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;

        var activeSalary = await context.SalaryHistories.AsNoTracking()
            .Where(s => s.UserId == userId && s.StartDate <= today && (s.EndDate == null || s.EndDate >= today))
            .OrderByDescending(s => s.StartDate)
            .FirstOrDefaultAsync(cancellationToken);

        var pensionBenefits = await context.BenefitRecords.AsNoTracking()
            .Where(b => b.UserId == userId
                     && b.BenefitType == BenefitTypes.PrivatePension
                     && b.StartDate <= today && (b.EndDate == null || b.EndDate >= today))
            .OrderBy(b => b.Payer)
            .ToListAsync(cancellationToken);

        return new MyPaymentSummaryDto
        {
            ActiveSalary = activeSalary is null ? null : new SalaryRecordDto
            {
                Id          = activeSalary.Id,
                NetAmount   = activeSalary.NetAmount,
                GrossAmount = activeSalary.GrossAmount,
                PayType     = activeSalary.PayType,
                Currency    = activeSalary.Currency,
                Period      = activeSalary.Period,
                StartDate   = activeSalary.StartDate,
                EndDate     = activeSalary.EndDate
            },
            PensionBenefits = [.. pensionBenefits.Select(b => new BenefitRecordDto
            {
                Id          = b.Id,
                BenefitType = b.BenefitType,
                Payer       = b.Payer,
                Amount      = b.Amount,
                Currency    = b.Currency,
                Period      = b.Period,
                StartDate   = b.StartDate,
                EndDate     = b.EndDate
            })]
        };
    }


    public async Task<SalaryRecordDto> CreateSalaryRecordAsync(int userId, SalaryRecordDto dto, CancellationToken cancellationToken = default)
    {
        await context.Users.FindRequiredAsync(userId, nameof(User), cancellationToken);

        var existing = await context.SalaryHistories.AsNoTracking()
            .Where(s => s.UserId == userId)
            .Select(s => new { s.StartDate, s.EndDate })
            .ToListAsync(cancellationToken);

        EnsureNoOverlap(existing.Select(e => (e.StartDate, e.EndDate)), dto.StartDate, dto.EndDate, "salary");

        var entity = new SalaryHistory { UserId = userId };
        ApplySalaryDto(dto, entity);

        context.SalaryHistories.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<SalaryRecordDto>(entity);
    }

    public async Task UpdateSalaryRecordAsync(int userId, int recordId, SalaryRecordDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await RequireSalaryRecordAsync(userId, recordId, cancellationToken);

        var others = await context.SalaryHistories.AsNoTracking()
            .Where(s => s.UserId == userId && s.Id != recordId)
            .Select(s => new { s.StartDate, s.EndDate })
            .ToListAsync(cancellationToken);
        var otherRanges = others.Select(e => (e.StartDate, e.EndDate));

        var opensNewPeriod =
            entity.StartDate != dto.StartDate || entity.EndDate != dto.EndDate ||
            entity.NetAmount != dto.NetAmount || entity.Currency != dto.Currency;

        if (opensNewPeriod)
        {
            var closedEndDate = dto.StartDate.AddDays(-1);
            if (closedEndDate < entity.StartDate)
                closedEndDate = entity.StartDate;

            EnsureNoOverlap(otherRanges, entity.StartDate, closedEndDate, "salary");
            EnsureNoOverlap(otherRanges, dto.StartDate, dto.EndDate, "salary");

            entity.EndDate = closedEndDate;

            var newEntity = new SalaryHistory { UserId = userId };
            ApplySalaryDto(dto, newEntity);
            context.SalaryHistories.Add(newEntity);
        }
        else
        {
            EnsureNoOverlap(otherRanges, dto.StartDate, dto.EndDate, "salary");
            ApplySalaryDto(dto, entity);
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteSalaryRecordAsync(int userId, int recordId, CancellationToken cancellationToken = default)
    {
        var entity = await RequireSalaryRecordAsync(userId, recordId, cancellationToken);
        context.SalaryHistories.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }


    public async Task<BenefitRecordDto> CreateBenefitRecordAsync(int userId, BenefitRecordDto dto, CancellationToken cancellationToken = default)
    {
        await context.Users.FindRequiredAsync(userId, nameof(User), cancellationToken);
        EnsureDependentsValid(dto);

        var entity = new BenefitRecord { UserId = userId, Source = string.IsNullOrWhiteSpace(dto.Source) ? PaymentSources.Manual : dto.Source };
        ApplyBenefitDto(dto, entity);
        await ReplaceDependentsAsync(entity, dto.Dependents, cancellationToken);

        context.BenefitRecords.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<BenefitRecordDto>(entity);
    }

    public async Task UpdateBenefitRecordAsync(int userId, int recordId, BenefitRecordDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await RequireBenefitRecordAsync(userId, recordId, cancellationToken);
        EnsureDependentsValid(dto);

        ApplyBenefitDto(dto, entity);
        await ReplaceDependentsAsync(entity, dto.Dependents, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteBenefitRecordAsync(int userId, int recordId, CancellationToken cancellationToken = default)
    {
        var entity = await RequireBenefitRecordAsync(userId, recordId, cancellationToken);
        context.BenefitRecords.Remove(entity);        await context.SaveChangesAsync(cancellationToken);
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


    private async Task<SalaryHistory> RequireSalaryRecordAsync(int userId, int recordId, CancellationToken cancellationToken)
        => await context.SalaryHistories.FirstOrDefaultAsync(s => s.Id == recordId && s.UserId == userId, cancellationToken)
           ?? throw new EntityNotFoundException(nameof(SalaryHistory), recordId);

    private async Task<BenefitRecord> RequireBenefitRecordAsync(int userId, int recordId, CancellationToken cancellationToken)
        => await context.BenefitRecords.Include(b => b.Dependents)
               .FirstOrDefaultAsync(b => b.Id == recordId && b.UserId == userId, cancellationToken)
           ?? throw new EntityNotFoundException(nameof(BenefitRecord), recordId);

    private async Task<AdditionalPayment> RequireAdditionalPaymentAsync(int userId, int paymentId, CancellationToken cancellationToken)
        => await context.AdditionalPayments.FirstOrDefaultAsync(p => p.Id == paymentId && p.UserId == userId, cancellationToken)
           ?? throw new EntityNotFoundException(nameof(AdditionalPayment), paymentId);

    private async Task<Deduction> RequireDeductionAsync(int userId, int deductionId, CancellationToken cancellationToken)
        => await context.Deductions.FirstOrDefaultAsync(d => d.Id == deductionId && d.UserId == userId, cancellationToken)
           ?? throw new EntityNotFoundException(nameof(Deduction), deductionId);


    private static void ApplySalaryDto(SalaryRecordDto dto, SalaryHistory entity)
    {
        if (entity.Notes != dto.Notes)
            entity.NotesModifiedAt = DateTime.UtcNow;

        entity.NetAmount    = dto.NetAmount;
        entity.GrossAmount  = dto.GrossAmount;
        entity.PayType      = dto.PayType;
        entity.Currency     = dto.Currency;
        entity.Period       = dto.Period;
        entity.PayrollCycle = dto.PayrollCycle;
        entity.StartDate    = dto.StartDate;
        entity.EndDate      = dto.EndDate;
        entity.Notes        = dto.Notes;
    }

    private static void ApplyBenefitDto(BenefitRecordDto dto, BenefitRecord entity)
    {
        entity.BenefitType  = dto.BenefitType;
        entity.Payer        = dto.Payer;
        entity.BenefitName  = dto.BenefitName;
        entity.Amount       = dto.Amount;
        entity.Currency     = dto.Currency;
        entity.Period       = dto.Period;
        entity.StartDate    = dto.StartDate;
        entity.EndDate      = dto.EndDate;
        entity.ReferenceId  = dto.ReferenceId;
        entity.ProviderName = dto.ProviderName;
        entity.Notes        = dto.Notes;

        var isPension = dto.BenefitType == BenefitTypes.PrivatePension;
        entity.EmployeeContributionAmount = isPension ? dto.EmployeeContributionAmount : null;
        entity.EmployerContributionAmount = isPension ? dto.EmployerContributionAmount : null;
        entity.PolicyNumber               = isPension ? dto.PolicyNumber : null;
    }

    private static void ApplyAdditionalPaymentDto(AdditionalPaymentDto dto, AdditionalPayment entity)
    {
        entity.Amount       = dto.Amount;
        entity.Currency     = dto.Currency;
        entity.Period       = dto.Period;
        entity.PaymentDate  = dto.PaymentDate;
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
        entity.Notes         = dto.Notes;
    }

    private async Task ReplaceDependentsAsync(BenefitRecord entity, List<BenefitDependentDto> dependents, CancellationToken ct)
    {
        if (entity.Dependents.Count > 0)
            context.BenefitDependents.RemoveRange(entity.Dependents);

        entity.Dependents = [.. dependents.Select((d, index) => new BenefitDependent
        {
            BenefitRecordId = entity.Id,
            Order           = index + 1,
            DependentName   = d.DependentName,
            RelationType    = d.RelationType,
            Amount          = d.Amount,
            StartDate       = d.StartDate,
            EndDate         = d.EndDate
        })];
    }


    private static void EnsureNoOverlap(IEnumerable<(DateTime StartDate, DateTime? EndDate)> existingRanges,
        DateTime newStart, DateTime? newEnd, string recordKind)
    {
        if (newEnd.HasValue && newEnd.Value < newStart)
            throw new DomainValidationException("End date cannot be before start date.");

        if (existingRanges.Any(r => RangesOverlap(r.StartDate, r.EndDate, newStart, newEnd)))
            throw new DomainConflictException($"This {recordKind} period overlaps with an existing record. Close the active record first or adjust the date range.");
    }

    private static bool RangesOverlap(DateTime startA, DateTime? endA, DateTime startB, DateTime? endB)
    {
        var endAValue = endA ?? DateTime.MaxValue;
        var endBValue = endB ?? DateTime.MaxValue;
        return startA < endBValue && startB < endAValue;
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

            if (dependent.StartDate < dto.StartDate || (dto.EndDate.HasValue && (dependent.EndDate ?? dependent.StartDate) > dto.EndDate.Value))
                throw new DomainConflictException("A dependent's validity period cannot extend beyond the benefit record's period.");
        }
    }

    private static void EnsureAdditionalPaymentDatesValid(AdditionalPaymentDto dto)
    {
        if (dto.Period == AdditionalPaymentPeriods.OneTime)
        {
            if (dto.PaymentDate is null)
                throw new DomainValidationException("Payment date is required for one-time additional payments.");
        }
        else
        {
            if (dto.StartDate is null)
                throw new DomainValidationException("Start date is required for recurring additional payments.");

            if (dto.EndDate.HasValue && dto.EndDate.Value < dto.StartDate.Value)
                throw new DomainValidationException("End date cannot be before start date.");
        }
    }


    private static EmployeePaymentSummaryDto BuildSummary(
        List<SalaryHistory> salaryHistory, List<BenefitRecord> benefits, List<AdditionalPayment> additionalPayments, List<Deduction> deductions)
    {
        var today = DateTime.UtcNow.Date;
        var horizon = today.AddDays(30);

        bool IsActive(DateTime start, DateTime? end) => start <= today && (end is null || end >= today);
        bool ExpiresWithinHorizon(DateTime? end) => end.HasValue && end.Value >= today && end.Value <= horizon;

        var activeSalary    = salaryHistory.Where(s => IsActive(s.StartDate, s.EndDate)).ToList();
        var activeBenefits  = benefits.Where(b => IsActive(b.StartDate, b.EndDate)).ToList();
        var activeAdditional = additionalPayments.Where(p =>
            p.Period == AdditionalPaymentPeriods.OneTime
                ? p.PaymentDate.HasValue && p.PaymentDate.Value.Year == today.Year && p.PaymentDate.Value.Month == today.Month
                : p.StartDate.HasValue && IsActive(p.StartDate.Value, p.EndDate)).ToList();
        var activeDeductions = deductions.Where(d => d.StartDate <= today).ToList();

        var monthlyCost = new Dictionary<string, decimal>();
        void AddMonthlyCost(string currency, decimal? amount)
        {
            if (amount is not { } value) return;
            monthlyCost[currency] = monthlyCost.GetValueOrDefault(currency) + value;
        }

        foreach (var salary in activeSalary)
            AddMonthlyCost(salary.Currency, NormalizeToMonthly(salary.NetAmount, salary.Period));

        foreach (var benefit in activeBenefits.Where(b => b.Payer == BenefitPayers.Employer))
            AddMonthlyCost(benefit.Currency, NormalizeToMonthly(benefit.Amount, benefit.Period));

        foreach (var payment in activeAdditional)
            AddMonthlyCost(payment.Currency, payment.Period == AdditionalPaymentPeriods.OneTime ? payment.Amount : NormalizeToMonthly(payment.Amount, payment.Period));

        var benefitsTotal = new Dictionary<string, decimal>();
        foreach (var benefit in activeBenefits)
            benefitsTotal[benefit.Currency] = benefitsTotal.GetValueOrDefault(benefit.Currency) + benefit.Amount;

        var deductionsTotal = new Dictionary<string, decimal>();
        foreach (var deduction in activeDeductions)
            deductionsTotal[deduction.Currency] = deductionsTotal.GetValueOrDefault(deduction.Currency) + deduction.Amount;

        var upcomingExpirations =
            salaryHistory.Count(s => ExpiresWithinHorizon(s.EndDate)) +
            benefits.Count(b => ExpiresWithinHorizon(b.EndDate)) +
            additionalPayments.Count(p => ExpiresWithinHorizon(p.EndDate));

        return new EmployeePaymentSummaryDto
        {
            EstimatedMonthlyCost     = [.. monthlyCost.Select(kv => new CurrencyAmountDto(kv.Key, kv.Value))],
            ActiveBenefitsTotal      = [.. benefitsTotal.Select(kv => new CurrencyAmountDto(kv.Key, kv.Value))],
            ActiveDeductionsTotal    = [.. deductionsTotal.Select(kv => new CurrencyAmountDto(kv.Key, kv.Value))],
            ActiveItemCount          = activeSalary.Count + activeBenefits.Count + activeAdditional.Count + activeDeductions.Count,
            UpcomingExpirationCount  = upcomingExpirations
        };
    }

    private static decimal? NormalizeToMonthly(decimal amount, string period) => period switch
    {
        PaymentPeriods.Monthly => amount,
        PaymentPeriods.Yearly  => amount / 12m,
        PaymentPeriods.Weekly  => amount * 52m / 12m,
        _ => null
    };
}
