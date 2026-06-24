using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Client.Services;

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

    Task<DeductionDto?> CreateDeductionAsync(int userId, DeductionDto dto);
    Task<HttpResponseMessage> UpdateDeductionAsync(int userId, int deductionId, DeductionDto dto);
    Task<HttpResponseMessage> DeleteDeductionAsync(int userId, int deductionId);
}
