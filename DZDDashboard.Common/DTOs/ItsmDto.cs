using DZDDashboard.Common.DTOs.Users;

namespace DZDDashboard.Common.DTOs
{
    public record ItsmDto
    {
        public int Id { get; init; }
        public string? IssueKey { get; init; }
        public int? AsigneeId { get; init; }
        public UserDto? Asignee { get; init; }
        public int? BankId { get; init; }
        public BankDto? Bank { get; init; }
        public int? IssuePriorityId { get; init; }
        public IssuePriorityDto? IssuePriority { get; init; }
        public int? IssueStatusId { get; init; }
        public IssueStatusDto? IssueStatus { get; init; }
        public int? IssueTypeId { get; init; }
        public IssueTypeDto? IssueType { get; init; }
        public int? ItsmPaymentTypeId { get; init; }
        public IssuePaymentTypeDto? ItsmPaymentType { get; init; }
        public int? PeriodId { get; init; }
        public PeriodDto? Period { get; init; }
        public int? ResolutionId { get; init; }
        public ResolutionDto? Resolution { get; init; }
        public int? TeamId { get; init; }
        public TeamDto? Team { get; init; }
        public bool Active { get; init; }
    }
}
