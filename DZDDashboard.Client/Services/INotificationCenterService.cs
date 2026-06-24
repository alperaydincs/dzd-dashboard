namespace DZDDashboard.Client.Services;

public interface INotificationCenterService
{
    event Action? Changed;
    IReadOnlyList<AppNotification> GetAll();
    int UnreadCount { get; }
    void Add(string title, string message);
    void MarkAsRead(int id);
    void MarkAllAsRead();
}
