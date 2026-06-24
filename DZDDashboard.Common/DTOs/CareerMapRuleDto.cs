namespace DZDDashboard.Common.DTOs;

public record CareerMapRuleDto
{
    public int Id            { get; set; }
    public int CareerPathId  { get; set; }

    public int Grade { get; set; }

    public RoleDurationDto MinRoleTime   { get; set; } = new();
    public RoleDurationDto MinExperience { get; set; } = new();

    public bool ManagerPerformanceEvaluation { get; set; }
    public bool AssessmentCenterApplication  { get; set; }
    public bool TechnicalInterview           { get; set; }
    public bool CaseStudy                    { get; set; }
    public bool EnglishProficiency           { get; set; }
    public int? ProjectObjective             { get; set; }
    public bool CommitteeApproval            { get; set; }

    public List<int>    PositionJobIds { get; set; } = [];
    public List<JobDto> PositionJobs   { get; set; } = [];
}
