namespace DZDDashboard.Data.Entities;

public class Role : IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}