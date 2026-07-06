namespace DZDDashboard.Data.Entities;

public class PositionHistory : AuditableEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public string? JobTitle { get; set; }
    public string? CompanyName { get; set; }
    public string? DepartmentName { get; set; }
    public string? TeamName { get; set; }
    public int? Grade { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? ChangeType { get; set; }
}
