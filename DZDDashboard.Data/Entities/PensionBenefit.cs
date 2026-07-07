namespace DZDDashboard.Data.Entities;

public class PensionBenefit : BenefitPayment
{
    public decimal? EmployeeContributionAmount { get; set; }
    public decimal? EmployerContributionAmount { get; set; }
    public string?  PolicyNumber               { get; set; }
}
