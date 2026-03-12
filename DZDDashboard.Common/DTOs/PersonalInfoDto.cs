namespace DZDDashboard.Common.DTOs;

public class PersonalInfoDto
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
    public DateTime? PositionStartDate { get; set; }
    public DateTime? UserStartDate { get; set; }
    public string? ContractType { get; set; }
    public DateTime? ContractEndDate { get; set; }
    public string? WorkModel { get; set; }
    public int? PayrollLocationId { get; set; }
    public int? ReportsToId { get; set; }
    public string? ApprovalProcessUnit { get; set; }

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
    public List<EmergencyContactDto> EmergencyContacts { get; set; } = new();
    public string? MaritalStatus { get; set; }
    public string? SpouseFullName { get; set; }

    public List<ChildInfoDto> Children { get; set; } = new();
    public string? LegalAddress { get; set; }
    public string? CurrentAddress { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? EducationStatus { get; set; }
    public string? HighestEducationLevel { get; set; }
    public string? HighSchoolName { get; set; }
    public string? BachelorsUniversityName { get; set; }
    public string? BachelorsProgramName { get; set; }
    public string? MastersUniversityName { get; set; }
    public string? MastersProgramName { get; set; }
    public DateTime? BachelorsGraduatedDate { get; set; }
    public DateTime? MastersGraduatedDate { get; set; }
}

