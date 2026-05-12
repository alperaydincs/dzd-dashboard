namespace DZDDashboard.Data.Entities;

public class CareerMapRule : IAuditableEntity
{
    public int Id { get; set; }
    public int CareerPathId { get; set; }
    public CareerPath? CareerPath { get; set; }
    public int Grade { get; set; }

    // Minimum role duration
    public int? MinRoleTimeMonth { get; set; }
    public int? MinRoleTimeYear { get; set; }

    // Minimum general experience
    public int? MinExperienceMonth { get; set; }
    public int? MinExperienceYear { get; set; }

    // Requirement flags
    public bool ManagerPerformanceEvaluation { get; set; }
    public bool AssessmentCenterApplication { get; set; }
    public bool TechnicalInterview { get; set; }
    public bool CaseStudy { get; set; }
    public bool EnglishProficiency { get; set; }
    public int? ProjectObjective { get; set; }
    public bool CommitteeApproval { get; set; }

    // Positions (many-to-many with Job)
    public ICollection<CareerMapRulePosition> Positions { get; set; } = new List<CareerMapRulePosition>();

    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}
