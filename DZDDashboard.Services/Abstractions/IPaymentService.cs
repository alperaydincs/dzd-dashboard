using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Services;

/// <summary>
/// Manages the Employee Profile → Payment screen: salary history, benefit line items
/// (incl. ÖSS dependents) and additional payments. Enforces the BR-PAY-* business rules
/// from the Payment analysis doc (overlap checks, dependent caps, OneTime date requirements).
/// </summary>
public interface IPaymentService
{
    /// <summary>Full admin/HR view: salary history + all benefits + all additional payments + summary cards.</summary>
    Task<EmployeePaymentDto> GetEmployeePaymentAsync(int userId, CancellationToken cancellationToken = default);

    /// <summary>Reduced self-service view: active salary + active BES lines only (decision: "Maaş ve BES görebilir ama düzenleyemez").</summary>
    Task<MyPaymentSummaryDto> GetMyPaymentSummaryAsync(int userId, CancellationToken cancellationToken = default);

    // ── Salary ───────────────────────────────────────────────────────────────
    Task<SalaryRecordDto> CreateSalaryRecordAsync(int userId, SalaryRecordDto dto, CancellationToken cancellationToken = default);
    Task                  UpdateSalaryRecordAsync(int userId, int recordId, SalaryRecordDto dto, CancellationToken cancellationToken = default);
    Task                  DeleteSalaryRecordAsync(int userId, int recordId, CancellationToken cancellationToken = default);

    // ── Benefits ─────────────────────────────────────────────────────────────
    Task<BenefitRecordDto> CreateBenefitRecordAsync(int userId, BenefitRecordDto dto, CancellationToken cancellationToken = default);
    Task                   UpdateBenefitRecordAsync(int userId, int recordId, BenefitRecordDto dto, CancellationToken cancellationToken = default);
    Task                   DeleteBenefitRecordAsync(int userId, int recordId, CancellationToken cancellationToken = default);

    // ── Additional Payments ──────────────────────────────────────────────────
    Task<AdditionalPaymentDto> CreateAdditionalPaymentAsync(int userId, AdditionalPaymentDto dto, CancellationToken cancellationToken = default);
    Task                       UpdateAdditionalPaymentAsync(int userId, int paymentId, AdditionalPaymentDto dto, CancellationToken cancellationToken = default);
    Task                       DeleteAdditionalPaymentAsync(int userId, int paymentId, CancellationToken cancellationToken = default);
}
