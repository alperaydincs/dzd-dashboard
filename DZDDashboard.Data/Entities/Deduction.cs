namespace DZDDashboard.Data.Entities;

public class Deduction : AuditableEntity
{
    public int Id { get; set; }

    public int? DeductionTypeId { get; set; }
    public DeductionTypeEntity? DeductionTypeRef { get; set; }

    public decimal Amount   { get; set; }
    public string  Currency { get; set; } = string.Empty;
    public string  Period   { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public string? Notes { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
}
