namespace DZDDashboard.Data.Entities;

public class Period : AuditableEntity
{
    public int Id { get; set; }
    public string PeriodName { get; set; } = string.Empty;
    public bool Active { get; set; }
    public List<TargetEffort>? TargetEfforts { get; set; }
    public List<Itsm>? Itsms { get; set; }
    public List<Project>? Projects { get; set; }
    public List<HeadLeadCoefficient>? HeadLeadCoefficients { get; set; }
}