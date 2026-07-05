namespace DZDDashboard.Data.Entities;

public class Department : AuditableEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? CompanyId { get; set; }
    public Company? Company { get; set; }
    public ICollection<Team> Teams { get; set; } = new List<Team>();
    public ICollection<User> Users { get; set; } = new List<User>();
}