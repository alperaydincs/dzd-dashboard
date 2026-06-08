namespace DZDDashboard.Data.Entities;

public class JiraStatus : AuditableEntity
{
    public int Id { get; set; }
    public string? JiraStatusName { get; set; }
    public List<Project>? Projects { get; set; }
}
