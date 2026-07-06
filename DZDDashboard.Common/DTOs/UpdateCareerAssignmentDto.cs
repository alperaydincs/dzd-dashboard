namespace DZDDashboard.Common.DTOs;

public record UpdateCareerAssignmentDto
{
    public int?    CompanyId    { get; init; }
    public int?    DepartmentId { get; init; }
    public int?    TeamId       { get; init; }
    public int?    CareerPathId { get; init; }
    public int?    JobId        { get; init; }
    public int?    Grade        { get; init; }
    public int?    ManagerId       { get; init; }
    public string? NewPositionName { get; init; }
}
