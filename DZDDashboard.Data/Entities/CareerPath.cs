namespace DZDDashboard.Data.Entities;

public class CareerPath : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<CareerMapRule> Rules { get; set; } = new List<CareerMapRule>();
}