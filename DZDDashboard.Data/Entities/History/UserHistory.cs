using DZDDashboard.Data.Abstractions;

namespace DZDDashboard.Data.Entities.History;

public class UserHistory : IHistoryEntity
{
    public long HistoryId { get; set; }
    public HistoryOperation Operation { get; set; }
    public DateTime HistoryRecordedAt { get; set; }
    public int? HistoryRecordedById { get; set; }

    public int Id { get; set; }
    public string? EntraObjectId { get; set; }
    public string? Email { get; set; }
    public string? NormalizedEmail { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsActive { get; set; }
    public string Slug { get; set; } = string.Empty;
    public int? AvatarColorIndex { get; set; }
    public string? RegistrationNumber { get; set; }
    public DateTime? UserStartDate { get; set; }
    public DateTime? PositionStartDate { get; set; }
    public DateTime? PositionUpdateDate { get; set; }
    public string? ContractType { get; set; }
    public DateTime? ContractEndDate { get; set; }
    public string? WorkModel { get; set; }
    public int? CompanyId { get; set; }
    public int? DepartmentId { get; set; }
    public int? TeamId { get; set; }
    public int? PayrollLocationId { get; set; }
    public int? OrganizationPositionId { get; set; }
    public int? ReportsToId { get; set; }
    public int? JobId { get; set; }
    public int? Grade { get; set; }
    public int? CareerPathId { get; set; }
    public int? AvatarId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? PersonalEmail { get; set; }
    public string? PersonalPhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Nationality { get; set; }
    public string? CitizenshipNumber { get; set; }
    public bool DisabilityStatus { get; set; }
    public string? DisabilityDegree { get; set; }
    public string? MaritalStatus { get; set; }
    public string? SpouseFullName { get; set; }
    public string? LegalAddress { get; set; }
    public string? LegalAddressCity { get; set; }
    public string? LegalAddressCountry { get; set; }
    public string? CurrentAddress { get; set; }
    public DateTime? CurrentAddressChangedAt { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string LifecycleStatus { get; set; } = string.Empty;
}
