using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Validation;
using FluentValidation;

namespace DZDDashboard.Api.Validators;

/// <summary>
/// Input-shape validation for the Payment screen DTOs — required fields, lengths and
/// allowed-value-set membership. Cross-record business rules (overlap checks, dependent
/// caps/ranges, OneTime date requirements — BR-PAY-*) are enforced in <c>PaymentService</c>
/// because they need database state; keeping them out of here avoids dual-validation drift
/// (same convention as <c>UpdateEducationInfoDtoValidator</c>).
/// </summary>
public class SalaryRecordDtoValidator : AbstractValidator<SalaryRecordDto>
{
    public SalaryRecordDtoValidator()
    {
        RuleFor(x => x.NetAmount)
            .GreaterThan(0).WithMessage(ValidationMessages.SalaryAmountInvalid);

        RuleFor(x => x.GrossAmount)
            .GreaterThan(0).When(x => x.GrossAmount.HasValue)
            .WithMessage(ValidationMessages.SalaryAmountInvalid);

        RuleFor(x => x.Currency)
            .Must(c => Currencies.All.Contains(c)).WithMessage(ValidationMessages.CurrencyInvalid);

        RuleFor(x => x.Period)
            .Must(p => PaymentPeriods.All.Contains(p)).WithMessage(ValidationMessages.SalaryPeriodInvalid);

        RuleFor(x => x.PayrollCycle)
            .MaximumLength(ValidationConstants.MaxStandardLength);

        RuleFor(x => x.Notes)
            .MaximumLength(ValidationConstants.MaxNotesLength);

        RuleFor(x => x.StartDate)
            .NotEqual(default(DateTime)).WithMessage(ValidationMessages.SalaryStartDateRequired);

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate).When(x => x.EndDate.HasValue)
            .WithMessage("End date cannot be before start date.");
    }
}

public class BenefitDependentDtoValidator : AbstractValidator<BenefitDependentDto>
{
    public BenefitDependentDtoValidator()
    {
        RuleFor(x => x.DependentType)
            .NotEmpty().WithMessage(ValidationMessages.DependentTypeRequired)
            .Must(t => DependentTypes.All.Contains(t)).WithMessage(ValidationMessages.DependentTypeRequired);

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage(ValidationMessages.DependentAmountInvalid);

        RuleFor(x => x.StartDate)
            .NotEqual(default(DateTime)).WithMessage(ValidationMessages.BenefitStartDateRequired);

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate).When(x => x.EndDate.HasValue)
            .WithMessage("End date cannot be before start date.");
    }
}

public class BenefitRecordDtoValidator : AbstractValidator<BenefitRecordDto>
{
    public BenefitRecordDtoValidator()
    {
        RuleFor(x => x.BenefitType)
            .Must(t => BenefitTypes.All.Contains(t)).WithMessage(ValidationMessages.BenefitTypeInvalid);

        RuleFor(x => x.Payer)
            .Must(p => BenefitPayers.All.Contains(p)).WithMessage(ValidationMessages.BenefitPayerInvalid);

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage(ValidationMessages.BenefitAmountInvalid);

        RuleFor(x => x.Currency)
            .Must(c => Currencies.All.Contains(c)).WithMessage(ValidationMessages.CurrencyInvalid);

        RuleFor(x => x.Period)
            .Must(p => PaymentPeriods.All.Contains(p)).WithMessage(ValidationMessages.SalaryPeriodInvalid);

        RuleFor(x => x.StartDate)
            .NotEqual(default(DateTime)).WithMessage(ValidationMessages.BenefitStartDateRequired);

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate).When(x => x.EndDate.HasValue)
            .WithMessage("End date cannot be before start date.");

        RuleFor(x => x.ReferenceId).MaximumLength(ValidationConstants.MaxReferenceCodeLength);
        RuleFor(x => x.ProviderName).MaximumLength(ValidationConstants.MaxProviderNameLength);
        RuleFor(x => x.Notes).MaximumLength(ValidationConstants.MaxNotesLength);

        RuleFor(x => x.Dependents)
            .Must(d => d.Count <= ValidationConstants.MaxBenefitDependents)
            .WithMessage($"A benefit record may have at most {ValidationConstants.MaxBenefitDependents} dependents.");

        RuleForEach(x => x.Dependents).SetValidator(new BenefitDependentDtoValidator());
    }
}

public class AdditionalPaymentDtoValidator : AbstractValidator<AdditionalPaymentDto>
{
    public AdditionalPaymentDtoValidator()
    {
        RuleFor(x => x.PaymentType)
            .Must(t => AdditionalPaymentTypes.All.Contains(t)).WithMessage(ValidationMessages.AdditionalPaymentTypeInvalid);

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage(ValidationMessages.AdditionalPaymentAmountInvalid);

        RuleFor(x => x.Currency)
            .Must(c => Currencies.All.Contains(c)).WithMessage(ValidationMessages.CurrencyInvalid);

        RuleFor(x => x.Period)
            .Must(p => AdditionalPaymentPeriods.All.Contains(p)).WithMessage(ValidationMessages.AdditionalPaymentPeriodInvalid);

        RuleFor(x => x.Description).MaximumLength(ValidationConstants.MaxNotesLength);

        // BR-PAY-06 (date-required-by-period) is enforced in PaymentService — it's a
        // cross-field business rule, not a pure input-shape check, and the service already
        // needs to branch on Period to decide which date(s) to persist.
    }
}
