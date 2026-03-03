namespace DZDDashboard.Data.Entities;

public class Department : IAuditableEntity
{
    public int Id { get; set; }
    public string? DepartmentName { get; set; }
    public int? CompanyId { get; set; }
    public Company? Company { get; set; }
    public List<Team>? Teams { get; set; }
    public List<Project>? Project { get; set; }
    public List<User>? Users { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public User? ModifiedBy { get; set; }
    public int? ModifiedById { get; set; }
}
