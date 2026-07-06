namespace DZDDashboard.Common.DTOs;

public record BenefitDependentDto
{   
    public int Id { get; set; }
    public string? DependentName { get; set; }
    public string? RelationType { get; set; }
    public decimal Amount { get; set; }
    public DateTime  StartDate { get; set; }
    public DateTime? EndDate   { get; set; }
}
