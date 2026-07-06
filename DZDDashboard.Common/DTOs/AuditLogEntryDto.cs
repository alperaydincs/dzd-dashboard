namespace DZDDashboard.Common.DTOs;

public record AuditLogEntryDto
{
    public int       Id             { get; set; }
    public string    Action         { get; set; } = string.Empty;
    public string    Detail         { get; set; } = string.Empty;
    public string?   PerformedByName { get; set; }
    public DateTime  CreatedAt      { get; set; }
}
