namespace DZDDashboard.Common.DTOs
{
    public record SalaryHistoryDto
    {
        public int Id { get; init; }
        public decimal Amount { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }
    }
}

