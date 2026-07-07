using DZDDashboard.Data.Abstractions;

namespace DZDDashboard.Data.Entities.History;

public class BenefitPaymentDependentHistory : IHistoryEntity
{
    public long HistoryId { get; set; }
    public HistoryOperation Operation { get; set; }
    public DateTime HistoryRecordedAt { get; set; }
    public int? HistoryRecordedById { get; set; }

    public int Id { get; set; }
    public string? DependentName { get; set; }
    public string? RelationType { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int BenefitPaymentId { get; set; }
}
