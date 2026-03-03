namespace DZDDashboard.Common.DTOs
{
    public record IssuePaymentTypeDto
    {
        public int Id { get; init; }
        public string? PaymentTypeName { get; init; }
        public decimal Coefficient { get; init; }
        public int? PeriodId { get; init; }
        public PeriodDto? Period { get; init; }
    }
}
