namespace DZDDashboard.Data.Entities;

public partial class User
{
    // ── Audit history (Restrict delete — see SalaryHistoryConfiguration / GradeHistoryConfiguration) ─
    // These navigation properties are defined for future salary/grade history tracking.
    // No service writes to these tables yet. The delete guard in UserService.DeleteAsync
    // is intentionally kept as a safety net so the restriction is enforced automatically
    // once history records start being created.
    // Roadmap: add service methods + API endpoints + Blazor pages before using these.
    public List<SalaryHistory>? SalaryHistories { get; set; }
    public List<GradeHistory>?  GradeHistories  { get; set; }

    // ── Operational collections (Cascade delete — operational data) ──────────
    public List<EmergencyContact>?  EmergencyContacts  { get; set; }
    public List<EducationHistory>?  EducationHistories { get; set; }
    public List<UserTraining>?      UserTrainings      { get; set; }
    public List<TargetEffort>?      TargetEfforts      { get; set; }
    public List<ExCompanyHistory>?  ExCompanyHistories { get; set; }
}
