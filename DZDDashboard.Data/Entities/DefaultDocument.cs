namespace DZDDashboard.Data.Entities;

public class DefaultDocument : IAuditableEntity
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string DocumentName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}
