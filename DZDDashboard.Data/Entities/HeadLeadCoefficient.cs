namespace DZDDashboard.Data.Entities;

public class HeadLeadCoefficient : IAuditableEntity
{
    public int Id { get; set; }
    public int? PeriodId { get; set; }
    public Period? Period { get; set; }
    public int? JobId { get; set; }
    public Job? Job { get; set; }
    public decimal Coefficient { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}
