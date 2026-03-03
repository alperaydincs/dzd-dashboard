namespace DZDDashboard.Common.DTOs.Users
{
    public record EmployeeDetailDto
    {
        public int Id { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? Email { get; init; }
        public string? PersonalEmail { get; init; }
        public string? PhoneNumber { get; init; }
        public string? PersonalPhoneNumber { get; init; }
        public DateTime? UserStartDate { get; init; }
        public DateTime? PositionStartDate { get; init; }
        public string? ContractType { get; init; }
        public DateTime? ContractEndDate { get; init; }
        public string? WorkModel { get; init; }
        public string? CompanyName { get; init; }
        public string? UnitName { get; init; }
        public string? ApprovalProcessUnit { get; init; }
        public int? Grade { get; init; }
        public UserProfileReportsToDto? ReportsTo { get; init; }
        public DepartmentDto? Department { get; init; }
        public TeamDto? Team { get; init; }
        public JobDto? Job { get; init; }
        public PayrollLocationDto? PayrollLocation { get; init; }
        public UserAvatarDto? Avatar { get; init; }
        public List<TargetEffortDto>? TargetEfforts { get; init; }
        public List<UserTrainingDto>? UserTrainings { get; init; }
        public int? OrganizationPositionId { get; init; }
        public string? OrganizationPositionName { get; init; }
    }
}
