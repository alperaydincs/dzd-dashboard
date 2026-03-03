namespace DZDDashboard.Data.Entities;

public class Resolution : IAuditableEntity
{
    public int Id { get; set; }
    public string? ResolutionName { get; set; }
    public List<Itsm>? Itsms { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}