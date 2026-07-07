using DZDDashboard.Data.Abstractions;

namespace DZDDashboard.Data.Entities.History;

public class TeamHistory : IHistoryEntity
{
    public long HistoryId { get; set; }
    public HistoryOperation Operation { get; set; }
    public DateTime HistoryRecordedAt { get; set; }
    public int? HistoryRecordedById { get; set; }

    public int Id { get; set; }
    public string? Name { get; set; }
    public int? DepartmentId { get; set; }
}
