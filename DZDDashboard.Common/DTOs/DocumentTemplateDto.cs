namespace DZDDashboard.Common.DTOs;

public record DocumentTemplateDto
{
    public int    Id                { get; set; }
    public int    ProcessTemplateId { get; set; }
    public string Name              { get; set; } = string.Empty;
    public int    Sequence          { get; set; }
    public bool   IsRequired        { get; set; } = true;
    public int    DeadlineDays      { get; set; }
}
