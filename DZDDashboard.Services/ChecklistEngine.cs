using DZDDashboard.Common.Constants;
using DZDDashboard.Common.Exceptions;
using DZDDashboard.Common.Utils;
using DZDDashboard.Data;
using DZDDashboard.Data.Abstractions;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Services;

public class LifecycleEngine(AppDbContext context, IAuditProvider audit, IFileStorageService fileStorage)
{
    public async Task<List<ChecklistItem>> BuildChecklistItemsAsync(int processTemplateId, CancellationToken cancellationToken)
    {
        var templates = await context.ChecklistStepTemplates.AsNoTracking()
            .Where(t => t.ProcessTemplateId == processTemplateId)
            .OrderBy(t => t.Sequence)
            .ToListAsync(cancellationToken);

        return [.. templates.Select(t => new ChecklistItem
        {
            Title      = t.Title,
            Sequence   = t.Sequence,
            IsRequired = t.IsRequired,
            Status     = ChecklistItemStatuses.Pending
        })];
    }

    public async Task<List<ProcessDocument>> BuildDocumentsAsync(int processTemplateId, DateTime anchorDate, CancellationToken cancellationToken)
    {
        var templates = await context.DocumentTemplates.AsNoTracking()
            .Where(t => t.ProcessTemplateId == processTemplateId)
            .OrderBy(t => t.Sequence)
            .ToListAsync(cancellationToken);

        return [.. templates.Select(t => new ProcessDocument
        {
            Name       = t.Name,
            IsRequired = t.IsRequired,
            Deadline   = anchorDate.AddDays(t.DeadlineDays),
            Status     = DocumentReviewStatuses.Pending
        })];
    }

