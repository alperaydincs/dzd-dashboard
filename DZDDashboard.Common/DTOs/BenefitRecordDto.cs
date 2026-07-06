namespace DZDDashboard.Common.DTOs;

public record BenefitRecordDto
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
    public List<BenefitDependentDto> Dependents { get; set; } = [];
}
