namespace DZDDashboard.Common.DTOs;

public record CareerPathDto
{
    public int    Id            { get; set; }
    public string Name          { get; set; } = string.Empty;
    public List<CareerMapRuleDto> Rules { get; set; } = [];
}