    public async Task CompleteChecklistItemAsync(ChecklistItem item, CancellationToken cancellationToken)
    {
        if (item.Status == ChecklistItemStatuses.Completed) return;

        item.Status        = ChecklistItemStatuses.Completed;
        item.CompletedAt   = audit.GetNow();
        item.CompletedById = audit.GetCurrentUserId();
        await LogAsync(item.OnboardingProcessId, item.OffboardingProcessId, OnboardingOffboardingAuditActions.ChecklistCompleted, item.Title, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task ReopenChecklistItemAsync(ChecklistItem item, CancellationToken cancellationToken)
    {
        item.Status        = ChecklistItemStatuses.Pending;
        item.CompletedAt    = null;
        item.CompletedById  = null;
        await LogAsync(item.OnboardingProcessId, item.OffboardingProcessId, OnboardingOffboardingAuditActions.ChecklistReopened, item.Title, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<ProcessDocument> AddDocumentAsync(int? onboardingProcessId, int? offboardingProcessId, AddProcessDocumentDto dto, CancellationToken cancellationToken)
    {
        var doc = new ProcessDocument
        {
            OnboardingProcessId  = onboardingProcessId,
            OffboardingProcessId = offboardingProcessId,
            Name       = dto.Name.Trim(),
            IsRequired = dto.IsRequired,
            Deadline   = dto.Deadline,
            Status     = DocumentReviewStatuses.Pending
        };
        context.ProcessDocuments.Add(doc);
        await LogAsync(onboardingProcessId, offboardingProcessId, OnboardingOffboardingAuditActions.DocumentAdded, doc.Name, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return doc;
    }

    public async Task UploadDocumentAsync(ProcessDocument document, string fileName, string contentType, byte[] content, CancellationToken cancellationToken)
    {
        if (document.FileId is { } existing)
            await fileStorage.DeleteAsync(existing, cancellationToken);

        document.FileId      = await fileStorage.SaveAsync(content, contentType, cancellationToken);
        document.FileName    = fileName;
        document.ContentType = contentType;
        document.Status      = DocumentReviewStatuses.Uploaded;
        document.UploadedAt  = audit.GetNow();
        document.UploadedById = audit.GetCurrentUserId();
        document.ReviewedAt   = null;
        document.ReviewedById = null;

        await LogAsync(document.OnboardingProcessId, document.OffboardingProcessId, OnboardingOffboardingAuditActions.DocumentUploaded, $"{document.Name}: {fileName}", cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task ApproveDocumentAsync(ProcessDocument document, CancellationToken cancellationToken)
    {
        if (document.FileId is null)
            throw new DomainValidationException("Onaylanmadan önce belge yüklenmelidir.");

        document.Status       = DocumentReviewStatuses.Approved;
        document.ReviewedAt   = audit.GetNow();
        document.ReviewedById = audit.GetCurrentUserId();

        await LogAsync(document.OnboardingProcessId, document.OffboardingProcessId, OnboardingOffboardingAuditActions.DocumentApproved, document.Name, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RequestDocumentCorrectionAsync(ProcessDocument document, CancellationToken cancellationToken)
    {
        document.Status       = DocumentReviewStatuses.NeedsCorrection;
        document.ReviewedAt   = audit.GetNow();
        document.ReviewedById = audit.GetCurrentUserId();

        await LogAsync(document.OnboardingProcessId, document.OffboardingProcessId, OnboardingOffboardingAuditActions.DocumentCorrectionRequested, document.Name, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task ReopenDocumentAsync(ProcessDocument document, CancellationToken cancellationToken)
    {
        document.Status       = document.FileId is null ? DocumentReviewStatuses.Pending : DocumentReviewStatuses.Uploaded;
        document.ReviewedAt   = null;
        document.ReviewedById = null;

        await LogAsync(document.OnboardingProcessId, document.OffboardingProcessId, OnboardingOffboardingAuditActions.ChecklistReopened, document.Name, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteDocumentAsync(ProcessDocument document, CancellationToken cancellationToken)
    {
        if (document.FileId is { } fileId)
            await fileStorage.DeleteAsync(fileId, cancellationToken);

        await LogAsync(document.OnboardingProcessId, document.OffboardingProcessId, OnboardingOffboardingAuditActions.DocumentDeleted, document.Name, cancellationToken);
        context.ProcessDocuments.Remove(document);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<(byte[] Content, string? ContentType, string FileName)?> GetDocumentContentAsync(ProcessDocument document, CancellationToken cancellationToken)
    {
        if (document.FileId is not { } storageId) return null;
        var file = await fileStorage.GetAsync(storageId, cancellationToken);
        if (file is null) return null;
        return (file.Value.Content, file.Value.ContentType ?? document.ContentType, document.FileName ?? "document");
    }

    public async Task LogAsync(int? onboardingProcessId, int? offboardingProcessId, string action, string detail, CancellationToken cancellationToken)
    {
        context.LifecycleAuditLogEntries.Add(new LifecycleAuditLogEntry
        {
            OnboardingProcessId  = onboardingProcessId,
            OffboardingProcessId = offboardingProcessId,
            Action        = action,
            Detail        = detail,
            PerformedById = audit.GetCurrentUserId(),
            CreatedAt     = audit.GetNow()
        });
        await Task.CompletedTask;
    }

    public static ChecklistItemDto MapItem(ChecklistItem item) => new()
    {
        Id          = item.Id,
        Title       = item.Title,
        IsRequired  = item.IsRequired,
        Status      = item.Status,
        CompletedAt = item.CompletedAt,
        CompletedByName = item.CompletedBy != null
            ? AppFormatter.BuildFullName(item.CompletedBy.FirstName, item.CompletedBy.LastName)
            : null
    };

    public static ProcessDocumentDto MapDocument(ProcessDocument doc) => new()
    {
        Id             = doc.Id,
        Name           = doc.Name,
        IsRequired     = doc.IsRequired,
        Deadline       = doc.Deadline,
        Status         = doc.Status,
        FileName       = doc.FileName,
        UploadedAt     = doc.UploadedAt,
        ReviewedAt     = doc.ReviewedAt,
        ReviewedByName = doc.ReviewedBy != null
            ? AppFormatter.BuildFullName(doc.ReviewedBy.FirstName, doc.ReviewedBy.LastName)
            : null
    };

    public static AuditLogEntryDto MapAuditEntry(LifecycleAuditLogEntry entry) => new()
    {
        Id              = entry.Id,
        Action          = entry.Action,
        Detail          = entry.Detail,
        PerformedByName = entry.PerformedBy != null
            ? AppFormatter.BuildFullName(entry.PerformedBy.FirstName, entry.PerformedBy.LastName)
            : null,
        CreatedAt       = entry.CreatedAt
    };
}
