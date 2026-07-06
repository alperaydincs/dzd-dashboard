namespace DZDDashboard.Common.DTOs;

public record DeductionDto
{
    public int Id { get; set; }
    public string? DeductionType { get; set; }
    public decimal Amount   { get; set; }
    public string  Currency { get; set; } = string.Empty;
    public string  Period   { get; set; } = string.Empty;
    public DateTime  StartDate { get; set; }
    public DateTime? EndDate   { get; set; }
    public string? Notes { get; set; }
}
