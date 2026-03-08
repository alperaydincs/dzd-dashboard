namespace DZDDashboard.Common.DTOs
{
    public record JobDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Level { get; set; }
    }
}

