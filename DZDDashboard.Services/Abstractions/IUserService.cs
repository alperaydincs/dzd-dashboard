namespace DZDDashboard.Services;

/// <summary>
/// Unified user service — composes IUserReadService, IUserWriteService, and IUserSyncService.
/// Inject the focused sub-interface when a component only needs one concern.
/// </summary>
public interface IUserService : IUserReadService, IUserWriteService, IUserSyncService { }
