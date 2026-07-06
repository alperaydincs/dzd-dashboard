namespace DZDDashboard.Common.DTOs;

public record EmployeePaymentSummaryDto
{
    public List<CurrencyAmountDto> ActiveBenefitsTotal { get; set; } = [];

    public List<CurrencyAmountDto> ActiveDeductionsTotal { get; set; } = [];

    public List<CurrencyAmountDto> ActiveAdditionalPaymentsTotal { get; set; } = [];
}

public record CurrencyAmountDto(string Currency, decimal Amount);
