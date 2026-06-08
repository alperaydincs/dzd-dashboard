namespace DZDDashboard.Common.DTOs;

public record PeriodDto
{
    public int Id { get; init; }
    public string PeriodName { get; init; } = string.Empty;
    public bool Active { get; init; }
}
