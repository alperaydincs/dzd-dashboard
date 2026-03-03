namespace DZDDashboard.Data.Entities;

public class IssueType : IAuditableEntity
{
    public int Id { get; set; }
    public string? TypeName { get; set; }
    public List<Itsm>? Itsms { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}
