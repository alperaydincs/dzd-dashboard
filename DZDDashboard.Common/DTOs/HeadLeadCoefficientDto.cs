namespace DZDDashboard.Common.DTOs
{
    public record HeadLeadCoefficientDto
    {
        public int Id { get; init; }
        public int? PeriodId { get; init; }
        public PeriodDto? Period { get; init; }
        public int? JobId { get; init; }
        public JobDto? Job { get; init; }
        public decimal Coefficient { get; init; }
    }
}
