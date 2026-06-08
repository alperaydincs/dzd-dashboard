namespace DZDDashboard.Common.DTOs;

/// <summary>
/// One dependent line under a <see cref="BenefitRecordDto"/> (e.g. ÖSS spouse/child rows).
/// At most 5 dependents per benefit record — enforced by <c>BenefitRecordDtoValidator</c> (BR-PAY-03).
/// </summary>
public record BenefitDependentDto
{
    public int Id { get; set; }

    /// <summary>1-based display order (1..5). Re-numbered by the service when a dependent is removed.</summary>
    public int Order { get; set; }

    public string DependentType { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime  StartDate { get; set; }
    public DateTime? EndDate   { get; set; }
}
