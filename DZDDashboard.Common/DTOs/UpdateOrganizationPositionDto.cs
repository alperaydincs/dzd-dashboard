namespace DZDDashboard.Common.DTOs;

public record UpdateOrganizationPositionDto
{
    public int    Id       { get; init; }
    public string Name     { get; init; } = string.Empty;
    public int?   ParentId { get; init; }
    public int?   UserId   { get; init; }
}
