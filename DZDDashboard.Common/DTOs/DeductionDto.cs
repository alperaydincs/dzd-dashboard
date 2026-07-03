namespace DZDDashboard.Common.DTOs;

public record DeductionDto
{
    public int Id { get; set; }

    public string? DeductionType { get; set; }

    public decimal Amount   { get; set; }
    public string  Currency { get; set; } = string.Empty;
    public string  Period   { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public string? Notes { get; set; }

    public DateTime  CreatedAt      { get; set; }
    public DateTime? ModifiedAt     { get; set; }
    public string?   ModifiedByName { get; set; }
}
