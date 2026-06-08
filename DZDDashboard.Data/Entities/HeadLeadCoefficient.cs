namespace DZDDashboard.Data.Entities;

public class HeadLeadCoefficient : AuditableEntity
{
    public int Id { get; set; }
    public int? PeriodId { get; set; }
    public Period? Period { get; set; }
    public int? JobId { get; set; }
    public Job? Job { get; set; }
    public decimal Coefficient { get; set; }
}