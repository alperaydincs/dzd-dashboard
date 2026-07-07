using DZDDashboard.Data.Abstractions;

namespace DZDDashboard.Data.Entities.History;

public class HealthInsuranceBenefitHistory : IHistoryEntity
{
    public long HistoryId { get; set; }
    public HistoryOperation Operation { get; set; }
    public DateTime HistoryRecordedAt { get; set; }
    public int? HistoryRecordedById { get; set; }

    public int Id { get; set; }
    public string BenefitType { get; set; } = string.Empty;
    public string? BenefitName { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? ProviderName { get; set; }
    public string? Notes { get; set; }
    public int UserId { get; set; }
}
