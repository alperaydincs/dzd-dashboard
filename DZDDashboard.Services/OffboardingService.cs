using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Exceptions;
using DZDDashboard.Common.Utils;
using DZDDashboard.Data;
using DZDDashboard.Data.Abstractions;
using DZDDashboard.Data.Entities;
using DZDDashboard.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;

public class OffboardingService(AppDbContext context, IAuditProvider audit, LifecycleEngine engine) : IOffboardingService
{
    public async Task<List<OffboardingListItemDto>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.OffboardingProcesses.AsNoTracking()
            .Include(p => p.User)
            .OrderByDescending(p => p.ExitDate)
            .Select(p => new OffboardingListItemDto
            {
                Id             = p.Id,
                UserId         = p.UserId,
                EmployeeName   = p.User != null ? AppFormatter.BuildFullName(p.User.FirstName, p.User.LastName) : null,
                EmployeeSlug   = p.User != null ? p.User.Slug : null,
                TemplateName   = p.TemplateName,
                ExitDate       = p.ExitDate,
                Status         = p.Status,
                CompletedCount = p.Items.Count(i => i.Status == ChecklistItemStatuses.Completed),
                TotalCount     = p.Items.Count
            })
            .ToListAsync(cancellationToken);

    public async Task<OffboardingProcessDto> GetAsync(int processId, CancellationToken cancellationToken = default)
        => Map(await LoadAsync(processId, cancellationToken));

