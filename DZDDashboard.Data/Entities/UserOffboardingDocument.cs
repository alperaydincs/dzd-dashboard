namespace DZDDashboard.Data.Entities;

public class UserOffboardingDocument : AuditableEntity
{
    public int Id { get; set; }
    public int ChecklistItemId { get; set; }
    public ChecklistItem? ChecklistItem { get; set; }

    public string? FileName    { get; set; }
    public string? ContentType { get; set; }

    public int? FileId { get; set; }
    public StoredFile? File { get; set; }
}
