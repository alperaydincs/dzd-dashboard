namespace DZDDashboard.Data.Entities;

public class UserGroup : AuditableEntity
{
    public int Id { get; set; }
    public string? GroupName { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
}