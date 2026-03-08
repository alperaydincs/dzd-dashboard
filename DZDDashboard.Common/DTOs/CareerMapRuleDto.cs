
namespace DZDDashboard.Common.DTOs
{
    public record CareerMapRuleDto
    {
        public int Id { get; init; }
        public int JobId { get; init; }
        public JobDto? Job { get; init; }
        public int Grade { get; init; }
        public int? MinExperienceMonth { get; init; }
        public int? MinExperienceYear { get; init; }
        public int? MinRoleTimeMonth { get; init; }
        public int? MinRoleTimeyear { get; init; }
        public bool ManagerPerformanceEvaluation { get; init; }
        public bool AssesmentCenterApplication { get; init; }
        public bool TechnicalInterview { get; init; }
        public bool CaseStudy { get; init; }
        public bool EnglishProficiency { get; init; }
        public int? ProjectObjective { get; init; }
        public bool CommitteeApproval { get; init; }
    }
}

