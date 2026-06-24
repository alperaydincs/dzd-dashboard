namespace DZDDashboard.Common.DTOs;

public record UpdatePositionHistoryDto
{
    public string? CompanyName  { get; init; }
    public int?    DepartmentId { get; init; }
    public int?    TeamId       { get; init; }

    public DateTime  StartDate { get; init; }
    public DateTime? EndDate   { get; init; }
}
