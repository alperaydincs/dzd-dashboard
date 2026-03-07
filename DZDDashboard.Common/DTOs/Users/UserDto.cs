using DZDDashboard.Common.DTOs.Organization;

namespace DZDDashboard.Common.DTOs.Users;

public record UserDto
{
    public int Id { get; init; }
    public string? Username { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? City { get; init; }
    public string? Country { get; init; }
    public bool IsActive { get; init; }
    public DateTime? UserStartDate { get; init; }
    public UserAvatarDto? Avatar { get; init; }
    public IReadOnlyCollection<RoleDto> Roles { get; init; } = Array.Empty<RoleDto>();
    public DepartmentDto? Department { get; init; }
    public JobDto? Job { get; init; }
    public TeamDto? Team { get; init; }
    public int? OrganizationPositionId { get; init; }
    public int? ReportsToId { get; init; }
    public string? ReportsToName { get; init; }
}
