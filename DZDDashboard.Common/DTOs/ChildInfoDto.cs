namespace DZDDashboard.Common.DTOs
{
    public record ChildInfoDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}

