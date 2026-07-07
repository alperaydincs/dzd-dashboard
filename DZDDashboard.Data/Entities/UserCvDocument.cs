namespace DZDDashboard.Data.Entities;

public class UserCvDocument : EntityWithHistory
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public long SizeBytes { get; set; }
    public bool IsActive { get; set; }

    public int? FileId { get; set; }
    public StoredFile? File { get; set; }

    public DateTime UploadedAt { get; set; }
}
