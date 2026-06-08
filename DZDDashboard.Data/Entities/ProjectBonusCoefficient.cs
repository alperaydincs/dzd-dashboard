namespace DZDDashboard.Data.Entities;

public class ProjectBonusCoefficient : AuditableEntity
{
    public int Id { get; set; }
    public decimal Coefficient { get; set; }
    public int? PeriodId { get; set; }
    public Period? Period { get; set; }
}