namespace DZDDashboard.Common.DTOs;

public record PositionHistoryDto
{
    public int Id { get; set; }
    public string? JobTitle    { get; set; }
    public string? CompanyName { get; set; }
    public string? DepartmentName { get; set; }
    public string? TeamName     { get; set; }
    public int?    Grade        { get; set; }
    public DateTime  StartDate { get; set; }
    public DateTime? EndDate   { get; set; }
    public string? ChangeType { get; set; }
    public bool IsCurrent => EndDate == null;
}
