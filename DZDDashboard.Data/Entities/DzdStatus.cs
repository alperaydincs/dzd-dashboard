namespace DZDDashboard.Data.Entities;

public class DzdStatus : AuditableEntity
{
    public int Id { get; set; }
    public string DzdStatusName { get; set; } = string.Empty;
    public List<Project>? Projects { get; set; }
}