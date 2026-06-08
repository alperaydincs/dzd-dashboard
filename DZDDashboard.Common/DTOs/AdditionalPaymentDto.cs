namespace DZDDashboard.Common.DTOs;

/// <summary>
/// A one-time or recurring additional payment (prim/ikramiye/avans/mesai/bonus/diğer).
/// "OneTime" entries require <see cref="PaymentDate"/> (BR-PAY-06); periodic entries use
/// <see cref="StartDate"/>/<see cref="EndDate"/> to bound the applicable range.
/// </summary>
public record AdditionalPaymentDto
{
    public int Id { get; set; }

    public string PaymentType { get; set; } = string.Empty;

    public decimal Amount   { get; set; }
    public string  Currency { get; set; } = string.Empty;
    public string  Period   { get; set; } = string.Empty;

    /// <summary>Required when <see cref="Period"/> is "OneTime" — the date the money was actually paid.</summary>
    public DateTime? PaymentDate { get; set; }

    /// <summary>Required when <see cref="Period"/> is periodic — the range the recurring payment applies to.</summary>
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate   { get; set; }

    public bool? TaxableFlag { get; set; }

    public string? Description { get; set; }

    // ── Audit (read-only) ────────────────────────────────────────────────────
    public DateTime  CreatedAt      { get; set; }
    public DateTime? ModifiedAt     { get; set; }
    public string?   ModifiedByName { get; set; }
}
