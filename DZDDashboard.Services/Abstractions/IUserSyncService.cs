namespace DZDDashboard.Services;

public interface IUserSyncService
{
    Task<int> SyncEntraUserAsync(string objectId, string? email, string? firstName, string? lastName,
        bool hasElevatedRole = false, CancellationToken cancellationToken = default);
}
