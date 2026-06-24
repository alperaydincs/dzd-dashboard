using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Services;

public interface IUserReadService
{
    Task<UserProfileDto?>             GetProfileByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<UserSummaryDto>> GetAllSummariesAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<EmployeeCardDto?>            GetEmployeeCardAsync(int id, CancellationToken cancellationToken = default);
    Task<EmployeeCardDto?>            GetEmployeeCardBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<List<UserSearchResultDto>>  SearchUsersAsync(string? query, int take, CancellationToken cancellationToken = default);
    Task<UserAvatarDto?>              GetAvatarByUserIdAsync(int id, CancellationToken cancellationToken = default);

    Task<EmployeeSensitiveInfoDto?>   GetSensitiveInfoAsync(int id, CancellationToken cancellationToken = default);
}
