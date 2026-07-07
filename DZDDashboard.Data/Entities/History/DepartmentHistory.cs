using DZDDashboard.Data.Abstractions;

namespace DZDDashboard.Data.Entities.History;

public class DepartmentHistory : IHistoryEntity
{
    public long HistoryId { get; set; }
    public HistoryOperation Operation { get; set; }
    public DateTime HistoryRecordedAt { get; set; }
    public int? HistoryRecordedById { get; set; }

    public int Id { get; set; }
    public string? Name { get; set; }
    public int? CompanyId { get; set; }
}
