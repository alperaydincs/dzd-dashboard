namespace DZDDashboard.Data.Entities;

/// <summary>
/// One benefit (yan hak) line item for a user — meal, transport, ÖSS (private health
/// insurance), BES (private pension), etc. Each item is tracked/closed independently
/// via its own <see cref="StartDate"/>/<see cref="EndDate"/> ("kalem bazlı yönetim").
/// </summary>
public class BenefitRecord : AuditableEntity
{
    public int Id { get; set; }

    public string BenefitType { get; set; } = string.Empty;
    public string Payer       { get; set; } = string.Empty;

    public decimal Amount   { get; set; }
    public string  Currency { get; set; } = string.Empty;
    public string  Period   { get; set; } = string.Empty;

    public DateTime  StartDate { get; set; }
    public DateTime? EndDate   { get; set; }

    /// <summary>"Manual" or "Onboarding" — provenance for traceability (BR-PAY-04).</summary>
    public string  Source      { get; set; } = string.Empty;
    public string? ReferenceId { get; set; }

    public string? ProviderName { get; set; }
    public string? Notes        { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    /// <summary>ÖSS dependent rows (max 5 — BR-PAY-03).</summary>
    public List<BenefitDependent> Dependents { get; set; } = [];
}
