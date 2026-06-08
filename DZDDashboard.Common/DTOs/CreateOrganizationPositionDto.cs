namespace DZDDashboard.Common.DTOs;

public record CreateOrganizationPositionDto
{
    public string Name { get; set; } = string.Empty;
    public int? ParentId { get; set; }
}

