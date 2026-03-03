namespace DZDDashboard.Common.DTOs
{
    public record UserRoleDto
    {
        public int UserId { get; init; }
        public int RoleId { get; init; }
        public RoleDto? Role { get; init; }
    }
}
