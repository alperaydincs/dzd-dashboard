namespace DZDDashboard.Common.DTOs;

public record AdditionalPaymentDto
{
    public int Id { get; set; }
    public string? PaymentType { get; set; }
    public decimal Amount   { get; set; }
    public string  Currency { get; set; } = string.Empty;
    public string  Period   { get; set; } = string.Empty;
    public DateTime  StartDate { get; set; }
    public DateTime? EndDate   { get; set; }
    public string? Description { get; set; }
}
