using DZDDashboard.Data.Abstractions;

namespace DZDDashboard.Data.Entities.History;

public class PositionHistory : IHistoryEntity
{
    public long HistoryId { get; set; }
    public HistoryOperation Operation { get; set; }
    public DateTime HistoryRecordedAt { get; set; }
    public int? HistoryRecordedById { get; set; }

    public int Id { get; set; }
    public int UserId { get; set; }
    public string? JobTitle { get; set; }
    public string? CompanyName { get; set; }
    public string? DepartmentName { get; set; }
    public string? TeamName { get; set; }
    public int? Grade { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? ChangeType { get; set; }
}
