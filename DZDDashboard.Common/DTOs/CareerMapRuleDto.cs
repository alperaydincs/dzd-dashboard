namespace DZDDashboard.Common.DTOs;

// Validation is handled exclusively by CareerMapRuleDtoValidator (FluentValidation).
// Data annotations removed to avoid dual-validation with different error formats.
public record CareerMapRuleDto
{
    public int Id            { get; set; }
    public int CareerPathId  { get; set; }

    public int Grade { get; set; }

    /// <summary>Minimum time in the current role. Only one of Months/Years should be set.</summary>
    public RoleDurationDto MinRoleTime   { get; set; } = new();
    /// <summary>Minimum total experience. Only one of Months/Years should be set.</summary>
    public RoleDurationDto MinExperience { get; set; } = new();

    // Requirement flags
    public bool ManagerPerformanceEvaluation { get; set; }
    public bool AssessmentCenterApplication  { get; set; }
    public bool TechnicalInterview           { get; set; }
    public bool CaseStudy                    { get; set; }
    public bool EnglishProficiency           { get; set; }
    public int? ProjectObjective             { get; set; }
    public bool CommitteeApproval            { get; set; }

    // Linked job titles
    public List<int>    PositionJobIds { get; set; } = [];
    public List<JobDto> PositionJobs   { get; set; } = [];
}
