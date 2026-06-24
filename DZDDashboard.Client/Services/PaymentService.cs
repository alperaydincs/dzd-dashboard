using Microsoft.AspNetCore.Components;
using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Client.Services;

public class PaymentService : ApiServiceBase, IPaymentClientService
{
    public PaymentService(
        IHttpClientFactory httpClientFactory,
        NavigationManager navigationManager)
        : base(httpClientFactory, navigationManager) { }

    public async Task<MyPaymentSummaryDto?> GetMyPaymentSummaryAsync()
        => await GetAsync<MyPaymentSummaryDto>(ApiRoutes.Users.MyPaymentSummary);

    public async Task<EmployeePaymentDto?> GetEmployeePaymentAsync(int userId)
        => await GetAsync<EmployeePaymentDto>(ApiRoutes.Users.Payment(userId));


    public async Task<SalaryRecordDto?> CreateSalaryRecordAsync(int userId, SalaryRecordDto dto)
        => await PostAsync<SalaryRecordDto>(ApiRoutes.Users.PaymentSalary(userId), dto);

    public async Task<HttpResponseMessage> UpdateSalaryRecordAsync(int userId, int recordId, SalaryRecordDto dto)
        => await PutAsync(ApiRoutes.Users.PaymentSalaryRecord(userId, recordId), dto);

    public async Task<HttpResponseMessage> DeleteSalaryRecordAsync(int userId, int recordId)
        => await DeleteAsync(ApiRoutes.Users.PaymentSalaryRecord(userId, recordId));


    public async Task<BenefitRecordDto?> CreateBenefitRecordAsync(int userId, BenefitRecordDto dto)
        => await PostAsync<BenefitRecordDto>(ApiRoutes.Users.PaymentBenefits(userId), dto);

    public async Task<HttpResponseMessage> UpdateBenefitRecordAsync(int userId, int recordId, BenefitRecordDto dto)
        => await PutAsync(ApiRoutes.Users.PaymentBenefitRecord(userId, recordId), dto);

    public async Task<HttpResponseMessage> DeleteBenefitRecordAsync(int userId, int recordId)
        => await DeleteAsync(ApiRoutes.Users.PaymentBenefitRecord(userId, recordId));


    public async Task<AdditionalPaymentDto?> CreateAdditionalPaymentAsync(int userId, AdditionalPaymentDto dto)
        => await PostAsync<AdditionalPaymentDto>(ApiRoutes.Users.PaymentAdditional(userId), dto);

    public async Task<HttpResponseMessage> UpdateAdditionalPaymentAsync(int userId, int paymentId, AdditionalPaymentDto dto)
        => await PutAsync(ApiRoutes.Users.PaymentAdditionalRecord(userId, paymentId), dto);

    public async Task<HttpResponseMessage> DeleteAdditionalPaymentAsync(int userId, int paymentId)
        => await DeleteAsync(ApiRoutes.Users.PaymentAdditionalRecord(userId, paymentId));


    public async Task<DeductionDto?> CreateDeductionAsync(int userId, DeductionDto dto)
        => await PostAsync<DeductionDto>(ApiRoutes.Users.PaymentDeductions(userId), dto);

    public async Task<HttpResponseMessage> UpdateDeductionAsync(int userId, int deductionId, DeductionDto dto)
        => await PutAsync(ApiRoutes.Users.PaymentDeductionRecord(userId, deductionId), dto);

    public async Task<HttpResponseMessage> DeleteDeductionAsync(int userId, int deductionId)
        => await DeleteAsync(ApiRoutes.Users.PaymentDeductionRecord(userId, deductionId));
}
