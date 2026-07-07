namespace DZDDashboard.Data.Entities;

/// <summary>
/// Pure marker - implementing this is what makes an entity history-tracked
/// (see DZDDashboard.Data.History.HistoryEntryFactory). Independent of BaseEntity/CreatedAt.
/// </summary>
public interface EntityWithHistory;
