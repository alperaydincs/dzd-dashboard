namespace DZDDashboard.Data.Entities;

public class Company : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<Department> Departments { get; set; } = new List<Department>();
}