namespace DZDDashboard.Common.DTOs;

public record ProcessDocumentDto
{
    public int      Id         { get; set; }
    public string   Name       { get; set; } = string.Empty;
    public bool     IsRequired { get; set; }
    public DateTime Deadline   { get; set; }
    public string   Status     { get; set; } = string.Empty;
    public string?   FileName   { get; set; }
    public DateTime? UploadedAt { get; set; }
    public DateTime? ReviewedAt     { get; set; }
    public string?   ReviewedByName { get; set; }
}

public record AddProcessDocumentDto
{
    public string   Name       { get; set; } = string.Empty;
    public bool     IsRequired { get; set; } = true;
    public DateTime Deadline   { get; set; }
}
