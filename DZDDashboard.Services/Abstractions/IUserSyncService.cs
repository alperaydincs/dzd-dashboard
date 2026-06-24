namespace DZDDashboard.Services;

public interface IUserSyncService
{
    Task<int> SyncEntraUserAsync(string objectId, string? email, string? firstName, string? lastName,
        CancellationToken cancellationToken = default);
}
