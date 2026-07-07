namespace DZDDashboard.Data.Entities;

public class PayrollLocation : EntityWithHistory
{
    public int Id { get; set; }
    public string? Location { get; set; }
}