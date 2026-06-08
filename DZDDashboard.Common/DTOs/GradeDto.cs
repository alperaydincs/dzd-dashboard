namespace DZDDashboard.Common.DTOs;

// Validation is handled exclusively by GradeDtoValidator (FluentValidation).
// Data annotations removed to avoid dual-validation with different error formats.
public record GradeDto
{
    public int Id { get; set; }

    public string Level { get; set; } = string.Empty;

    public decimal MinSalary { get; set; }

    public decimal MaxSalary { get; set; }

    public string Currency { get; set; } = "TRY";

    /// <summary>FK to the next grade in the progression chain. Populated from the entity column — no Join needed.</summary>
    public int? NextStepId { get; set; }
}
