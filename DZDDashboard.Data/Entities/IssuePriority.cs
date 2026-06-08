namespace DZDDashboard.Data.Entities;

public class IssuePriority : AuditableEntity
{
    public int Id { get; set; }
    public string? PriorityName { get; set; }
    public List<Itsm>? Itsms { get; set; }
}