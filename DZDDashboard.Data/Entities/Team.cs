namespace DZDDashboard.Data.Entities;

public class Team : AuditableEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Project> Projects { get; set; } = new List<Project>();
    public ICollection<Itsm> Itsms { get; set; } = new List<Itsm>();
}