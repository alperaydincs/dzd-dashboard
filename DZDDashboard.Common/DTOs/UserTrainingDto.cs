namespace DZDDashboard.Common.DTOs;

public record UserTrainingDto
{
    public int      Id             { get; init; }
    public int?     TrainingId     { get; init; }
    public string?  TrainingName   { get; init; }
    public bool     IsActive       { get; init; }
    public DateTime? CompletionDate { get; init; }
    public string?  Status         { get; init; }
    public int?     Evaluation     { get; init; }
    public string?  Description    { get; init; }
}
