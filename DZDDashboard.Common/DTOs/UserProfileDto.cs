
namespace DZDDashboard.Common.DTOs
{
    public record UserProfileDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PersonalEmail { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PersonalPhoneNumber { get; set; }
        public DateTime? UserStartDate { get; set; }
        public DateTime? PositionStartDate { get; set; }
        public string? ContractType { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string? WorkModel { get; set; }
        public string? CompanyName { get; set; }
        public string? UnitName { get; set; }
        public string? ApprovalProcessUnit { get; set; }
        public int? Grade { get; set; }
        public UserProfileReportsToDto? ReportsTo { get; set; }
        public DepartmentDto? Department { get; set; }
        public TeamDto? Team { get; set; }
        public JobDto? Job { get; set; }
        public PayrollLocationDto? PayrollLocation { get; set; }
        public UserAvatarDto? Avatar { get; set; }
    }
}

