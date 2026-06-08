namespace DZDDashboard.Common.DTOs;

// Validation is handled exclusively by UpdateFamilyInfoDtoValidator (FluentValidation).
// Data annotations removed to avoid dual-validation with different error formats.
public record UpdateFamilyInfoDto
{
    public string?         MaritalStatus  { get; init; }
    public string?         SpouseFullName { get; init; }
    public List<ChildInfoDto> Children   { get; init; } = [];
}
