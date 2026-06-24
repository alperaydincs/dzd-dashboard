using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Validation;
using FluentValidation;

namespace DZDDashboard.Api.Validators;

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
            edu.RuleFor(e => e.Level)
                .NotEmpty().WithMessage(ValidationMessages.EducationLevelRequired)
                .MaximumLength(ValidationConstants.MaxEducationLevelLength);

            edu.RuleFor(e => e.Institution)
                .NotEmpty().WithMessage(ValidationMessages.EducationInstitutionRequired)
                .MaximumLength(ValidationConstants.MaxInstitutionLength);

            edu.RuleFor(e => e.Status)
                .Must(s => s is null || EducationStatuses.All.Contains(s))
                .WithMessage($"Status must be one of: {string.Join(", ", EducationStatuses.All)}.");
        });
    }
}
