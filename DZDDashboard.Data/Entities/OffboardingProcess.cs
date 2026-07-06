using DZDDashboard.Common.Constants;

namespace DZDDashboard.Data.Entities;

public class OffboardingProcess : AuditableEntity
{
    public int Id { get; set; }
    public int   UserId { get; set; }
    public User? User   { get; set; }
    public int    TemplateId   { get; set; }
    public string TemplateName { get; set; } = string.Empty;
    public DateTime ExitDate { get; set; }
    public string    Status      { get; set; } = ProcessStatuses.InProgress;
    public DateTime? CompletedAt { get; set; }
    public List<ChecklistItem> Items { get; set; } = [];
    public List<ProcessDocument> Documents { get; set; } = [];
    public List<LifecycleAuditLogEntry> AuditLog { get; set; } = [];
}
