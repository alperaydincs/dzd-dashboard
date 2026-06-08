namespace DZDDashboard.Common.DTOs;

/// <summary>
/// Aggregate payload for the Employee Profile → Payment screen: salary history,
/// benefit line items and additional payments for a single employee, plus the
/// "Toplamlar" summary cards. Returned by <c>GET /api/users/{id}/payment</c>.
/// </summary>
public record EmployeePaymentDto
{
    public int EmployeeId { get; set; }

    /// <summary>Most recent first; the entry with a null <c>EndDate</c> (if any) is the active one.</summary>
    public List<SalaryRecordDto> SalaryHistory { get; set; } = [];

    public List<BenefitRecordDto> Benefits { get; set; } = [];

    public List<AdditionalPaymentDto> AdditionalPayments { get; set; } = [];

    public EmployeePaymentSummaryDto Summary { get; set; } = new();
}

/// <summary>
/// Reduced, view-only projection for employee self-service (decision: "Maaş ve BES görebilir
/// ama düzenleyemez, sadece kendine ait olanı görür"). Returned by
/// <c>GET /api/users/my-profile/payment-summary</c> — never exposes other employees' data
/// and intentionally omits edit-relevant fields (audit, source/reference, notes).
/// </summary>
public record MyPaymentSummaryDto
{
    public SalaryRecordDto? ActiveSalary { get; set; }

    /// <summary>Active BES (PrivatePension) benefit lines only.</summary>
    public List<BenefitRecordDto> PensionBenefits { get; set; } = [];
}
