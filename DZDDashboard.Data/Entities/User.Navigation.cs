namespace DZDDashboard.Data.Entities;

public partial class User
{
    // ── Audit history (Restrict delete — see SalaryHistoryConfiguration / GradeHistoryConfiguration) ─
    // GradeHistory remains unwired (no service writes to it yet — same roadmap note as before).
    // SalaryHistory is now live: it backs the Payment screen's "Salary" tab via PaymentService.
    public List<SalaryHistory>? SalaryHistories { get; set; }
    public List<GradeHistory>?  GradeHistories  { get; set; }

    // ── Payment screen (Restrict delete — audit history must survive a user soft-delete) ───
    public List<BenefitRecord>?     BenefitRecords     { get; set; }
    public List<AdditionalPayment>? AdditionalPayments { get; set; }

    // ── Operational collections (Cascade delete — operational data) ──────────
    public List<EmergencyContact>?  EmergencyContacts  { get; set; }
    public List<EducationHistory>?  EducationHistories { get; set; }
    public List<UserTraining>?      UserTrainings      { get; set; }
    public List<TargetEffort>?      TargetEfforts      { get; set; }
    public List<ExCompanyHistory>?  ExCompanyHistories { get; set; }
}
