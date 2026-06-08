namespace DZDDashboard.Data.Entities;

public class IssueType : AuditableEntity
{
    public int Id { get; set; }
    public string? TypeName { get; set; }
    public List<Itsm>? Itsms { get; set; }
}