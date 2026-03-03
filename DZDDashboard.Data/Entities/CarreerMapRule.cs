using System;
using System.Collections.Generic;
namespace DZDDashboard.Data.Entities;

public class CareerMapRule : IAuditableEntity
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public Job? Job { get; set; }
    public int Grade { get; set; }
    public int? MinExperienceMonth { get; set; }
    public int? MinExperienceYear { get; set; }
    public int? MinRoleTimeMonth { get; set; }
    public int? MinRoleTimeyear { get; set; }
    public bool ManagerPerformanceEvaluation { get; set; }
    public bool AssesmentCenterApplication { get; set; }
    public bool TechnicalInterview { get; set; }
    public bool CaseStudy { get; set; }
    public bool EnglishProficiency { get; set; }
    public int? ProjectObjective { get; set; }
    public bool CommitteeApproval { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
}
