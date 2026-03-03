namespace DZDDashboard.Common.DTOs
{
    public record ExCompanyHistoryDto
    {
        public int Id { get; init; }
        public string? CompanyName { get; init; }
        public string? JobTitle { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }
    }
}
