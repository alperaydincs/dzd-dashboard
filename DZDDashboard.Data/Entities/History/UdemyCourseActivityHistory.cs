using DZDDashboard.Data.Abstractions;

namespace DZDDashboard.Data.Entities.History;

public class UdemyCourseActivityHistory : IHistoryEntity
{
    public long HistoryId { get; set; }
    public HistoryOperation Operation { get; set; }
    public DateTime HistoryRecordedAt { get; set; }
    public int? HistoryRecordedById { get; set; }

    public int Id { get; set; }
    public int? UserId { get; set; }
    public long UdemyUserId { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public string? UserExternalId { get; set; }
    public long CourseId { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public string? CourseCategory { get; set; }
    public double? CourseDurationMinutes { get; set; }
    public double CompletionRatio { get; set; }
    public DateTime? EnrollDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime? LastAccessedDate { get; set; }
    public bool IsAssigned { get; set; }
    public string? AssignedBy { get; set; }
    public DateTime LastSyncedAt { get; set; }
}
