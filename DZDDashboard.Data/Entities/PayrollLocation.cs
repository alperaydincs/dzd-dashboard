namespace DZDDashboard.Data.Entities;

public class PayrollLocation : AuditableEntity
{
    public int Id { get; set; }
    public string? Location { get; set; }
    public List<Salesforce>? IntertechTeams { get; set; }
}