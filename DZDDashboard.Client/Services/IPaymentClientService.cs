using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Client.Services;

/// <summary>
/// Client-side gateway for the Payment screen — salary history, benefits (incl. ÖSS
/// dependents) and additional payments. Mirrors <c>PaymentController</c> 1:1.
/// Edit endpoints are Admin/HR only; <see cref="GetMyPaymentSummaryAsync"/> is the
/// reduced self-service view (own salary + active BES benefits, read-only).
/// </summary>
public interface IPaymentClientService
{
    Task<MyPaymentSummaryDto?> GetMyPaymentSummaryAsync();
    Task<EmployeePaymentDto?> GetEmployeePaymentAsync(int userId);

    Task<SalaryRecordDto?> CreateSalaryRecordAsync(int userId, SalaryRecordDto dto);
    Task<HttpResponseMessage> UpdateSalaryRecordAsync(int userId, int recordId, SalaryRecordDto dto);
    Task<HttpResponseMessage> DeleteSalaryRecordAsync(int userId, int recordId);

    Task<BenefitRecordDto?> CreateBenefitRecordAsync(int userId, BenefitRecordDto dto);
    Task<HttpResponseMessage> UpdateBenefitRecordAsync(int userId, int recordId, BenefitRecordDto dto);
    Task<HttpResponseMessage> DeleteBenefitRecordAsync(int userId, int recordId);

    Task<AdditionalPaymentDto?> CreateAdditionalPaymentAsync(int userId, AdditionalPaymentDto dto);
    Task<HttpResponseMessage> UpdateAdditionalPaymentAsync(int userId, int paymentId, AdditionalPaymentDto dto);
    Task<HttpResponseMessage> DeleteAdditionalPaymentAsync(int userId, int paymentId);
}
