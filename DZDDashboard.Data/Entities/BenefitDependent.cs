namespace DZDDashboard.Data.Entities;

/// <summary>
/// One dependent line under a <see cref="BenefitRecord"/> (e.g. ÖSS spouse/child rows).
/// At most 5 per benefit record (BR-PAY-03); its validity period may not exceed the
/// parent benefit's range — both enforced in <c>PaymentService</c>.
/// </summary>
public class BenefitDependent : AuditableEntity
{
    public int Id { get; set; }

    /// <summary>1-based display order (1..5). Re-numbered by the service when a dependent is removed.</summary>
    public int Order { get; set; }

    public string DependentType { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime  StartDate { get; set; }
    public DateTime? EndDate   { get; set; }

    public int BenefitRecordId { get; set; }
    public BenefitRecord? BenefitRecord { get; set; }
}
