namespace DZDDashboard.Common.DTOs;

public record ChecklistItemTemplateDto
{
    public int    Id                { get; set; }
    public int    ProcessTemplateId { get; set; }
    public string Title             { get; set; } = string.Empty;
    public int    Sequence          { get; set; }
    public bool   IsRequired        { get; set; } = true;
}
