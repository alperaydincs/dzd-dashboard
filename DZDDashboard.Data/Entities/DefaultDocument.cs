namespace DZDDashboard.Data.Entities;

public class DefaultDocument : AuditableEntity
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string DocumentName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
}