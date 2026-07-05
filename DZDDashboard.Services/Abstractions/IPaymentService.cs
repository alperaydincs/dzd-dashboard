using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Services;

public interface IPaymentService
{
    Task<EmployeePaymentDto> GetEmployeePaymentAsync(int userId, CancellationToken cancellationToken = default);

    Task<SalaryRecordDto> CreateSalaryRecordAsync(int userId, SalaryRecordDto dto, CancellationToken cancellationToken = default);
    Task                  UpdateSalaryRecordAsync(int userId, int recordId, SalaryRecordDto dto, CancellationToken cancellationToken = default);
    Task                  DeleteSalaryRecordAsync(int userId, int recordId, CancellationToken cancellationToken = default);

    Task<BenefitRecordDto> CreateBenefitRecordAsync(int userId, BenefitRecordDto dto, CancellationToken cancellationToken = default);
    Task                   UpdateBenefitRecordAsync(int userId, int recordId, BenefitRecordDto dto, CancellationToken cancellationToken = default);
    Task                   DeleteBenefitRecordAsync(int userId, int recordId, CancellationToken cancellationToken = default);

    Task<AdditionalPaymentDto> CreateAdditionalPaymentAsync(int userId, AdditionalPaymentDto dto, CancellationToken cancellationToken = default);
    Task                       UpdateAdditionalPaymentAsync(int userId, int paymentId, AdditionalPaymentDto dto, CancellationToken cancellationToken = default);
    Task                       DeleteAdditionalPaymentAsync(int userId, int paymentId, CancellationToken cancellationToken = default);

    Task<DeductionDto> CreateDeductionAsync(int userId, DeductionDto dto, CancellationToken cancellationToken = default);
    Task               UpdateDeductionAsync(int userId, int deductionId, DeductionDto dto, CancellationToken cancellationToken = default);
    Task               DeleteDeductionAsync(int userId, int deductionId, CancellationToken cancellationToken = default);
}
