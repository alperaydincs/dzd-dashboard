namespace DZDDashboard.Common.DTOs
{
    public record RoleDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}
