namespace DZDDashboard.Common.DTOs
{
    public record ProjectBonusCoefficientDto
    {
        public int Id { get; init; }
        public decimal Coefficient { get; init; }
        public int? PeriodId { get; init; }
        public PeriodDto? Period { get; init; }
    }
}
