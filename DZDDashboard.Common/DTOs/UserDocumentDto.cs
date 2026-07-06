namespace DZDDashboard.Common.DTOs;

public record UserDocumentDto
{
    public int       Id          { get; set; }
    public string?   FileName    { get; set; }
    public string?   ContentType { get; set; }
    public long      SizeBytes   { get; set; }
    public DateTime? UploadedAt  { get; set; }
    public string   ReviewStatus { get; set; } = string.Empty;
    public string?  ReviewNote   { get; set; }
}

public record ReviewDocumentDto
{
    public string  Status { get; set; } = string.Empty;
    public string? Note   { get; set; }
}
