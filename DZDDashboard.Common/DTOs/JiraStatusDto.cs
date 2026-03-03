namespace DZDDashboard.Common.DTOs
{
    public record JiraStatusDto
    {
        public int Id { get; init; }
        public string? JiraStatusName { get; init; }
    }
}
