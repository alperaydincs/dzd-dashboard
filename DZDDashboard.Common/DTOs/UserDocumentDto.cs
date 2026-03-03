namespace DZDDashboard.Common.DTOs
{
    public record UserDocumentDto
    {
        public int Id { get; init; }
        public int UserId { get; init; }
        public int? DocumentCategoryId { get; init; }
        public UserDocumentCategoryDto? DocumentCategory { get; init; }
        public string? FileName { get; init; }
        public string? ContentType { get; init; }
        public bool IsActive { get; init; }
    }
}
