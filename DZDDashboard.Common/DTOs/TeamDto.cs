namespace DZDDashboard.Common.DTOs
{
    public record TeamDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
    }
}
