namespace DZDDashboard.Client.Services;

/// <summary>
/// Thread-safe in-memory notification store for a single Blazor Server circuit.
/// Uses a lock because Blazor Server circuits can receive concurrent updates
/// from SignalR hub callbacks and UI event handlers on the same instance.
/// <para>
/// <b>Threading contract for subscribers:</b> <see cref="Changed"/> may be raised from a
/// non-UI thread (e.g. a SignalR hub callback). Blazor components subscribing to this event
/// must dispatch <c>StateHasChanged</c> via <c>InvokeAsync</c> to stay on the renderer
/// synchronisation context:
/// <code>
///   _notifications.Changed += async () =&gt; await InvokeAsync(StateHasChanged);
/// </code>
/// </para>
/// </summary>
public sealed class NotificationCenterService : INotificationCenterService
{
    private readonly List<AppNotification> _notifications = [];
    private readonly object _lock = new();
    private int _nextId; // Interlocked.Increment returns post-incremented value

    public event Action? Changed;

    public IReadOnlyList<AppNotification> GetAll()
    {
        lock (_lock)
            return _notifications.OrderByDescending(n => n.CreatedAtUtc).ToList();
    }

    public int UnreadCount
    {
        get { lock (_lock) return _notifications.Count(n => !n.IsRead); }
    }

    public void Add(string title, string message)
    {
        var id = Interlocked.Increment(ref _nextId);
        lock (_lock)
            _notifications.Add(new AppNotification(id, title, message, DateTime.UtcNow, false));
        Changed?.Invoke();
    }

    public void MarkAsRead(int id)
    {
        lock (_lock)
        {
            var index = _notifications.FindIndex(n => n.Id == id);
            if (index < 0 || _notifications[index].IsRead) return;
            _notifications[index] = _notifications[index] with { IsRead = true };
        }
        Changed?.Invoke();
    }

    public void MarkAllAsRead()
    {
        var hasUnread = false;
        lock (_lock)
        {
            for (var i = 0; i < _notifications.Count; i++)
            {
                if (_notifications[i].IsRead) continue;
                hasUnread = true;
                _notifications[i] = _notifications[i] with { IsRead = true };
            }
        }
        if (hasUnread) Changed?.Invoke();
    }
}

public sealed record AppNotification(int Id, string Title, string Message, DateTime CreatedAtUtc, bool IsRead);
