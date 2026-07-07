namespace DZDDashboard.Data.Entities;

public partial class User
{
    public Salary? Salary { get; set; }
    public Position? Position { get; set; }

    public List<BenefitPayment>?    BenefitPayments    { get; set; }
    public List<AdditionalPayment>? AdditionalPayments { get; set; }
    public List<Deduction>?         Deductions         { get; set; }

    public List<EmergencyContact>?  EmergencyContacts  { get; set; }
    public List<Education>?  EducationHistories { get; set; }
}
