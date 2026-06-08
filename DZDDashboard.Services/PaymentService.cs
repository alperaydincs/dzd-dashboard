using AutoMapper;
using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Exceptions;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using DZDDashboard.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;

// Interface is in Abstractions/IPaymentService.cs

/// <summary>
/// Implements the Employee Profile → Payment screen (Salary / Benefits / Additional Payments).
/// FluentValidation (<c>PaymentValidators.cs</c>) covers input shape — required fields, lengths,
/// allowed-value-set membership. This service covers the BR-PAY-* business rules that need
/// database state: overlap checks, dependent caps/ranges and OneTime date requirements.
/// </summary>
public class PaymentService(AppDbContext context, IMapper mapper) : IPaymentService
{
    // ── Reads ────────────────────────────────────────────────────────────────

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

        return new EmployeePaymentDto
        {
            EmployeeId         = userId,
            SalaryHistory      = mapper.Map<List<SalaryRecordDto>>(salaryHistory),
            Benefits           = mapper.Map<List<BenefitRecordDto>>(benefits),
            AdditionalPayments = mapper.Map<List<AdditionalPaymentDto>>(additionalPayments),
            Summary            = BuildSummary(salaryHistory, benefits, additionalPayments)
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

        // Self-service is read-only and PII-light: project only the fields the employee needs to
        // see (amount/currency/period/validity). Audit, source/reference and notes are omitted —
        // mirrors the EmployeeCardDto PII-trap fix (don't ship fields the viewer shouldn't read).
        return new MyPaymentSummaryDto
        {
            ActiveSalary = activeSalary is null ? null : new SalaryRecordDto
            {
                Id          = activeSalary.Id,
                NetAmount   = activeSalary.NetAmount,
                GrossAmount = activeSalary.GrossAmount,
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

    // ── Salary ───────────────────────────────────────────────────────────────

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

        EnsureNoOverlap(others.Select(e => (e.StartDate, e.EndDate)), dto.StartDate, dto.EndDate, "salary");

        ApplySalaryDto(dto, entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteSalaryRecordAsync(int userId, int recordId, CancellationToken cancellationToken = default)
    {
        var entity = await RequireSalaryRecordAsync(userId, recordId, cancellationToken);
        context.SalaryHistories.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    // ── Benefits ─────────────────────────────────────────────────────────────

    public async Task<BenefitRecordDto> CreateBenefitRecordAsync(int userId, BenefitRecordDto dto, CancellationToken cancellationToken = default)
    {
        await context.Users.FindRequiredAsync(userId, nameof(User), cancellationToken);
        EnsureDependentsValid(dto);

        var entity = new BenefitRecord { UserId = userId, Source = string.IsNullOrWhiteSpace(dto.Source) ? PaymentSources.Manual : dto.Source };
        ApplyBenefitDto(dto, entity);

        context.BenefitRecords.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<BenefitRecordDto>(entity);
    }

    public async Task UpdateBenefitRecordAsync(int userId, int recordId, BenefitRecordDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await RequireBenefitRecordAsync(userId, recordId, cancellationToken);
        EnsureDependentsValid(dto);

        ApplyBenefitDto(dto, entity);
        ReplaceDependents(entity, dto.Dependents);

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteBenefitRecordAsync(int userId, int recordId, CancellationToken cancellationToken = default)
    {
        var entity = await RequireBenefitRecordAsync(userId, recordId, cancellationToken);
        context.BenefitRecords.Remove(entity); // cascades to Dependents
        await context.SaveChangesAsync(cancellationToken);
    }

    // ── Additional Payments ──────────────────────────────────────────────────

    public async Task<AdditionalPaymentDto> CreateAdditionalPaymentAsync(int userId, AdditionalPaymentDto dto, CancellationToken cancellationToken = default)
    {
        await context.Users.FindRequiredAsync(userId, nameof(User), cancellationToken);
        EnsureAdditionalPaymentDatesValid(dto);

        var entity = new AdditionalPayment { UserId = userId };
        ApplyAdditionalPaymentDto(dto, entity);

        context.AdditionalPayments.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<AdditionalPaymentDto>(entity);
    }

    public async Task UpdateAdditionalPaymentAsync(int userId, int paymentId, AdditionalPaymentDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await RequireAdditionalPaymentAsync(userId, paymentId, cancellationToken);
        EnsureAdditionalPaymentDatesValid(dto);

        ApplyAdditionalPaymentDto(dto, entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAdditionalPaymentAsync(int userId, int paymentId, CancellationToken cancellationToken = default)
    {
        var entity = await RequireAdditionalPaymentAsync(userId, paymentId, cancellationToken);
        context.AdditionalPayments.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    // ── Lookups ──────────────────────────────────────────────────────────────

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

    // ── Apply DTO → entity ───────────────────────────────────────────────────

    private static void ApplySalaryDto(SalaryRecordDto dto, SalaryHistory entity)
    {
        entity.NetAmount    = dto.NetAmount;
        entity.GrossAmount  = dto.GrossAmount;
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
        entity.Amount       = dto.Amount;
        entity.Currency     = dto.Currency;
        entity.Period       = dto.Period;
        entity.StartDate    = dto.StartDate;
        entity.EndDate      = dto.EndDate;
        entity.ReferenceId  = dto.ReferenceId;
        entity.ProviderName = dto.ProviderName;
        entity.Notes        = dto.Notes;
        // Source is set on creation only — never overwritten on update so an Onboarding-sourced
        // row keeps its provenance even when HR edits the amount later (BR-PAY-04 traceability).
    }

    private static void ApplyAdditionalPaymentDto(AdditionalPaymentDto dto, AdditionalPayment entity)
    {
        entity.PaymentType  = dto.PaymentType;
        entity.Amount       = dto.Amount;
        entity.Currency     = dto.Currency;
        entity.Period       = dto.Period;
        entity.PaymentDate  = dto.PaymentDate;
        entity.StartDate    = dto.StartDate;
        entity.EndDate      = dto.EndDate;
        entity.TaxableFlag  = dto.TaxableFlag;
        entity.Description  = dto.Description;
    }

    /// <summary>
    /// Replaces a benefit record's dependent rows wholesale (max 5, small list — bulk replace
    /// is simpler and safer here than a diff/merge) and re-numbers <c>Order</c> 1..N so display
    /// order always matches array position even after a middle row is removed (BR-PAY-03).
    /// </summary>
    private void ReplaceDependents(BenefitRecord entity, List<BenefitDependentDto> dependents)
    {
        if (entity.Dependents.Count > 0)
            context.BenefitDependents.RemoveRange(entity.Dependents);

        entity.Dependents = [.. dependents.Select((d, index) => new BenefitDependent
        {
            BenefitRecordId = entity.Id,
            Order           = index + 1,
            DependentType   = d.DependentType,
            Amount          = d.Amount,
            StartDate       = d.StartDate,
            EndDate         = d.EndDate
        })];
    }

    // ── Business-rule validation ─────────────────────────────────────────────

    /// <summary>BR-PAY-01: a user's salary validity periods may not overlap.</summary>
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

    /// <summary>
    /// BR-PAY-03: at most 5 dependents, each with type + amount, and each dependent's validity
    /// range must fall within the parent benefit record's range.
    /// </summary>
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

    /// <summary>BR-PAY-06 + range sanity: OneTime needs a payment date; periodic needs a start date.</summary>
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

    // ── Summary ("Toplamlar") ────────────────────────────────────────────────

    private static EmployeePaymentSummaryDto BuildSummary(
        List<SalaryHistory> salaryHistory, List<BenefitRecord> benefits, List<AdditionalPayment> additionalPayments)
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

        var upcomingExpirations =
            salaryHistory.Count(s => ExpiresWithinHorizon(s.EndDate)) +
            benefits.Count(b => ExpiresWithinHorizon(b.EndDate)) +
            additionalPayments.Count(p => ExpiresWithinHorizon(p.EndDate));

        return new EmployeePaymentSummaryDto
        {
            EstimatedMonthlyCost     = [.. monthlyCost.Select(kv => new CurrencyAmountDto(kv.Key, kv.Value))],
            ActiveBenefitsTotal      = [.. benefitsTotal.Select(kv => new CurrencyAmountDto(kv.Key, kv.Value))],
            ActiveItemCount          = activeSalary.Count + activeBenefits.Count + activeAdditional.Count,
            UpcomingExpirationCount  = upcomingExpirations
        };
    }

    /// <summary>
    /// Normalises a recurring amount to its monthly equivalent for the cost-estimate card.
    /// "Hourly" cannot be normalised without contracted working hours — returns null and the
    /// amount is excluded from the estimate (BR-PAY-05 / open question: precise FX & normalisation
    /// rules are still undecided per the analysis doc — this keeps the estimate conservative
    /// rather than guessing).
    /// </summary>
    private static decimal? NormalizeToMonthly(decimal amount, string period) => period switch
    {
        // PaymentPeriods.Monthly/.Weekly share their literal values with AdditionalPaymentPeriods —
        // a single case covers both DTOs' periodic values.
        PaymentPeriods.Monthly => amount,
        PaymentPeriods.Yearly  => amount / 12m,
        PaymentPeriods.Weekly  => amount * 52m / 12m,
        _ => null
    };
}
