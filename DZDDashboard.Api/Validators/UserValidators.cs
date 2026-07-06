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
            .Must(c => DomainOptionCatalog.IsValid(DomainCategories.ContractType, c))
            .When(x => !string.IsNullOrEmpty(x.ContractType));
        RuleFor(x => x.WorkModel)
            .Must(c => DomainOptionCatalog.IsValid(DomainCategories.WorkModel, c))
            .When(x => !string.IsNullOrEmpty(x.WorkModel));
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

public class UpdateCitizenshipInfoDtoValidator : AbstractValidator<UpdateCitizenshipInfoDto>
{
    public UpdateCitizenshipInfoDtoValidator()
    {
        RuleFor(x => x.Gender)
            .Must(g => g is null || GenderValues.All.Contains(g))
            .WithMessage($"Gender must be one of: {string.Join(", ", GenderValues.All)}.");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.UtcNow).When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth cannot be in the future.")
            .GreaterThan(new DateTime(1900, 1, 1)).When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth is not plausible.");

        RuleFor(x => x.Nationality).MaximumLength(ValidationConstants.MaxNameLength);
        RuleFor(x => x.DisabilityDegree)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .Empty().When(x => !x.DisabilityStatus)
            .WithMessage("Disability degree must be empty when disability status is false.");

        RuleFor(x => x.CitizenshipNumber)
            .MaximumLength(ValidationConstants.MaxNumericIdentifierLength)
            .Matches(@"^\d+$").When(x => !string.IsNullOrWhiteSpace(x.CitizenshipNumber))
            .WithMessage("Citizenship number must contain digits only.");
    }
}

public class UpdateFamilyInfoDtoValidator : AbstractValidator<UpdateFamilyInfoDto>
{
    public UpdateFamilyInfoDtoValidator()
    {
        RuleFor(x => x.MaritalStatus)
            .Must(s => s is null || MaritalStatuses.All.Contains(s))
            .WithMessage($"Marital status must be one of: {string.Join(", ", MaritalStatuses.All)}.");

        RuleFor(x => x.SpouseFullName).MaximumLength(ValidationConstants.MaxFullNameLength);

        RuleForEach(x => x.Children).ChildRules(child =>
        {
            child.RuleFor(c => c.FullName)
                .NotEmpty().WithMessage(ValidationMessages.ChildFullNameRequired)
                .MaximumLength(ValidationConstants.MaxFullNameLength);

            child.RuleFor(c => c.DateOfBirth)
                .NotNull().WithMessage("Child date of birth is required.")
                .LessThan(DateTime.UtcNow).When(c => c.DateOfBirth.HasValue)
                .WithMessage("Child date of birth cannot be in the future.");
        });
    }
}

public class UpdateEmergencyContactsDtoValidator : AbstractValidator<UpdateEmergencyContactsDto>
{
    public UpdateEmergencyContactsDtoValidator()
    {
        RuleForEach(x => x.EmergencyContacts).ChildRules(contact =>
        {
            contact.RuleFor(c => c.FullName)
                .NotEmpty().WithMessage(ValidationMessages.EmergencyFullNameRequired)
                .MaximumLength(ValidationConstants.MaxFullNameLength);

            contact.RuleFor(c => c.Relationship)
                .NotEmpty().WithMessage(ValidationMessages.EmergencyRelationRequired)
                .MaximumLength(ValidationConstants.MaxNameLength);

            contact.RuleFor(c => c.PhoneNumber)
                .NotEmpty().WithMessage(ValidationMessages.EmergencyPhoneRequired)
                .MaximumLength(ValidationConstants.MaxPhoneLength)
                .Must(p => string.IsNullOrWhiteSpace(p) || PhoneValidator.IsValid(p))
                .WithMessage(ValidationMessages.EmergencyPhoneInvalid);
        });
    }
}

public class UpdateEducationInfoDtoValidator : AbstractValidator<UpdateEducationInfoDto>
{
    public UpdateEducationInfoDtoValidator()
    {
        RuleForEach(x => x.EducationHistories).ChildRules(edu =>
        {
            edu.RuleFor(e => e.EducationLevel)
                .Must(l => DomainOptionCatalog.IsValid(DomainCategories.EducationLevel, l))
                .WithMessage(ValidationMessages.EducationLevelRequired);

            edu.RuleFor(e => e.Institution)
                .NotEmpty().WithMessage(ValidationMessages.EducationInstitutionRequired)
                .MaximumLength(ValidationConstants.MaxInstitutionLength);

            edu.RuleFor(e => e.Program)
                .MaximumLength(ValidationConstants.MaxStandardLength);

            edu.RuleFor(e => e.Status)
                .Must(s => s is null || EducationStatuses.All.Contains(s))
                .WithMessage($"Status must be one of: {string.Join(", ", EducationStatuses.All)}.");
        });
    }
}
