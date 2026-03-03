namespace DZDDashboard.Data.Entities;

public class Itsm : IAuditableEntity
{
    public int Id { get; set; }
    public IssueType? IssueType { get; set; }
    public int? IssueTypeId { get; set; }
    public Bank? Bank { get; set; }
    public int? BankId { get; set; }
    public string? IssueKey { get; set; }
    public int? AsigneeId { get; set; }
    public User? Asignee { get; set; }
    public Team? Team { get; set; }
    public int? TeamId { get; set; }
    public Resolution? Resolution { get; set; }
    public int? ResolutionId { get; set; }
    public IssuePriority? IssuePriority { get; set; }
    public int? IssuePriorityId { get; set; }
    public IssueStatus? IssueStatus { get; set; }
    public int? IssueStatusId { get; set; }
    public IssuePaymentType? ItsmPaymentType { get; set; }
    public int? ItsmPaymentTypeId { get; set; }
    public Period? Period { get; set; }
    public int? PeriodId { get; set; }
    public bool Active { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}