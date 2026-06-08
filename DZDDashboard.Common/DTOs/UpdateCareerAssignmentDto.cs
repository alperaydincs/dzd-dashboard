namespace DZDDashboard.Common.DTOs;

// Validation is handled exclusively by UpdateCareerAssignmentDtoValidator (FluentValidation).
// Data annotations removed to avoid dual-validation with different error formats.
public record UpdateCareerAssignmentDto
{
    public string? CompanyName  { get; init; }
    public int?    DepartmentId { get; init; }
    public int?    TeamId       { get; init; }
    public int?    CareerPathId { get; init; }
    public int?    JobId        { get; init; }
    public int?    Grade        { get; init; }
}
