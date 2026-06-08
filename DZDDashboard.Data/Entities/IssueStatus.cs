namespace DZDDashboard.Data.Entities;

public class IssueStatus : AuditableEntity
{
    public int Id { get; set; }
    public string? StatusName { get; set; }
    public List<Itsm>? Itsms { get; set; }
}