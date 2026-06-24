using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Validation;
using FluentValidation;

namespace DZDDashboard.Api.Validators;

public class RoleDurationDtoValidator : AbstractValidator<RoleDurationDto>
{
    public RoleDurationDtoValidator()
    {
        RuleFor(x => x.Months)
            .InclusiveBetween(0, ValidationConstants.MaxRoleTimeMonths)
            .When(x => x.Months.HasValue);

        RuleFor(x => x.Years)
            .InclusiveBetween(0, ValidationConstants.MaxRoleTimeYears)
            .When(x => x.Years.HasValue);
    }
}

public class CareerMapRuleDtoValidator : AbstractValidator<CareerMapRuleDto>
{
    public CareerMapRuleDtoValidator()
    {
        RuleFor(x => x.CareerPathId)
            .GreaterThan(0).WithMessage("Career path is required.");

        RuleFor(x => x.Grade)
            .NotEmpty().WithMessage("Grade is required.")
            .InclusiveBetween(ValidationConstants.MinGrade, ValidationConstants.MaxGrade)
            .WithMessage($"Grade must be between {ValidationConstants.MinGrade} and {ValidationConstants.MaxGrade}.");

        RuleFor(x => x.PositionJobIds)
            .NotEmpty().WithMessage("At least one job must be assigned to the career map rule.");

        RuleFor(x => x.MinRoleTime).SetValidator(new RoleDurationDtoValidator());
        RuleFor(x => x.MinExperience).SetValidator(new RoleDurationDtoValidator());
    }
}

public class UpdateCareerAssignmentDtoValidator : AbstractValidator<UpdateCareerAssignmentDto>
{
    public UpdateCareerAssignmentDtoValidator()
    {
        RuleFor(x => x.CompanyName)
            .MaximumLength(ValidationConstants.MaxStandardLength).When(x => x.CompanyName != null);

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0).When(x => x.DepartmentId.HasValue)
            .WithMessage("Department id must be a positive integer.");

        RuleFor(x => x.TeamId)
            .GreaterThan(0).When(x => x.TeamId.HasValue)
            .WithMessage("Team id must be a positive integer.");

        RuleFor(x => x.CareerPathId)
            .GreaterThan(0).When(x => x.CareerPathId.HasValue)
            .WithMessage("Career path id must be a positive integer.");

        RuleFor(x => x.JobId)
            .GreaterThan(0).When(x => x.JobId.HasValue)
            .WithMessage("Job id must be a positive integer.");

        RuleFor(x => x.Grade)
            .InclusiveBetween(ValidationConstants.MinGrade, ValidationConstants.MaxGrade).When(x => x.Grade.HasValue)
            .WithMessage($"Grade must be between {ValidationConstants.MinGrade} and {ValidationConstants.MaxGrade}.");

        RuleFor(x => x.ManagerId)
            .GreaterThan(0).When(x => x.ManagerId.HasValue)
            .WithMessage("Manager id must be a positive integer.");

        RuleFor(x => x.NewPositionName)
            .MaximumLength(ValidationConstants.MaxPositionNameLength).When(x => x.NewPositionName != null);
    }
}

public class UpdateUserOrganizationPositionDtoValidator : AbstractValidator<UpdateUserOrganizationPositionDto>
{
    public UpdateUserOrganizationPositionDtoValidator()
    {
        RuleFor(x => x.OrganizationPositionId)
            .GreaterThan(0).When(x => x.OrganizationPositionId.HasValue)
            .WithMessage("Organization position id must be a positive integer.");
    }
}

public class UpdateOrganizationPositionDtoValidator : AbstractValidator<UpdateOrganizationPositionDto>
{
    public UpdateOrganizationPositionDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Position id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Position name is required.")
            .MaximumLength(ValidationConstants.MaxPositionNameLength);

        RuleFor(x => x.ParentId)
            .GreaterThan(0).When(x => x.ParentId.HasValue)
            .WithMessage("Parent id must be a positive integer.")
            .NotEqual(x => x.Id).When(x => x.ParentId.HasValue)
            .WithMessage("A position cannot be its own parent.");
    }
}
