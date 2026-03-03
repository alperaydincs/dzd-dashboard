namespace DZDDashboard.Data.Entities;

public class ProjectInvoice : IAuditableEntity
{
    public int Id { get; set; }
    public Bank? Bank { get; set; }
    public int? BankId { get; set; }
    public string? ProjectName { get; set; }
    public DzdStatus? DzdStatus { get; set; }
    public int? DzdStatusId { get; set; }
    public Department? Department { get; set; }
    public int? DepartmentId { get; set; }
    public decimal TotalEffort { get; set; }
    public Period? Period { get; set; }
    public int? PeriodId { get; set; }
    public string? PurchaseInvoiceNumber { get; set; }
    public string? PurchaseOrder { get; set; }
    public string? EFaturaNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal UnitEffort { get; set; }
    public string? Notes { get; set; }
    public string? JiraProjectNo { get; set; }
    public string? JiraTaskNo { get; set; }
    public bool Active { get; set; }
    public bool PartialInvoice { get; set; }
    public decimal Vat { get; set; }
    public decimal VatIncludedAmount { get; set; }
    public PayrollLocation? PayrollLocation { get; set; }
    public int? PayrollLocationId { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}