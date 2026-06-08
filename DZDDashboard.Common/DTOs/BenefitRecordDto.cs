namespace DZDDashboard.Common.DTOs;

/// <summary>
/// A single benefit (yan hak) line item — meal, transport, ÖSS (private health insurance),
/// BES (private pension), etc. Each item carries its own validity period so it can be
/// tracked/closed independently of other benefits ("kalem bazlı yönetim").
/// </summary>
public record BenefitRecordDto
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

    /// <summary>Provider/policy info (e.g. BES sağlayıcı adı).</summary>
    public string? ProviderName { get; set; }
    public string? Notes        { get; set; }

    /// <summary>ÖSS dependent rows (1..5). Empty for non-ÖSS benefit types.</summary>
    public List<BenefitDependentDto> Dependents { get; set; } = [];

    // ── Audit (read-only) ────────────────────────────────────────────────────
    public DateTime  CreatedAt      { get; set; }
    public DateTime? ModifiedAt     { get; set; }
    public string?   ModifiedByName { get; set; }
}
