namespace DZDDashboard.Data.Entities;

public partial class User
{
    public List<SalaryHistory>? SalaryHistories { get; set; }
    public List<PositionHistory>? PositionHistories { get; set; }

    public List<BenefitRecord>?     BenefitRecords     { get; set; }
    public List<AdditionalPayment>? AdditionalPayments { get; set; }
    public List<Deduction>?         Deductions         { get; set; }

    public List<EmergencyContact>?  EmergencyContacts  { get; set; }
    public List<EducationHistory>?  EducationHistories { get; set; }
}
