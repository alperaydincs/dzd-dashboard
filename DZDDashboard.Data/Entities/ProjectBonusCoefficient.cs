namespace DZDDashboard.Data.Entities;

public class ProjectBonusCoefficient : IAuditableEntity
{
    public int Id { get; set; }
    public decimal Coefficient { get; set; }
    public int? PeriodId { get; set; }
    public Period? Period { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}