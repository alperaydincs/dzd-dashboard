namespace DZDDashboard.Common.DTOs
{
    public record WorkTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}

