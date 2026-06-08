namespace DZDDashboard.Data.Entities;

public class IssuePaymentType : AuditableEntity
{
    public int Id { get; set; }
    public string? PaymentTypeName { get; set; }
    public decimal Coefficient { get; set; }
    public int? PeriodId { get; set; }
    public Period? Period { get; set; }
    public List<Itsm>? Itsms { get; set; }
}