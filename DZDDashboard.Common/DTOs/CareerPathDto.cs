namespace DZDDashboard.Common.DTOs;

public record CareerPathDto
{
    public int    Id            { get; set; }
    public string Name          { get; set; } = string.Empty;
    public int    UserGroupId   { get; set; }
    public string? UserGroupName { get; set; }
    public List<CareerMapRuleDto> Rules { get; set; } = [];
}
