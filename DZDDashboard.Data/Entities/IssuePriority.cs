namespace DZDDashboard.Data.Entities;

public class IssuePriority : IAuditableEntity
{
    public int Id { get; set; }
    public string? PriorityName { get; set; }
    public List<Itsm>? Itsms { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}
