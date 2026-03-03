namespace DZDDashboard.Data.Entities;

public class UserDocumentCategory : IAuditableEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<UserDocument> UserDocuments { get; set; } = new();
    public string? ContentType { get; set; }
    public bool IsActive { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}
