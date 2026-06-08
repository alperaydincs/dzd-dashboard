using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Validation;
using FluentValidation;

namespace DZDDashboard.Api.Validators;

public class UpdateContactInfoDtoValidator : AbstractValidator<UpdateContactInfoDto>
{
    public UpdateContactInfoDtoValidator()
    {
        RuleFor(x => x.PersonalEmail)
            .MaximumLength(ValidationConstants.MaxEmailLength)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.PersonalEmail))
            .WithMessage(ValidationMessages.PersonalEmailInvalid);

        RuleFor(x => x.WorkPhoneNumber)
            .MaximumLength(ValidationConstants.MaxPhoneLength)
            .Must(p => string.IsNullOrWhiteSpace(p) || PhoneValidator.IsValid(p))
            .WithMessage(ValidationMessages.WorkPhoneInvalid);

        RuleFor(x => x.PersonalPhoneNumber)
            .MaximumLength(ValidationConstants.MaxPhoneLength)
            .Must(p => string.IsNullOrWhiteSpace(p) || PhoneValidator.IsValid(p))
            .WithMessage(ValidationMessages.PersonalPhoneInvalid);
    }
}

public class UpdateContactsDtoValidator : AbstractValidator<UpdateContactsDto>
{
    public UpdateContactsDtoValidator()
    {
        RuleFor(x => x.Email)
            .MaximumLength(ValidationConstants.MaxEmailLength)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage(ValidationMessages.WorkEmailInvalid);

        RuleFor(x => x.PersonalEmail)
            .MaximumLength(ValidationConstants.MaxEmailLength)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.PersonalEmail))
            .WithMessage(ValidationMessages.PersonalEmailInvalid);

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(ValidationConstants.MaxPhoneLength)
            .Must(p => string.IsNullOrWhiteSpace(p) || PhoneValidator.IsValid(p))
            .WithMessage(ValidationMessages.WorkPhoneInvalid);

        RuleFor(x => x.PersonalPhoneNumber)
            .MaximumLength(ValidationConstants.MaxPhoneLength)
            .Must(p => string.IsNullOrWhiteSpace(p) || PhoneValidator.IsValid(p))
            .WithMessage(ValidationMessages.PersonalPhoneInvalid);
    }
}

public class UpdateBasicInfoDtoValidator : AbstractValidator<UpdateBasicInfoDto>
{
    public UpdateBasicInfoDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .Must(n => n is null || !string.IsNullOrWhiteSpace(n))
            .WithMessage("First name cannot be whitespace only.");

        RuleFor(x => x.LastName)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .Must(n => n is null || !string.IsNullOrWhiteSpace(n))
            .WithMessage("Last name cannot be whitespace only.");

        RuleFor(x => x.RegistrationNumber).MaximumLength(ValidationConstants.MaxShortNameLength);

        RuleFor(x => x.ContractEndDate)
            .GreaterThan(x => x.UserStartDate)
            .When(x => x.ContractEndDate.HasValue && x.UserStartDate.HasValue)
            .WithMessage("Contract end date must be after the user start date.");

        RuleFor(x => x.PositionStartDate)
            .GreaterThanOrEqualTo(x => x.UserStartDate)
            .When(x => x.PositionStartDate.HasValue && x.UserStartDate.HasValue)
            .WithMessage("Position start date must be on or after user start date.");

        RuleFor(x => x.PositionStartDate)
            .LessThanOrEqualTo(x => x.ContractEndDate)
            .When(x => x.PositionStartDate.HasValue && x.ContractEndDate.HasValue)
            .WithMessage("Position start date must be on or before contract end date.");

        RuleFor(x => x.ContractType)
            .Must(v => v is null || ContractTypes.All.Contains(v))
            .WithMessage($"Contract type must be one of: {string.Join(", ", ContractTypes.All)}.");

        RuleFor(x => x.WorkModel)
            .Must(v => v is null || WorkModels.All.Contains(v))
            .WithMessage($"Work model must be one of: {string.Join(", ", WorkModels.All)}.");
    }
}

public class UpdateAddressInfoDtoValidator : AbstractValidator<UpdateAddressInfoDto>
{
    public UpdateAddressInfoDtoValidator()
    {
        RuleFor(x => x.LegalAddress).MaximumLength(ValidationConstants.MaxAddressLength);
        RuleFor(x => x.CurrentAddress).MaximumLength(ValidationConstants.MaxAddressLength);
        RuleFor(x => x.City).MaximumLength(ValidationConstants.MaxNameLength);
        RuleFor(x => x.Country).MaximumLength(ValidationConstants.MaxNameLength);
    }
}
