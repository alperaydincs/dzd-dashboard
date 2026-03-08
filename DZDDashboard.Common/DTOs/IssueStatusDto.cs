namespace DZDDashboard.Common.DTOs
{
    public record IssueStatusDto
    {
        public int Id { get; init; }
        public string? StatusName { get; init; }
    }
}

