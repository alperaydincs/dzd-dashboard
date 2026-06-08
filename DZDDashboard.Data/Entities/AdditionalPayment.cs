namespace DZDDashboard.Data.Entities;

/// <summary>
/// A one-time or recurring additional payment (prim/ikramiye/avans/mesai/bonus/diğer)
/// for a user. "OneTime" entries require <see cref="PaymentDate"/> (BR-PAY-06);
/// periodic entries use <see cref="StartDate"/>/<see cref="EndDate"/> instead.
/// </summary>
public class AdditionalPayment : AuditableEntity
{
    public int Id { get; set; }

    public string PaymentType { get; set; } = string.Empty;

    public decimal Amount   { get; set; }
    public string  Currency { get; set; } = string.Empty;
    public string  Period   { get; set; } = string.Empty;

    public DateTime? PaymentDate { get; set; }
    public DateTime? StartDate   { get; set; }
    public DateTime? EndDate     { get; set; }

    public bool? TaxableFlag { get; set; }

    public string? Description { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
}
