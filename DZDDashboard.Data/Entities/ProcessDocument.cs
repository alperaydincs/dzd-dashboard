using DZDDashboard.Common.Constants;

namespace DZDDashboard.Data.Entities;

public class ProcessDocument : AuditableEntity
{
    public int Id { get; set; }
    public int? OnboardingProcessId { get; set; }
    public OnboardingProcess? OnboardingProcess { get; set; }
    public int? OffboardingProcessId { get; set; }
    public OffboardingProcess? OffboardingProcess { get; set; }
    public string   Name       { get; set; } = string.Empty;
    public bool     IsRequired { get; set; } = true;
    public DateTime Deadline   { get; set; }
    public string Status { get; set; } = DocumentReviewStatuses.Pending;
    public string?  FileName    { get; set; }
    public string?  ContentType { get; set; }
    public int?     FileId      { get; set; }
    public StoredFile? File     { get; set; }
    public DateTime? UploadedAt   { get; set; }
    public int?      UploadedById { get; set; }
    public User?     UploadedBy   { get; set; }
    public DateTime? ReviewedAt   { get; set; }
    public int?      ReviewedById { get; set; }
    public User?     ReviewedBy   { get; set; }
}