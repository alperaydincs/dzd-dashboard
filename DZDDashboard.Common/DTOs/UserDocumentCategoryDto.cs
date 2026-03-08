namespace DZDDashboard.Common.DTOs
{
    public record UserDocumentCategoryDto
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public string? ContentType { get; init; }
        public bool IsActive { get; init; }
    }
}

