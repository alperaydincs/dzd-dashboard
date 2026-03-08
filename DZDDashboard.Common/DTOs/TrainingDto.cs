namespace DZDDashboard.Common.DTOs
{
    public record TrainingDto
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public string? InstructorName { get; init; }
        public string? InstructorCompanyDetails { get; init; }
        public string? Details { get; init; }
        public string? Location { get; init; }
        public int? RepeatFrequency { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public bool IsActive { get; init; }
    }
}

