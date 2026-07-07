using DZDDashboard.Data.Abstractions;

namespace DZDDashboard.Data.Entities.History;

public class EducationHistory : IHistoryEntity
{
    public long HistoryId { get; set; }
    public HistoryOperation Operation { get; set; }
    public DateTime HistoryRecordedAt { get; set; }
    public int? HistoryRecordedById { get; set; }

    public int Id { get; set; }
    public int UserId { get; set; }
    public string? EducationLevel { get; set; }
    public string? Institution { get; set; }
    public string? Program { get; set; }
    public DateTime? GraduationDate { get; set; }
    public string? Status { get; set; }
}
