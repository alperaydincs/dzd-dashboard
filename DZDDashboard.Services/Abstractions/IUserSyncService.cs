namespace DZDDashboard.Services;

/// <summary>Entra ID / Azure AD user synchronisation.</summary>
public interface IUserSyncService
{
    /// <summary>
    /// Ensures an internal user record exists for the given Entra object-id.
    /// Creates one on first login. Returns the internal database user-id.
    /// </summary>
    Task<int> SyncEntraUserAsync(string objectId, string? email, string? firstName, string? lastName,
        CancellationToken cancellationToken = default);
}
