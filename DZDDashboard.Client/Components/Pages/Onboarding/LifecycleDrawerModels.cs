using DZDDashboard.Common.DTOs;
using Microsoft.AspNetCore.Components.Forms;

namespace DZDDashboard.Client.Components.Pages.Onboarding;

public record LifecycleProcessView(
    int Id,
    string ProcessTypeLabel,
    string? EmployeeName,
    string? ManagerName,
    DateTime Date,
    string Status,
    List<ChecklistItemDto> Items,
    List<ProcessDocumentDto> Documents,
    List<AuditLogEntryDto> AuditLog);

public class LifecycleDrawerActions
{
    public Func<int, Task> CompleteItem { get; set; } = default!;
    public Func<int, Task> ReopenItem { get; set; } = default!;
    public Func<AddProcessDocumentDto, Task> AddDocument { get; set; } = default!;
    public Func<int, IBrowserFile, Task> UploadDocument { get; set; } = default!;
    public Func<int, Task> ApproveDocument { get; set; } = default!;
    public Func<int, Task> RequestCorrection { get; set; } = default!;
    public Func<int, Task> ReopenDocument { get; set; } = default!;
    public Func<int, Task> DeleteDocument { get; set; } = default!;
    public Func<int, Task> DownloadDocument { get; set; } = default!;
}
