namespace DZDDashboard.Common.DTOs
{
    public record DefaultDocumentDto
    {
        public int Id { get; init; }
        public string Content { get; init; } = string.Empty;
        public string DocumentName { get; init; } = string.Empty;
        public string ContentType { get; init; } = string.Empty;
    }
}
