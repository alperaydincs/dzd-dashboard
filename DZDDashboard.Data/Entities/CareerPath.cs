namespace DZDDashboard.Data.Entities;

public class CareerPath : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int UserGroupId { get; set; }
    public UserGroup? UserGroup { get; set; }
    public ICollection<CareerMapRule> Rules { get; set; } = new List<CareerMapRule>();
}