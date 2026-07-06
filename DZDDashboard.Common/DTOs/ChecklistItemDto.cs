namespace DZDDashboard.Common.DTOs;

public record ChecklistItemDto
{
    public int    Id         { get; set; }
    public string Title      { get; set; } = string.Empty;
    public int    Sequence   { get; set; }
    public bool   IsRequired { get; set; }
    public string Status     { get; set; } = string.Empty;
    public DateTime? CompletedAt     { get; set; }
    public string?   CompletedByName { get; set; }
}
