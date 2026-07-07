namespace DZDDashboard.Data.Entities;

public class HealthInsuranceBenefit : BenefitPayment
{
    public List<BenefitPaymentDependent> Dependents { get; set; } = [];
}
