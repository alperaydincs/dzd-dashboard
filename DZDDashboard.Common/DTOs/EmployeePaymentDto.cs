namespace DZDDashboard.Common.DTOs;

public record EmployeePaymentDto
{
    public int EmployeeId { get; set; }
    public List<SalaryRecordDto> SalaryHistory { get; set; } = [];
    public List<BenefitRecordDto> Benefits { get; set; } = [];
    public List<AdditionalPaymentDto> AdditionalPayments { get; set; } = [];
    public List<DeductionDto> Deductions { get; set; } = [];
    public EmployeePaymentSummaryDto Summary { get; set; } = new();
}
