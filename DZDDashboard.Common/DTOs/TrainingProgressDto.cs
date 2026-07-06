namespace DZDDashboard.Common.DTOs;

public enum TrainingProgressStatus
{
    Upcoming = 0,
    InProgress = 1,
    Completed = 2
}

public record TrainingProgressItemDto
{
    public long     CourseId        { get; init; }
    public string   CourseTitle     { get; init; } = string.Empty;
    public string?  CourseCategory  { get; init; }
    public int      ProgressPercent { get; init; }  
    public TrainingProgressStatus Status { get; init; }
    public DateTime? CompletionDate  { get; init; }
    public DateTime? LastAccessedDate { get; init; }
    public bool      IsAssigned      { get; init; }
    public string?   AssignedBy      { get; init; }
}


public record TrainingProgressSummaryDto
{
    public int TotalCount      { get; init; }
    public int CompletedCount  { get; init; }
    public int InProgressCount { get; init; }
    public int UpcomingCount   { get; init; }
    public int CompletionPercent { get; init; }
    public bool IsLinked { get; init; }
    public DateTime? LastSyncedAt { get; init; }
    public List<TrainingProgressItemDto> Items { get; init; } = [];
}
