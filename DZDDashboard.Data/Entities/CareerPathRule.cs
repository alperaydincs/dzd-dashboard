namespace DZDDashboard.Data.Entities;

public class CareerPathRule : EntityWithHistory
{
    public int Id { get; set; }
    public int CareerPathId { get; set; }
    public CareerPath? CareerPath { get; set; }
    public int          Grade          { get; set; }
    public RoleDuration MinRoleTime    { get; set; } = new();
    public RoleDuration MinExperience  { get; set; } = new();
    public bool ManagerPerformanceEvaluation { get; set; }
    public bool AssessmentCenterApplication { get; set; }
    public bool TechnicalInterview { get; set; }
    public bool CaseStudy { get; set; }
    public bool EnglishProficiency { get; set; }
    public int? ProjectObjective { get; set; }
    public bool CommitteeApproval { get; set; }

    public decimal? SalaryIncreasePercent { get; set; }

    public decimal? PrivatePensionInsuranceAmount   { get; set; }
    public string?  PrivatePensionInsuranceCurrency { get; set; }

    public decimal? EmployerContributionUpperLimitAmount   { get; set; }
    public string?  EmployerContributionUpperLimitCurrency { get; set; }

    public decimal? MealAllowanceAmount   { get; set; }
    public string?  MealAllowanceCurrency { get; set; }

    public ICollection<CareerPathRuleJob> Positions { get; set; } = new List<CareerPathRuleJob>();
}
