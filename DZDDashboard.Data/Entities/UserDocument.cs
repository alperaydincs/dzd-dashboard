namespace DZDDashboard.Data.Entities;

public class UserDocument : AuditableEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int? DocumentCategoryId { get; set; }
    public UserDocumentCategory? DocumentCategory { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public bool IsActive { get; set; }
}
