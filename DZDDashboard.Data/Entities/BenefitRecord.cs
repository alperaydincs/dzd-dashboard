namespace DZDDashboard.Data.Entities;

public class BenefitRecord : AuditableEntity
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
    public decimal? EmployeeContributionAmount { get; set; }
    public decimal? EmployerContributionAmount { get; set; }
    public string?  PolicyNumber               { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public List<BenefitDependent> Dependents { get; set; } = [];
}
