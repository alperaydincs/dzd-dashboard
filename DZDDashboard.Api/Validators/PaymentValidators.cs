using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Validation;
using FluentValidation;

namespace DZDDashboard.Api.Validators;

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

        RuleFor(x => x.PayType)
            .Must(t => PayTypes.All.Contains(t)).WithMessage(ValidationMessages.SalaryPayTypeInvalid);

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
        RuleFor(x => x.DependentName)
            .MaximumLength(ValidationConstants.MaxShortNameLength);

        RuleFor(x => x.RelationType)
            .Must(c => DomainOptionCatalog.IsValid(DomainCategories.RelationType, c))
            .WithMessage(ValidationMessages.RelationTypeRequired);

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
        RuleFor(x => x.BenefitName).MaximumLength(ValidationConstants.MaxBenefitNameLength);
        RuleFor(x => x.PolicyNumber).MaximumLength(ValidationConstants.MaxPolicyNumberLength);

        RuleFor(x => x.EmployeeContributionAmount)
            .GreaterThanOrEqualTo(0).When(x => x.EmployeeContributionAmount.HasValue)
            .WithMessage("Employee contribution amount cannot be negative.");

        RuleFor(x => x.EmployerContributionAmount)
            .GreaterThanOrEqualTo(0).When(x => x.EmployerContributionAmount.HasValue)
            .WithMessage("Employer contribution amount cannot be negative.");

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
            .Must(c => DomainOptionCatalog.IsValid(DomainCategories.AdditionalPaymentType, c))
            .WithMessage(ValidationMessages.AdditionalPaymentTypeInvalid);

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage(ValidationMessages.AdditionalPaymentAmountInvalid);

        RuleFor(x => x.Currency)
            .Must(c => Currencies.All.Contains(c)).WithMessage(ValidationMessages.CurrencyInvalid);

        RuleFor(x => x.Period)
            .Must(p => AdditionalPaymentPeriods.All.Contains(p)).WithMessage(ValidationMessages.AdditionalPaymentPeriodInvalid);

        RuleFor(x => x.Description).MaximumLength(ValidationConstants.MaxNotesLength);

    }
}

public class DeductionDtoValidator : AbstractValidator<DeductionDto>
{
    public DeductionDtoValidator()
    {
        RuleFor(x => x.DeductionType)
            .Must(c => DomainOptionCatalog.IsValid(DomainCategories.DeductionType, c))
            .WithMessage(ValidationMessages.DeductionTypeInvalid);

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage(ValidationMessages.DeductionAmountInvalid);

        RuleFor(x => x.Currency)
            .Must(c => Currencies.All.Contains(c)).WithMessage(ValidationMessages.CurrencyInvalid);

        RuleFor(x => x.Period)
            .Must(p => PaymentPeriods.All.Contains(p)).WithMessage(ValidationMessages.SalaryPeriodInvalid);

        RuleFor(x => x.Notes).MaximumLength(ValidationConstants.MaxNotesLength);

        RuleFor(x => x.StartDate)
            .NotEqual(default(DateTime)).WithMessage(ValidationMessages.DeductionStartDateRequired);
    }
}
