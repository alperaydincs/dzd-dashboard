namespace DZDDashboard.Common.DTOs;

public record LookupValueDto
{
    public int    Id       { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Value    { get; set; } = string.Empty;
    public int    Sequence { get; set; }
}
