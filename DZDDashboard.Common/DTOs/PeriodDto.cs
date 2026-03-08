namespace DZDDashboard.Common.DTOs
{
    public record PeriodDto
    {
        public int Id { get; init; }
        public string? PeriodName { get; init; }
        public bool Active { get; init; }

    }
}

