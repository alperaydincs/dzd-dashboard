namespace DZDDashboard.Data.Entities;

public class IssueStatus : IAuditableEntity
{
    public int Id { get; set; }
    public string? StatusName { get; set; }
    public List<Itsm>? Itsms { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}
