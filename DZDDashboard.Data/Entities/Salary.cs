namespace DZDDashboard.Data.Entities;

public class Salary : EntityWithHistory
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public string PayType { get; set; } = "Net";

    public string Currency { get; set; } = string.Empty;
    public string Period   { get; set; } = string.Empty;

    public string? PayrollCycle { get; set; }

    public DateTime  StartDate { get; set; }
    public DateTime? EndDate   { get; set; }

    public string? Notes { get; set; }

    public DateTime? NotesModifiedAt { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
}
