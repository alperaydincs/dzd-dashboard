namespace DZDDashboard.Common.DTOs;

public record BenefitDependentDto
{
    public int Id { get; set; }

    public int Order { get; set; }

    public string? DependentName { get; set; }

    public string DependentType { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime  StartDate { get; set; }
    public DateTime? EndDate   { get; set; }
}
