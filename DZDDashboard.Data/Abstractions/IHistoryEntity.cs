using DZDDashboard.Data.Entities.History;

namespace DZDDashboard.Data.Abstractions;

public interface IHistoryEntity
{
    long HistoryId { get; set; }
    HistoryOperation Operation { get; set; }
    DateTime HistoryRecordedAt { get; set; }
    int? HistoryRecordedById { get; set; }

    int Id { get; set; }
}
