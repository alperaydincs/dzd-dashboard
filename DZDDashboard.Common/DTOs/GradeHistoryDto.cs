namespace DZDDashboard.Common.DTOs
{
    public record GradeHistoryDto
    {
        public int Id { get; init; }
        public int Grade { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }
    }
}

