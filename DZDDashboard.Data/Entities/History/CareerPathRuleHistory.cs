using DZDDashboard.Data.Abstractions;

namespace DZDDashboard.Data.Entities.History;

public class CareerPathRuleHistory : IHistoryEntity
{
    public long HistoryId { get; set; }
    public HistoryOperation Operation { get; set; }
    public DateTime HistoryRecordedAt { get; set; }
    public int? HistoryRecordedById { get; set; }

    public int Id { get; set; }
    public int CareerPathId { get; set; }
    public int Grade { get; set; }
    public int? MinRoleTimeMonths { get; set; }
    public int? MinRoleTimeYears { get; set; }
    public int? MinExperienceMonths { get; set; }
    public int? MinExperienceYears { get; set; }
    public bool ManagerPerformanceEvaluation { get; set; }
    public bool AssessmentCenterApplication { get; set; }
    public bool TechnicalInterview { get; set; }
    public bool CaseStudy { get; set; }
    public bool EnglishProficiency { get; set; }
    public int? ProjectObjective { get; set; }
    public bool CommitteeApproval { get; set; }
    public decimal? SalaryIncreasePercent { get; set; }
    public decimal? PrivatePensionInsuranceAmount { get; set; }
    public string? PrivatePensionInsuranceCurrency { get; set; }
    public decimal? EmployerContributionUpperLimitAmount { get; set; }
    public string? EmployerContributionUpperLimitCurrency { get; set; }
    public decimal? MealAllowanceAmount { get; set; }
    public string? MealAllowanceCurrency { get; set; }
}
