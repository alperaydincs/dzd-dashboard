namespace DZDDashboard.Common.DTOs
{
    public record ProjectInvoiceDto
    {
        public int Id { get; init; }
        public int? BankId { get; init; }
        public BankDto? Bank { get; init; }
        public string? ProjectName { get; init; }
        public int? DzdStatusId { get; init; }
        public DzdStatusDto? DzdStatus { get; init; }
        public int? DepartmentId { get; init; }
        public DepartmentDto? Department { get; init; }
        public decimal TotalEffort { get; init; }
        public int? PeriodId { get; init; }
        public PeriodDto? Period { get; init; }
        public string? PurchaseInvoiceNumber { get; init; }
        public string? PurchaseOrder { get; init; }
        public string? EFaturaNumber { get; init; }
        public decimal TotalAmount { get; init; }
        public decimal UnitEffort { get; init; }
        public string? Notes { get; init; }
        public string? JiraProjectNo { get; init; }
        public string? JiraTaskNo { get; init; }
        public bool Active { get; init; }
        public bool PartialInvoice { get; init; }
        public decimal Vat { get; init; }
        public decimal VatIncludedAmount { get; init; }
        public int? PayrollLocationId { get; init; }
        public PayrollLocationDto? PayrollLocation { get; init; }
    }
}
