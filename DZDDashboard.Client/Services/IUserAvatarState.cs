namespace DZDDashboard.Client.Services;

public interface IUserAvatarState
{
    event Action? Changed;
    void NotifyChanged();
}

public sealed class UserAvatarState : IUserAvatarState
{
    public event Action? Changed;
    public void NotifyChanged() => Changed?.Invoke();
}
