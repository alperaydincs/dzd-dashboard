namespace DZDDashboard.Common.DTOs
{
    public record UserTrainingDto
    {
        public int Id { get; init; }
        public DateTime? CompletionDate { get; init; }
        public string? Status { get; init; }
        public int? Evaluation { get; init; }
        public string? Description { get; init; }
    }
}

