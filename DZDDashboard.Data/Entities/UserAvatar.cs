namespace DZDDashboard.Data.Entities;

public class UserAvatar : AuditableEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int StorageId { get; set; }
    public string? ContentType { get; set; }
    public User? User { get; set; }
    public StoredFile? Storage { get; set; }
}