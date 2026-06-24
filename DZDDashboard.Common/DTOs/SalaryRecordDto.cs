namespace DZDDashboard.Common.DTOs;

public record SalaryRecordDto
{
    public int Id { get; set; }

    public decimal NetAmount { get; set; }
    public decimal? GrossAmount { get; set; }

    public string PayType { get; set; } = Constants.PayTypes.Net;

    public string Currency { get; set; } = string.Empty;
    public string Period   { get; set; } = string.Empty;

    public string? PayrollCycle { get; set; }

    public DateTime  StartDate { get; set; }
    public DateTime? EndDate   { get; set; }

    public string? Notes { get; set; }

    public DateTime? NotesModifiedAt { get; set; }

    public DateTime  CreatedAt        { get; set; }
    public DateTime? ModifiedAt       { get; set; }
    public string?   ModifiedByName   { get; set; }
}
