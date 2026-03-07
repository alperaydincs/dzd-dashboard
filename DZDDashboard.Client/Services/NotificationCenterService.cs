namespace DZDDashboard.Client.Services;

public sealed class NotificationCenterService
{
    private readonly List<AppNotification> _notifications =
    [
        new AppNotification(1, "Welcome", "Employees module is ready to use.", DateTime.UtcNow.AddMinutes(-15), false),
        new AppNotification(2, "Info", "You can review organization changes in Settings.", DateTime.UtcNow.AddHours(-1), false)
    ];

    public event Action? Changed;

    public IReadOnlyList<AppNotification> GetAll()
    {
        return _notifications
            .OrderByDescending(n => n.CreatedAtUtc)
            .ToList();
    }

    public int UnreadCount => _notifications.Count(n => !n.IsRead);

    public void Add(string title, string message)
    {
        var nextId = _notifications.Count == 0 ? 1 : _notifications.Max(n => n.Id) + 1;
        _notifications.Add(new AppNotification(nextId, title, message, DateTime.UtcNow, false));
        Changed?.Invoke();
    }

    public void MarkAsRead(int id)
    {
        var index = _notifications.FindIndex(n => n.Id == id);
        if (index < 0)
        {
            return;
        }

        var existing = _notifications[index];
        if (existing.IsRead)
        {
            return;
        }

        _notifications[index] = existing with { IsRead = true };
        Changed?.Invoke();
    }

    public void MarkAllAsRead()
    {
        var hasUnread = false;
        for (var i = 0; i < _notifications.Count; i++)
        {
            if (_notifications[i].IsRead)
            {
                continue;
            }

            hasUnread = true;
            _notifications[i] = _notifications[i] with { IsRead = true };
        }

        if (hasUnread)
        {
            Changed?.Invoke();
        }
    }
}

public sealed record AppNotification(int Id, string Title, string Message, DateTime CreatedAtUtc, bool IsRead);
