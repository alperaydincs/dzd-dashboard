namespace DZDDashboard.Common.DTOs;

/// <summary>
/// A single salary validity period ("tarihçeleme"): when a salary changes, the previous
/// record is closed with an <see cref="EndDate"/> and a new record opens with a new
/// <see cref="StartDate"/>. Validation (required fields, value-set membership, overlap
/// rules) lives in <c>SalaryRecordDtoValidator</c> — no data-annotation attributes here
/// to avoid dual-validation drift (see EducationHistoryDto for the same convention).
/// </summary>
public record SalaryRecordDto
{
    public int Id { get; set; }

    public decimal NetAmount { get; set; }
    public decimal? GrossAmount { get; set; }

    public string Currency { get; set; } = string.Empty;
    public string Period   { get; set; } = string.Empty;

    /// <summary>Optional payroll cycle label (e.g. "1st–30th of month") — reporting/integration only.</summary>
    public string? PayrollCycle { get; set; }

    public DateTime  StartDate { get; set; }
    public DateTime? EndDate   { get; set; }

    public string? Notes { get; set; }

    // ── Audit (read-only, surfaced for the "Tarihçe" view) ──────────────────
    public DateTime  CreatedAt        { get; set; }
    public DateTime? ModifiedAt       { get; set; }
    public string?   ModifiedByName   { get; set; }
}
