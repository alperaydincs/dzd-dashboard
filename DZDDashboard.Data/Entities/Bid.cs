namespace DZDDashboard.Data.Entities;

public class Bid : IAuditableEntity
{
    public int Id { get; set; }
    public Bank? Bank { get; set; }
    public int? BankId { get; set; }
    public string? JiraProjectNo { get; set; }
    public string? ProjectName { get; set; }
    public DzdStatus? DzdStatus { get; set; }
    public int? DzdStatusId { get; set; }
    public Department? Department { get; set; }
    public int? DepartmentId { get; set; }
    public Team? Team { get; set; }
    public int? TeamId { get; set; }
    public Period? Period { get; set; }
    public int? PeriodId { get; set; }
    public User? ProjectManager { get; set; }
    public int? ProjectManagerId { get; set; }
    public User? Analyst { get; set; }
    public int? AnalystId { get; set; }
    public User? Developer { get; set; }
    public int? DeveloperId { get; set; }
    public DateTime? CloseDate { get; set; }
    public DateTime? AnalysisDate { get; set; }
    public DateTime? UatDate { get; set; }
    public string? TshirtSize { get; set; }
    public decimal Budget { get; set; }
    public string? Notes { get; set; }
    public Salesforce? IntertechTeam { get; set; }
    public int? IntertechTeamId { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}
