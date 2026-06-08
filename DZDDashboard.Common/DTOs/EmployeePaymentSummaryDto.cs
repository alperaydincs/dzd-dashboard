namespace DZDDashboard.Common.DTOs;

/// <summary>
/// "Toplamlar" summary cards shown above the Payment tabs (section 8 of the analysis doc).
/// Amounts are grouped by currency rather than converted — no FX-conversion source exists yet
/// (open question BR-PAY-05); mixing currencies into one number would be misleading.
/// </summary>
public record EmployeePaymentSummaryDto
{
    /// <summary>Estimated monthly cost per currency: active salary (normalised to monthly) + active monthly benefits.</summary>
    public List<CurrencyAmountDto> EstimatedMonthlyCost { get; set; } = [];

    /// <summary>Net active-benefit total per currency (what the employee actually receives).</summary>
    public List<CurrencyAmountDto> ActiveBenefitsTotal { get; set; } = [];

    public int ActiveItemCount { get; set; }

    /// <summary>Number of items whose <c>EndDate</c> falls within the next 30 days.</summary>
    public int UpcomingExpirationCount { get; set; }
}

/// <summary>A monetary amount tagged with its currency — used to avoid summing across currencies.</summary>
public record CurrencyAmountDto(string Currency, decimal Amount);
