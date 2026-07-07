namespace DZDDashboard.Common.DTOs;

public record UserDocumentDto
{
    public int       Id          { get; set; }
    public string?   FileName    { get; set; }
    public string?   ContentType { get; set; }
    public long      SizeBytes   { get; set; }
    public DateTime? UploadedAt  { get; set; }
}
