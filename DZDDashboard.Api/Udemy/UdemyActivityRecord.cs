using System.Text.Json.Serialization;

namespace DZDDashboard.Api.Udemy;

/// <summary>
/// Raw row from GET organizations/{id}/analytics/user-course-activity/.
/// Property names map to the snake_case fields returned by Udemy.
/// </summary>
public class UdemyActivityRecord
{
    [JsonPropertyName("user_id")]            public long    UserId { get; set; }
    [JsonPropertyName("user_email")]         public string? UserEmail { get; set; }
    [JsonPropertyName("user_external_id")]   public string? UserExternalId { get; set; }

    [JsonPropertyName("course_id")]          public long    CourseId { get; set; }
    [JsonPropertyName("course_title")]       public string? CourseTitle { get; set; }
    [JsonPropertyName("course_category")]    public string? CourseCategory { get; set; }
    [JsonPropertyName("course_duration")]    public double? CourseDuration { get; set; }

    [JsonPropertyName("completion_ratio")]   public double  CompletionRatio { get; set; }

    [JsonPropertyName("course_enroll_date")]        public DateTime? EnrollDate { get; set; }
    [JsonPropertyName("course_start_date")]         public DateTime? StartDate { get; set; }
    [JsonPropertyName("course_completion_date")]    public DateTime? CompletionDate { get; set; }
    [JsonPropertyName("course_last_accessed_date")] public DateTime? LastAccessedDate { get; set; }

    [JsonPropertyName("is_assigned")]        public bool    IsAssigned { get; set; }
    [JsonPropertyName("assigned_by")]        public string? AssignedBy { get; set; }
    [JsonPropertyName("user_is_deactivated")] public bool   UserIsDeactivated { get; set; }
}

/// <summary>Paginated envelope returned by the Udemy reporting endpoints.</summary>
public class UdemyPagedResponse<T>
{
    [JsonPropertyName("next")]    public string? Next { get; set; }
    [JsonPropertyName("results")] public List<T> Results { get; set; } = [];
}
