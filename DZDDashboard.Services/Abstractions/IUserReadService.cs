using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Services;

/// <summary>Read-only user queries — profile, card, summaries, avatar.</summary>
public interface IUserReadService
{
    Task<UserProfileDto?>             GetProfileByIdAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>Paged lightweight user list without avatar base64 — use for list/grid views.</summary>
    Task<PagedResult<UserSummaryDto>> GetAllSummariesAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<EmployeeCardDto?>            GetEmployeeCardAsync(int id, CancellationToken cancellationToken = default);
    Task<UserAvatarDto?>              GetAvatarByUserIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns PII-sensitive employee fields. Requires <see cref="DZDDashboard.Common.Constants.Roles.SensitiveDataPolicy"/>.
    /// Separated from <see cref="GetEmployeeCardAsync"/> so future roles can be granted access here independently.
    /// </summary>
    Task<EmployeeSensitiveInfoDto?>   GetSensitiveInfoAsync(int id, CancellationToken cancellationToken = default);
}
