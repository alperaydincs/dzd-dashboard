namespace DZDDashboard.Common.DTOs;

public record UserSummaryDto
{
    public int Id { get; init; }
    public string? Slug { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public int? AvatarColorIndex { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? City { get; init; }
    public string? Country { get; init; }
    public bool IsActive { get; init; }
    public DateTime? UserStartDate { get; init; }

    public UserAvatarSummaryDto? Avatar { get; init; }

    public int? DepartmentId { get; init; }
    public int? OrganizationPositionId { get; init; }
    public DepartmentDto? Department { get; init; }
    public JobDto? Job { get; init; }
    public TeamDto? Team { get; init; }
}

public record UserAvatarSummaryDto
{
    public int Id { get; init; }
    public string? ContentType { get; init; }

    /// <summary>Last time the avatar image changed; used as a cache-busting token in avatar URLs.</summary>
    public DateTime? UpdatedAt { get; init; }
}