    public async Task<OffboardingProcessDto> StartAsync(StartOffboardingDto dto, CancellationToken cancellationToken = default)
    {
        var template = await RequireOffboardingTemplateAsync(dto.TemplateId, cancellationToken);

        var user = await context.Users.FindRequiredAsync(dto.UserId, nameof(User), cancellationToken);

        var hasOpen = await context.OffboardingProcesses
            .AnyAsync(p => p.UserId == dto.UserId && p.Status != ProcessStatuses.Cancelled, cancellationToken);
        if (hasOpen)
            throw new DomainConflictException("Bu çalışan için zaten bir offboarding süreci mevcut.");

        user.LifecycleStatus = UserLifecycleStatuses.Offboarding;

        var process = new OffboardingProcess
        {
            UserId       = user.Id,
            TemplateId   = template.Id,
            TemplateName = template.Name,
            ExitDate     = dto.ExitDate,
            Status       = ProcessStatuses.InProgress,
            Items        = await engine.BuildChecklistItemsAsync(template.Id, cancellationToken),
            Documents    = await engine.BuildDocumentsAsync(template.Id, dto.ExitDate, cancellationToken)
        };
        context.OffboardingProcesses.Add(process);
        await context.SaveChangesAsync(cancellationToken);

        await engine.LogAsync(null, process.Id, OnboardingOffboardingAuditActions.Started, "Offboarding started", cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return await GetAsync(process.Id, cancellationToken);
    }

    public async Task<OffboardingProcessDto> CompleteItemAsync(int processId, int itemId, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.CompleteChecklistItemAsync(RequireItem(process, itemId), cancellationToken);
        await RecomputeAsync(process, cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OffboardingProcessDto> ReopenItemAsync(int processId, int itemId, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.ReopenChecklistItemAsync(RequireItem(process, itemId), cancellationToken);
        await RecomputeAsync(process, cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OffboardingProcessDto> AddDocumentAsync(int processId, AddProcessDocumentDto dto, CancellationToken cancellationToken = default)
    {
        await LoadAsync(processId, cancellationToken);
        await engine.AddDocumentAsync(null, processId, dto, cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OffboardingProcessDto> UploadDocumentAsync(int processId, int documentId, string fileName, string contentType, byte[] content, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.UploadDocumentAsync(RequireDocument(process, documentId), fileName, contentType, content, cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OffboardingProcessDto> ApproveDocumentAsync(int processId, int documentId, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.ApproveDocumentAsync(RequireDocument(process, documentId), cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OffboardingProcessDto> RequestDocumentCorrectionAsync(int processId, int documentId, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.RequestDocumentCorrectionAsync(RequireDocument(process, documentId), cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OffboardingProcessDto> ReopenDocumentAsync(int processId, int documentId, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.ReopenDocumentAsync(RequireDocument(process, documentId), cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OffboardingProcessDto> DeleteDocumentAsync(int processId, int documentId, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.DeleteDocumentAsync(RequireDocument(process, documentId), cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<(byte[] Content, string? ContentType, string FileName)?> GetDocumentAsync(int processId, int documentId, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        return await engine.GetDocumentContentAsync(RequireDocument(process, documentId), cancellationToken);
    }

    public async Task<List<DueSoonDocumentDto>> GetDueSoonDocumentsAsync(CancellationToken cancellationToken = default)
    {
        var threshold = audit.GetNow().Date.AddDays(1);
        return await context.ProcessDocuments.AsNoTracking()
            .Where(d => d.OffboardingProcessId != null
                     && d.Status != DocumentReviewStatuses.Approved
                     && d.Deadline.Date <= threshold)
            .Include(d => d.OffboardingProcess!).ThenInclude(p => p!.User)
            .Select(d => new DueSoonDocumentDto
            {
                ProcessId    = d.OffboardingProcessId!.Value,
                Kind         = ProcessTemplateKinds.Offboarding,
                EmployeeName = d.OffboardingProcess!.User != null ? AppFormatter.BuildFullName(d.OffboardingProcess.User!.FirstName, d.OffboardingProcess.User!.LastName) : null,
                DocumentName = d.Name,
                Deadline     = d.Deadline
            })
            .ToListAsync(cancellationToken);
    }

    private async Task<OffboardingProcess> LoadAsync(int processId, CancellationToken cancellationToken)
        => await context.OffboardingProcesses
            .Include(p => p.User)
            .Include(p => p.Items).ThenInclude(i => i.CompletedBy)
            .Include(p => p.Documents).ThenInclude(d => d.ReviewedBy)
            .Include(p => p.AuditLog).ThenInclude(a => a.PerformedBy)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == processId, cancellationToken)
           ?? throw new EntityNotFoundException(nameof(OffboardingProcess), processId);

    private static ChecklistItem RequireItem(OffboardingProcess process, int itemId)
        => process.Items.FirstOrDefault(i => i.Id == itemId)
           ?? throw new EntityNotFoundException(nameof(ChecklistItem), itemId);

    private static ProcessDocument RequireDocument(OffboardingProcess process, int documentId)
        => process.Documents.FirstOrDefault(d => d.Id == documentId)
           ?? throw new EntityNotFoundException(nameof(ProcessDocument), documentId);

    private async Task<ProcessTemplate> RequireOffboardingTemplateAsync(int templateId, CancellationToken cancellationToken)
        => await context.ProcessTemplates
            .FirstOrDefaultAsync(t => t.Id == templateId && t.Kind == ProcessTemplateKinds.Offboarding, cancellationToken)
            ?? throw new DomainValidationException("Geçersiz offboarding şablonu.");

    private async Task RecomputeAsync(OffboardingProcess process, CancellationToken cancellationToken)
    {
        var allRequiredDone = process.Items.Where(i => i.IsRequired).All(i => i.Status == ChecklistItemStatuses.Completed);

        if (allRequiredDone && process.Status != ProcessStatuses.Completed)
        {
            process.Status      = ProcessStatuses.Completed;
            process.CompletedAt = audit.GetNow();
            if (process.User is { } u)
            {
                u.LifecycleStatus = UserLifecycleStatuses.Exited;
                u.IsActive        = false;
                u.ContractEndDate ??= process.ExitDate;
            }
            await engine.LogAsync(null, process.Id, OnboardingOffboardingAuditActions.ProcessCompleted, "Offboarding completed", cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        else if (!allRequiredDone && process.Status == ProcessStatuses.Completed)
        {
            process.Status      = ProcessStatuses.InProgress;
            process.CompletedAt = null;
            if (process.User is { } u)
            {
                u.LifecycleStatus = UserLifecycleStatuses.Offboarding;
                u.IsActive        = true;
            }
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    private static OffboardingProcessDto Map(OffboardingProcess p) => new()
    {
        Id           = p.Id,
        UserId       = p.UserId,
        EmployeeName = p.User != null ? AppFormatter.BuildFullName(p.User.FirstName, p.User.LastName) : null,
        EmployeeSlug = p.User?.Slug,
        TemplateName = p.TemplateName,
        ExitDate     = p.ExitDate,
        Status       = p.Status,
        CompletedAt  = p.CompletedAt,
        Items        = [.. p.Items.OrderBy(i => i.Sequence).Select(LifecycleEngine.MapItem)],
        Documents    = [.. p.Documents.OrderBy(d => d.Deadline).Select(LifecycleEngine.MapDocument)],
        AuditLog     = [.. p.AuditLog.OrderByDescending(a => a.CreatedAt).Select(LifecycleEngine.MapAuditEntry)]
    };
}
