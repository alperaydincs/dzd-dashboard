namespace DZDDashboard.Data.Entities;

public class Salesforce : AuditableEntity
{
    public int Id { get; set; }
    public string? TaskTeam { get; set; }
    public string? TaskPo { get; set; }
    public string? IsSuitable { get; set; }
    public string? Info { get; set; }
    public List<Bid>? Bids { get; set; }
    public List<Project>? Projects { get; set; }    public int? PayrollLocationId { get; set; }
    public PayrollLocation? PayrollLocation { get; set; }
}