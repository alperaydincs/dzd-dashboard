namespace DZDDashboard.Data.Entities;

public class IssuePaymentType : IAuditableEntity
{
    public int Id { get; set; }
    public string? PaymentTypeName { get; set; }
    public decimal Coefficient { get; set; }
    public int? PeriodId { get; set; }
    public Period? Period { get; set; }
    public List<Itsm>? Itsms { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}

