namespace DZDDashboard.Data.Entities;

public class Period : IAuditableEntity
{
    public int Id { get; set; }
    public string? PeriodName { get; set; }
    public bool Active { get; set; }
    public List<TargetEffort>? TargetEfforts { get; set; }
    public List<Itsm>? Itsms { get; set; }
    public List<Project>? Projects { get; set; }
    public List<HeadLeadCoefficient>? HeadLeadCoefficients { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}