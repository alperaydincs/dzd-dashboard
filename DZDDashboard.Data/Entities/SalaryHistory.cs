namespace DZDDashboard.Data.Entities;

/// <summary>
/// One salary validity period for a user ("tarihçeleme"): when pay changes, the active
/// record is closed with an <see cref="EndDate"/> and a new record opens with a new
/// <see cref="StartDate"/>. Backs the Payment screen's "Salary" tab.
/// BR-PAY-01: a user may not have two active (open-ended or overlapping) records at once —
/// enforced in <c>PaymentService</c>, not at the database level.
/// </summary>
public class SalaryHistory : AuditableEntity
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

    public int UserId { get; set; }
    public User? User { get; set; }
}
