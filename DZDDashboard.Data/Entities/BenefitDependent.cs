namespace DZDDashboard.Data.Entities;

public class BenefitDependent : AuditableEntity
{
    public int Id { get; set; }

    public int Order { get; set; }

    public string? DependentName { get; set; }

    public int? DependentTypeId { get; set; }
    public DependentTypeEntity? DependentTypeRef { get; set; }

    public decimal Amount { get; set; }

    public DateTime  StartDate { get; set; }
    public DateTime? EndDate   { get; set; }

    public int BenefitRecordId { get; set; }
    public BenefitRecord? BenefitRecord { get; set; }
}
