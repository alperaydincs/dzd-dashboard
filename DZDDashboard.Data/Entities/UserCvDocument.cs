namespace DZDDashboard.Data.Entities;

public class UserCvDocument : AuditableEntity
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

    public string  ReviewStatus { get; set; } = DZDDashboard.Common.Constants.DocumentReviewStatuses.Pending;
    public string? ReviewNote   { get; set; }
}
