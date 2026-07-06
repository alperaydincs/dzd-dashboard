namespace DZDDashboard.Common.DTOs;

public record OrganizationPositionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public OrgChartUserDto? User  { get; set; }
}

