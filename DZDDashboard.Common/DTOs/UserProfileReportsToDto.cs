
namespace DZDDashboard.Common.DTOs
{
    public class UserProfileReportsToDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DepartmentDto? Department { get; set; }
        public JobDto? Job { get; set; }
        public UserAvatarDto? Avatar { get; set; }
    }
}

