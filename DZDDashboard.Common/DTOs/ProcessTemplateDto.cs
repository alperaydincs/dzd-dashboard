namespace DZDDashboard.Common.DTOs;

public record ProcessTemplateDto
{
    public int    Id       { get; set; }
    public string Kind     { get; set; } = string.Empty;
    public string Name     { get; set; } = string.Empty;
    public int    Sequence { get; set; }
}
