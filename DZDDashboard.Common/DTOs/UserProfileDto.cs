namespace DZDDashboard.Common.DTOs;

/// <summary>
/// Read model for <c>GET /api/users/my-profile</c> — the authenticated user's own record.
/// <para>
/// <c>PersonalEmail</c> and <c>PersonalPhoneNumber</c> are included here intentionally:
/// self-service reads of one's own personal contact are permitted without <c>SensitiveDataPolicy</c>.
/// Cross-user reads of personal contact require the sensitive-info endpoint.
/// </para>
/// </summary>
public record UserProfileDto
{
    public int Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string? PersonalEmail { get; init; }
    public string? PhoneNumber { get; init; }
    public string? PersonalPhoneNumber { get; init; }
    public DateTime? UserStartDate { get; init; }
    public DateTime? PositionStartDate { get; init; }
    public string? ContractType { get; init; }
    public DateTime? ContractEndDate { get; init; }
    public string? WorkModel { get; init; }
    public string? CompanyName { get; init; }
    public string? UnitName { get; init; }
    public string? ApprovalProcessUnit { get; init; }
    public int? Grade { get; init; }
    public UserProfileReportsToDto? ReportsTo { get; init; }
    public DepartmentDto? Department { get; init; }
    public TeamDto? Team { get; init; }
    public JobDto? Job { get; init; }
    public PayrollLocationDto? PayrollLocation { get; init; }
    public UserAvatarDto? Avatar { get; init; }
}
