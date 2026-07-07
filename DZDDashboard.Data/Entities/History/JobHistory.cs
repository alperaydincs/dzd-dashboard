using DZDDashboard.Data.Abstractions;

namespace DZDDashboard.Data.Entities.History;

public class JobHistory : IHistoryEntity
{
    public long HistoryId { get; set; }
    public HistoryOperation Operation { get; set; }
    public DateTime HistoryRecordedAt { get; set; }
    public int? HistoryRecordedById { get; set; }

    public int Id { get; set; }
    public string? Title { get; set; }
    public int? Level { get; set; }
}
