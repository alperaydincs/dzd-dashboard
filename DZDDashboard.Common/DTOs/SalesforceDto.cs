
namespace DZDDashboard.Common.DTOs
{
    public record SalesforceDto
    {
        public int Id { get; init; }
        public string? TaskTeam { get; init; }
        public string? TaskPo { get; init; }
        public string? IsSuitable { get; init; }
        public string? Info { get; init; }
        public int? PayrollLocationId { get; init; }
        public PayrollLocationDto? PayrollLocation { get; init; }
    }
}

