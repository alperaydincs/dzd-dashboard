namespace DZDDashboard.Data.Entities;

public class TargetEffort : AuditableEntity
{
    public int Id { get; set; }
    public int? PeriodId { get; set; }
    public Period? Period { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public decimal Target { get; set; }
    public decimal CompletedTarget { get; set; }
    /// <summary>DB-computed: [Target] - [CompletedTarget]. Do not set manually.</summary>
    public decimal RemainingTarget { get; private set; }
    public decimal ProjectBonusAmount { get; set; }
    public decimal ItsmBonusAmount { get; set; }
    public decimal ManagerBonusEffort { get; set; }
    public decimal ManagerBonusAmount { get; set; }
    public decimal TotalBonusAmount { get; set; }
    public bool IsActive { get; set; }
}