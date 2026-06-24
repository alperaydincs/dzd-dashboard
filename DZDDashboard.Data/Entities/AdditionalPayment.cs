namespace DZDDashboard.Data.Entities;

public class AdditionalPayment : AuditableEntity
{
    public int Id { get; set; }

    public int? PaymentTypeId { get; set; }
    public AdditionalPaymentTypeEntity? PaymentTypeRef { get; set; }

    public decimal Amount   { get; set; }
    public string  Currency { get; set; } = string.Empty;
    public string  Period   { get; set; } = string.Empty;

    public DateTime? PaymentDate { get; set; }
    public DateTime? StartDate   { get; set; }
    public DateTime? EndDate     { get; set; }

    public string? Description { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
}
