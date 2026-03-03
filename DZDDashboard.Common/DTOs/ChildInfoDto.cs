namespace DZDDashboard.Common.DTOs
{
    public record ChildInfoDto
    {
        public int Id { get; init; }
        public string? FullName { get; init; }
        public DateTime? DateOfBirth { get; init; }
    }
}
