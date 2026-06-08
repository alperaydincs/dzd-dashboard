namespace DZDDashboard.Common.DTOs;

// Validation handled exclusively by UpdateOrganizationPositionDtoValidator (FluentValidation).
// Data annotation removed to be consistent with the rest of the DTO layer.
public record UpdateOrganizationPositionDto
{
    public int    Id       { get; init; }
    public string Name     { get; init; } = string.Empty;
    public int?   ParentId { get; init; }
    public int?   UserId   { get; init; }
}
