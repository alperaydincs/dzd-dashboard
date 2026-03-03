namespace DZDDashboard.Data.Entities;

public class PayrollLocation : IAuditableEntity
{
    public int Id { get; set; }
    public string? Location { get; set; }
    public List<Salesforce>? IntertechTeams { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}
