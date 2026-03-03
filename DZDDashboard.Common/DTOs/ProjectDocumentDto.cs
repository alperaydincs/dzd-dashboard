namespace DZDDashboard.Common.DTOs
{
    public record ProjectDocumentDto
    {
        public int Id { get; init; }
        public int? ProjectId { get; init; }
        public string? DocumentName { get; init; }
        public string? ContentType { get; init; }
    }
}
