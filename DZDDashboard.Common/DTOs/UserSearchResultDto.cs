namespace DZDDashboard.Common.DTOs;

public record UserSearchResultDto
{
    public int     Id    { get; init; }
    public string? Slug  { get; init; }
    public string? FirstName { get; init; }
    public string? LastName  { get; init; }
    public string? Email { get; init; }
    public int?    AvatarColorIndex   { get; init; }
    public bool      HasAvatar        { get; init; }
    public DateTime? AvatarUpdatedAt  { get; init; }
    public int?    CompanyId              { get; init; }
    public int?    DepartmentId           { get; init; }
    public int?    TeamId                 { get; init; }
    public int?    OrganizationPositionId { get; init; }
}
