using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Validation;
using FluentValidation;

namespace DZDDashboard.Api.Validators;

public class StartOnboardingDtoValidator : AbstractValidator<StartOnboardingDto>
{
    public StartOnboardingDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(ValidationConstants.MaxNameLength);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(ValidationConstants.MaxNameLength);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ValidationMessages.WorkEmailRequired)
            .MaximumLength(ValidationConstants.MaxEmailLength)
            .EmailAddress().WithMessage(ValidationMessages.WorkEmailInvalid);

        RuleFor(x => x.PersonalEmail)
            .MaximumLength(ValidationConstants.MaxEmailLength)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.PersonalEmail))
            .WithMessage(ValidationMessages.PersonalEmailInvalid);
    }
}
