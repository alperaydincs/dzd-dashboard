namespace DZDDashboard.Common.DTOs
{
    public class CareerMapRuleDto
    {
        public int Id { get; set; }
        public int CareerPathId { get; set; }
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

        // Linked job titles
        public List<int> PositionJobIds { get; set; } = new();
        public List<JobDto> PositionJobs { get; set; } = new();
    }
}
