using DZDDashboard.Data.Abstractions;

namespace DZDDashboard.Data.Entities.History;

public class EmergencyContactHistory : IHistoryEntity
{
    public long HistoryId { get; set; }
    public HistoryOperation Operation { get; set; }
    public DateTime HistoryRecordedAt { get; set; }
    public int? HistoryRecordedById { get; set; }

    public int Id { get; set; }
    public int UserId { get; set; }
    public string? FullName { get; set; }
    public string? Relationship { get; set; }
    public string? PhoneNumber { get; set; }
}
