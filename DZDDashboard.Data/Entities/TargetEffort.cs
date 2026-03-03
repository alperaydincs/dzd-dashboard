namespace DZDDashboard.Data.Entities;

public class TargetEffort : IAuditableEntity
{
    public int Id { get; set; }
    public int? PeriodId { get; set; }
    public Period? Period { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public decimal Target { get; set; }
    public decimal CompletedTarget { get; set; }
    public decimal RemainingTarget { get; set; }
    public decimal ProjectBonusAmount { get; set; }
    public decimal ItsmBonusAmount { get; set; }
    public decimal ManagerBonusEffort { get; set; }
    public decimal ManagerBonusAmount { get; set; }
    public decimal TotalBonusAmount { get; set; }
    public bool IsActive { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}
