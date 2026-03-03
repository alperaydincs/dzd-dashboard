namespace DZDDashboard.Data.Entities;

public class Salesforce : IAuditableEntity
{
    public int Id { get; set; }
    public string? TaskTeam { get; set; }
    public string? TaskPo { get; set; }
    public string? IsSuitable { get; set; }
    public string? Info { get; set; }
    public List<Bid>? Bids { get; set; }
    public List<Project>? Projects { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
    public int? PayrollLocationId { get; set; }
    public PayrollLocation? PayrollLocation { get; set; }
}