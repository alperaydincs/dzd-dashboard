
namespace DZDDashboard.Common.DTOs
{
    public record TargetEffortDto
    {
        public int Id { get; init; }
        public PeriodDto? Period { get; init; }
        public int? UserId { get; init; }
        public UserDto? User { get; init; }
        public decimal Target { get; init; }
        public decimal CompletedTarget { get; init; }
        public decimal RemainingTarget { get; init; }
        public decimal ProjectBonusAmount { get; init; }
        public decimal ItsmBonusAmount { get; init; }
        public decimal ManagerBonusEffort { get; init; }
        public decimal ManagerBonusAmount { get; init; }
        public decimal TotalBonusAmount { get; init; }
    }
}

