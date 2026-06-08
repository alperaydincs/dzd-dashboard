namespace DZDDashboard.Client.Services;

/// <summary>Per-circuit notification feed for in-app toast/bell messages.</summary>
public interface INotificationCenterService
{
    event Action? Changed;
    IReadOnlyList<AppNotification> GetAll();
    int UnreadCount { get; }
    void Add(string title, string message);
    void MarkAsRead(int id);
    void MarkAllAsRead();
}
