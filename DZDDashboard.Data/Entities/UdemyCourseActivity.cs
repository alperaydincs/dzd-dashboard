namespace DZDDashboard.Data.Entities;

/// <summary>
/// One row per (Udemy user, Udemy course) pulled from the Udemy Business
/// Reporting API (analytics/user-course-activity). Linked to a local <see cref="User"/>
/// by e-mail when a match exists; unmatched rows are still stored so they can be
/// reconciled later.
/// </summary>
public class UdemyCourseActivity : EntityWithHistory
{
    public int Id { get; set; }

    // Local user link (resolved by e-mail; null when no local match was found).
    public int?  UserId { get; set; }
    public User? User   { get; set; }

    // Udemy identity of the learner (kept so unmatched rows can be reconciled).
    public long    UdemyUserId    { get; set; }
    public string  UserEmail      { get; set; } = string.Empty;
    public string? UserExternalId { get; set; }

    // Course.
    public long    CourseId       { get; set; }
    public string  CourseTitle    { get; set; } = string.Empty;
    public string? CourseCategory { get; set; }
    public double? CourseDurationMinutes { get; set; }

    // Progress.
    public double    CompletionRatio { get; set; }   // 0..100
    public DateTime? EnrollDate      { get; set; }
    public DateTime? StartDate       { get; set; }
    public DateTime? CompletionDate  { get; set; }
    public DateTime? LastAccessedDate { get; set; }

    public bool    IsAssigned { get; set; }
    public string? AssignedBy { get; set; }

    /// <summary>When this row was last refreshed from the Udemy API.</summary>
    public DateTime LastSyncedAt { get; set; }
}
