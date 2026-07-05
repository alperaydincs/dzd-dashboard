namespace DZDDashboard.Common.DTOs;

public record UserProfileDto
{
    public int Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public int? AvatarColorIndex { get; init; }
    public string? Email { get; init; }
    public string? PersonalEmail { get; init; }
    public string? PhoneNumber { get; init; }
    public string? PersonalPhoneNumber { get; init; }
    public DateTime? UserStartDate { get; init; }
    public DateTime? PositionStartDate { get; init; }
    public string? ContractType { get; init; }
    public DateTime? ContractEndDate { get; init; }
    public string? WorkModel { get; init; }
    public int? CompanyId { get; init; }
    public CompanyDto? Company { get; init; }
    public int? Grade { get; init; }
    public UserProfileReportsToDto? ReportsTo { get; init; }
    public DepartmentDto? Department { get; init; }
    public TeamDto? Team { get; init; }
    public JobDto? Job { get; init; }
    public PayrollLocationDto? PayrollLocation { get; init; }

    /// <summary>Whether the user has an uploaded avatar. The image itself is served via the
    /// <c>/avatars/me</c> proxy rather than embedded here, keeping this payload small.</summary>
    public bool HasAvatar { get; init; }

    /// <summary>Last time the avatar changed; used as a cache-busting token in the avatar URL.</summary>
    public DateTime? AvatarUpdatedAt { get; init; }
}
