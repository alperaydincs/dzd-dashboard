namespace DZDDashboard.Data.Entities;

public class Team : IAuditableEntity
{
    public int Id { get; set; }
    public string? TeamName { get; set; }
    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }
    public List<User>? Users { get; set; }
    public List<Project>? Projects { get; set; }
    public List<Itsm>? Itsms { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}