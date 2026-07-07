namespace DZDDashboard.Common.DTOs;

public record OrgChartUserDto
{
    public int     Id        { get; init; }
    public string? Slug      { get; init; }
    public string? FirstName { get; init; }
    public string? LastName  { get; init; }
    public int?    AvatarColorIndex { get; init; }
    public string? Email     { get; init; }
    public DepartmentDto? Department { get; init; }
    public bool HasAvatar { get; init; }
    public int? AvatarId  { get; init; }
}
