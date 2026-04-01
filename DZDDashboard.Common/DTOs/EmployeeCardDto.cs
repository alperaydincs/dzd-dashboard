namespace DZDDashboard.Common.DTOs;

public class EmployeeCardDto
{
    public int Id { get; set; }

    public string? FullName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? CompanyName { get; set; }
    public string? RegistrationNumber { get; set; }

    public int? Grade { get; set; }
    public int? JobId { get; set; }
    public int? DepartmentId { get; set; }
    public int? TeamId { get; set; }
    public int? PayrollLocationId { get; set; }
    public int? ReportsToId { get; set; }
    public int? OrganizationPositionId { get; set; }

    public DateTime? PositionStartDate { get; set; }
    public DateTime? UserStartDate { get; set; }
    public string? ContractType { get; set; }
    public DateTime? ContractEndDate { get; set; }
    public string? WorkModel { get; set; }

    public string? UnitName { get; set; }
    public string? ApprovalProcessUnit { get; set; }
    public string? OrganizationPositionName { get; set; }

    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? PersonalEmail { get; set; }
    public string? PersonalPhoneNumber { get; set; }

    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public bool DisabilityStatus { get; set; }
    public string? DisabilityDegree { get; set; }
    public string? Nationality { get; set; }
    public string? CitizenshipNumber { get; set; }

    public string? MaritalStatus { get; set; }
    public string? SpouseFullName { get; set; }

    public string? LegalAddress { get; set; }
    public string? CurrentAddress { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }

    public List<EducationHistoryDto> EducationHistories { get; set; } = new();

    public DepartmentDto? Department { get; set; }
    public TeamDto? Team { get; set; }
    public JobDto? Job { get; set; }
    public PayrollLocationDto? PayrollLocation { get; set; }
    public UserAvatarDto? Avatar { get; set; }
    public UserProfileReportsToDto? ReportsTo { get; set; }

    public List<TargetEffortDto>? TargetEfforts { get; set; }
    public List<UserTrainingDto>? UserTrainings { get; set; }
    public List<EmergencyContactDto> EmergencyContacts { get; set; } = new();
    public List<ChildInfoDto> Children { get; set; } = new();
}
