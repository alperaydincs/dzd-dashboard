namespace DZDDashboard.Common.DTOs;

public record UserProfileReportsToDto
{
    public int Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public int? AvatarColorIndex { get; init; }
    public int? OrganizationPositionId { get; init; }
    public DepartmentDto? Department { get; init; }
    public JobDto? Job { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public bool HasAvatar { get; init; }
    public int? AvatarId  { get; init; }
}
