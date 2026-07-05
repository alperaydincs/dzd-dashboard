namespace DZDDashboard.Common.DTOs;

/// <summary>Status of a single Udemy course for a learner.</summary>
public enum TrainingProgressStatus
{
    /// <summary>Assigned/enrolled but not started (0% progress).</summary>
    Upcoming = 0,
    InProgress = 1,
    Completed = 2
}

/// <summary>A single Udemy course row shown in the Trainings tab.</summary>
public record TrainingProgressItemDto
{
    public long     CourseId        { get; init; }
    public string   CourseTitle     { get; init; } = string.Empty;
    public string?  CourseCategory  { get; init; }
    public int      ProgressPercent { get; init; }   // 0..100, rounded
    public TrainingProgressStatus Status { get; init; }
    public DateTime? CompletionDate  { get; init; }
    public DateTime? LastAccessedDate { get; init; }
    public bool      IsAssigned      { get; init; }
    public string?   AssignedBy      { get; init; }
}

/// <summary>Aggregated training progress for one employee (feeds the whole tab).</summary>
public record TrainingProgressSummaryDto
{
    public int TotalCount      { get; init; }
    public int CompletedCount  { get; init; }
    public int InProgressCount { get; init; }
    public int UpcomingCount   { get; init; }

    /// <summary>Completed / Total as a 0..100 percentage (0 when no courses).</summary>
    public int CompletionPercent { get; init; }

    /// <summary>True when the employee could not be matched to a Udemy account.</summary>
    public bool IsLinked { get; init; }

    /// <summary>When the underlying data was last pulled from Udemy.</summary>
    public DateTime? LastSyncedAt { get; init; }

    public List<TrainingProgressItemDto> Items { get; init; } = [];
}
