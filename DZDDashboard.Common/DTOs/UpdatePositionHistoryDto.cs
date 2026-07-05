namespace DZDDashboard.Common.DTOs;

public record UpdatePositionHistoryDto
{
    public int?    CompanyId    { get; init; }
    public int?    DepartmentId { get; init; }
    public int?    TeamId       { get; init; }

    public DateTime  StartDate { get; init; }
    public DateTime? EndDate   { get; init; }
}
