namespace DZDDashboard.Common.DTOs;

public record OrganizationPositionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public int? ParentId { get; set; }
    public List<OrganizationPositionDto> Children { get; set; } = new();
    public int UserCount { get; set; }
    public int?            UserId { get; set; }
    public OrgChartUserDto? User  { get; set; }
}

