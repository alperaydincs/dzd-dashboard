namespace DZDDashboard.Data.Entities;

public class Resolution : AuditableEntity
{
    public int Id { get; set; }
    public string? ResolutionName { get; set; }
    public List<Itsm>? Itsms { get; set; }
}