namespace DZDDashboard.Common.DTOs;

/// <summary>
/// Lightweight user record for list views.
/// Does NOT include avatar base64 content (reduces payload ~33% per user).
/// Use <see cref="UserDto"/> only when full avatar data is required.
/// </summary>
public record UserSummaryDto
{
    public int Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? City { get; init; }
    public string? Country { get; init; }
    public bool IsActive { get; init; }
    public DateTime? UserStartDate { get; init; }

    /// <summary>Only contains ContentType (for placeholder color), NOT base64 data.</summary>
    public UserAvatarSummaryDto? Avatar { get; init; }

    public int? DepartmentId { get; init; }
    public int? OrganizationPositionId { get; init; }
    public DepartmentDto? Department { get; init; }
    public JobDto? Job { get; init; }
    public TeamDto? Team { get; init; }
}

/// <summary>Avatar metadata without the base64 payload — safe for list endpoints.</summary>
public record UserAvatarSummaryDto
{
    public int Id { get; init; }
    public string? ContentType { get; init; }
    // ContentBase64 intentionally omitted from summary
}
