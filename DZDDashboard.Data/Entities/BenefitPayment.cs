namespace DZDDashboard.Data.Entities;

/// <summary>
/// TPH base for the 3 benefit types (see BenefitTypes). Fields specific to one type live on
/// the corresponding subclass (HealthInsuranceBenefit.Dependents, PensionBenefit's contribution
/// fields) instead of as always-nullable columns here.
/// </summary>
public abstract class BenefitPayment : EntityWithHistory
{
    public int Id { get; set; }
    public string BenefitType { get; set; } = string.Empty;
    public string? BenefitName { get; set; }
    public decimal Amount   { get; set; }
    public string  Currency { get; set; } = string.Empty;
    public string  Period   { get; set; } = string.Empty;
    public DateTime  StartDate { get; set; }
    public DateTime? EndDate   { get; set; }
    public string? ProviderName { get; set; }
    public string? Notes        { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}
