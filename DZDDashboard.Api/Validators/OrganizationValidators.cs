using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Validation;
using FluentValidation;

namespace DZDDashboard.Api.Validators;

public class CompanyDtoValidator : AbstractValidator<CompanyDto>
{
    public CompanyDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(ValidationConstants.MaxStandardLength).WithMessage($"Company name cannot exceed {ValidationConstants.MaxStandardLength} characters.");
    }
}

public class DepartmentDtoValidator : AbstractValidator<DepartmentDto>
{
    public DepartmentDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Department name is required.")
            .MaximumLength(ValidationConstants.MaxEntityNameLength).WithMessage($"Department name cannot exceed {ValidationConstants.MaxEntityNameLength} characters.");
    }
}

public class TeamDtoValidator : AbstractValidator<TeamDto>
{
    public TeamDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Team name is required.")
            .MaximumLength(ValidationConstants.MaxEntityNameLength).WithMessage($"Team name cannot exceed {ValidationConstants.MaxEntityNameLength} characters.");
    }
}

public class JobDtoValidator : AbstractValidator<JobDto>
{
    public JobDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Job title is required.")
            .MaximumLength(ValidationConstants.MaxEntityNameLength).WithMessage($"Job title cannot exceed {ValidationConstants.MaxEntityNameLength} characters.");

        RuleFor(x => x.Level)
            .InclusiveBetween(0, ValidationConstants.MaxGrade).When(x => x.Level.HasValue)
            .WithMessage($"Level must be between 0 and {ValidationConstants.MaxGrade}.");
    }
}

public class WorkTypeDtoValidator : AbstractValidator<WorkTypeDto>
{
    public WorkTypeDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Work type name is required.")
            .MaximumLength(ValidationConstants.MaxNameLength).WithMessage($"Work type name cannot exceed {ValidationConstants.MaxNameLength} characters.");
    }
}

public class GradeDtoValidator : AbstractValidator<GradeDto>
{
    public GradeDtoValidator()
    {
        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Grade level is required.")
            .MaximumLength(ValidationConstants.MaxGradeLevelLength).WithMessage($"Grade level cannot exceed {ValidationConstants.MaxGradeLevelLength} characters.");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .MaximumLength(ValidationConstants.MaxCurrencyCodeLength).WithMessage($"Currency code cannot exceed {ValidationConstants.MaxCurrencyCodeLength} characters.");

        RuleFor(x => x.MinSalary)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum salary must be non-negative.");

        RuleFor(x => x.MaxSalary)
            .GreaterThanOrEqualTo(0).WithMessage("Maximum salary must be non-negative.")
            .GreaterThanOrEqualTo(x => x.MinSalary).WithMessage("Maximum salary must be greater than or equal to minimum salary.");
    }
}

public class PayrollLocationDtoValidator : AbstractValidator<PayrollLocationDto>
{
    public PayrollLocationDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Payroll location name is required.")
            .MaximumLength(ValidationConstants.MaxEntityNameLength).WithMessage($"Payroll location name cannot exceed {ValidationConstants.MaxEntityNameLength} characters.");
    }
}

public class UserGroupDtoValidator : AbstractValidator<UserGroupDto>
{
    public UserGroupDtoValidator()
    {
        RuleFor(x => x.GroupName)
            .NotEmpty().WithMessage("Group name is required.")
            .MaximumLength(ValidationConstants.MaxEntityNameLength).WithMessage($"Group name cannot exceed {ValidationConstants.MaxEntityNameLength} characters.");
    }
}

public class CareerPathDtoValidator : AbstractValidator<CareerPathDto>
{
    public CareerPathDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Career path name is required.")
            .MaximumLength(ValidationConstants.MaxStandardLength);

        RuleFor(x => x.UserGroupId)
            .GreaterThan(0).WithMessage("Employee group is required.");
    }
}

public class CreateOrganizationPositionDtoValidator : AbstractValidator<CreateOrganizationPositionDto>
{
    public CreateOrganizationPositionDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Position name is required.")
            .MaximumLength(ValidationConstants.MaxPositionNameLength);

        RuleFor(x => x.ParentId)
            .GreaterThan(0).When(x => x.ParentId.HasValue)
            .WithMessage("Parent position id must be a positive integer.");
    }
}
