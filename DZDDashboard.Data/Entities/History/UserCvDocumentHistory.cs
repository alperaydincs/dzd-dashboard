using DZDDashboard.Data.Abstractions;

namespace DZDDashboard.Data.Entities.History;

public class UserCvDocumentHistory : IHistoryEntity
{
    public long HistoryId { get; set; }
    public HistoryOperation Operation { get; set; }
    public DateTime HistoryRecordedAt { get; set; }
    public int? HistoryRecordedById { get; set; }

    public int Id { get; set; }
    public int UserId { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public long SizeBytes { get; set; }
    public bool IsActive { get; set; }
    public int? FileId { get; set; }
    public DateTime UploadedAt { get; set; }
}
