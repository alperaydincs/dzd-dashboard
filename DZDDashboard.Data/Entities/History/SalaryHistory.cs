using DZDDashboard.Data.Abstractions;

namespace DZDDashboard.Data.Entities.History;

public class SalaryHistory : IHistoryEntity
{
    public long HistoryId { get; set; }
    public HistoryOperation Operation { get; set; }
    public DateTime HistoryRecordedAt { get; set; }
    public int? HistoryRecordedById { get; set; }

    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string PayType { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
    public string? PayrollCycle { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Notes { get; set; }
    public DateTime? NotesModifiedAt { get; set; }
    public int UserId { get; set; }
}
