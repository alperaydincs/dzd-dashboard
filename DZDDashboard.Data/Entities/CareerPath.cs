namespace DZDDashboard.Data.Entities;

public class CareerPath : IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int UserGroupId { get; set; }
    public UserGroup? UserGroup { get; set; }
    public ICollection<CareerMapRule> Rules { get; set; } = new List<CareerMapRule>();
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}
