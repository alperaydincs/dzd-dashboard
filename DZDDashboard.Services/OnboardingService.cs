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

public class OnboardingService(
    AppDbContext context,
    IAuditProvider audit,
    LifecycleEngine engine) : IOnboardingService
{
    public async Task<List<OnboardingListItemDto>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.OnboardingProcesses.AsNoTracking()
            .Include(p => p.User)
            .OrderByDescending(p => p.StartDate)
            .Select(p => new OnboardingListItemDto
            {
                Id             = p.Id,
                UserId         = p.UserId,
                EmployeeName   = p.User != null ? AppFormatter.BuildFullName(p.User.FirstName, p.User.LastName) : null,
                EmployeeSlug   = p.User != null ? p.User.Slug : null,
                StartDate      = p.StartDate,
                TemplateName   = p.TemplateName,
                Status         = p.Status,
                CompletedCount = p.Items.Count(i => i.Status == ChecklistItemStatuses.Completed),
                TotalCount     = p.Items.Count
            })
            .ToListAsync(cancellationToken);

    public async Task<OnboardingProcessDto> GetAsync(int processId, CancellationToken cancellationToken = default)
        => Map(await LoadAsync(processId, cancellationToken));

    public async Task<OnboardingProcessDto> StartAsync(StartOnboardingDto dto, CancellationToken cancellationToken = default)
    {
        var template = await RequireOnboardingTemplateAsync(dto.TemplateId, cancellationToken);

        var user = new User
        {
            FirstName         = dto.FirstName.Trim(),
            LastName          = dto.LastName.Trim(),
            Email             = dto.Email!.Trim(),
            NormalizedEmail   = dto.Email.Trim().ToUpperInvariant(),
            CitizenshipNumber = dto.CitizenshipNumber?.Trim(),
            Slug              = await GenerateUniqueSlugAsync(dto.Email, cancellationToken),
            LifecycleStatus   = UserLifecycleStatuses.Onboarding,
            IsActive          = false
        };
        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        var process = new OnboardingProcess
        {
            UserId       = user.Id,
            StartDate    = dto.StartDate,
            ManagerId    = dto.ManagerId,
            TemplateId   = template.Id,
            TemplateName = template.Name,
            Status       = ProcessStatuses.InProgress,
            Items        = await engine.BuildChecklistItemsAsync(template.Id, cancellationToken),
            Documents    = await engine.BuildDocumentsAsync(template.Id, dto.StartDate, cancellationToken)
        };
        context.OnboardingProcesses.Add(process);
        await context.SaveChangesAsync(cancellationToken);

        await engine.LogAsync(process.Id, null, OnboardingOffboardingAuditActions.Started, "Onboarding started", cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return await GetAsync(process.Id, cancellationToken);
    }

    public async Task<OnboardingProcessDto> CompleteItemAsync(int processId, int itemId, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.CompleteChecklistItemAsync(RequireItem(process, itemId), cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OnboardingProcessDto> ReopenItemAsync(int processId, int itemId, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.ReopenChecklistItemAsync(RequireItem(process, itemId), cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OnboardingProcessDto> AddDocumentAsync(int processId, AddProcessDocumentDto dto, CancellationToken cancellationToken = default)
    {
        await LoadAsync(processId, cancellationToken);
        await engine.AddDocumentAsync(processId, null, dto, cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OnboardingProcessDto> UploadDocumentAsync(int processId, int documentId, string fileName, string contentType, byte[] content, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.UploadDocumentAsync(RequireDocument(process, documentId), fileName, contentType, content, cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OnboardingProcessDto> ApproveDocumentAsync(int processId, int documentId, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.ApproveDocumentAsync(RequireDocument(process, documentId), cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OnboardingProcessDto> RequestDocumentCorrectionAsync(int processId, int documentId, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.RequestDocumentCorrectionAsync(RequireDocument(process, documentId), cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OnboardingProcessDto> ReopenDocumentAsync(int processId, int documentId, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.ReopenDocumentAsync(RequireDocument(process, documentId), cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OnboardingProcessDto> DeleteDocumentAsync(int processId, int documentId, CancellationToken cancellationToken = default)
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
            .Where(d => d.OnboardingProcessId != null
                     && d.Status != DocumentReviewStatuses.Approved
                     && d.Deadline.Date <= threshold)
            .Include(d => d.OnboardingProcess!).ThenInclude(p => p!.User)
            .Select(d => new DueSoonDocumentDto
            {
                ProcessId    = d.OnboardingProcessId!.Value,
                Kind         = ProcessTemplateKinds.Onboarding,
                EmployeeName = d.OnboardingProcess!.User != null ? AppFormatter.BuildFullName(d.OnboardingProcess.User!.FirstName, d.OnboardingProcess.User!.LastName) : null,
                DocumentName = d.Name,
                Deadline     = d.Deadline
            })
            .ToListAsync(cancellationToken);
    }

    private async Task<OnboardingProcess> LoadAsync(int processId, CancellationToken cancellationToken)
        => await context.OnboardingProcesses
            .Include(p => p.User)
            .Include(p => p.Manager)
            .Include(p => p.Items).ThenInclude(i => i.CompletedBy)
            .Include(p => p.Documents).ThenInclude(d => d.ReviewedBy)
            .Include(p => p.AuditLog).ThenInclude(a => a.PerformedBy)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == processId, cancellationToken)
           ?? throw new EntityNotFoundException(nameof(OnboardingProcess), processId);

    private static ChecklistItem RequireItem(OnboardingProcess process, int itemId)
        => process.Items.FirstOrDefault(i => i.Id == itemId)
           ?? throw new EntityNotFoundException(nameof(ChecklistItem), itemId);

    private static ProcessDocument RequireDocument(OnboardingProcess process, int documentId)
        => process.Documents.FirstOrDefault(d => d.Id == documentId)
           ?? throw new EntityNotFoundException(nameof(ProcessDocument), documentId);

    private static bool AllRequiredDone(OnboardingProcess process)
        => process.Items.Where(i => i.IsRequired).All(i => i.Status == ChecklistItemStatuses.Completed);

    public async Task<OnboardingProcessDto> UpdateProcessAsync(int processId, UpdateOnboardingProcessDto dto, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        process.StartDate = dto.StartDate;
        process.ManagerId = dto.ManagerId;
        await context.SaveChangesAsync(cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OnboardingProcessDto> CompleteProcessAsync(int processId, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);

        if (process.Status == ProcessStatuses.Cancelled)
            throw new DomainConflictException("İptal edilmiş onboarding tamamlanamaz.");
        if (!AllRequiredDone(process))
            throw new DomainConflictException("Tüm zorunlu adımlar tamamlanmadan işe alım tamamlanamaz.");

        process.Status      = ProcessStatuses.Completed;
        process.CompletedAt = audit.GetNow();
        if (process.User is { } u)
        {
            u.LifecycleStatus = UserLifecycleStatuses.Active;
            u.IsActive        = true;
            u.UserStartDate ??= process.StartDate;
        }
        await engine.LogAsync(processId, null, OnboardingOffboardingAuditActions.ProcessCompleted, "Onboarding completed", cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task CancelAsync(int processId, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);

        process.Status      = ProcessStatuses.Cancelled;
        process.CompletedAt = null;
        if (process.User is { } u)
        {
            u.LifecycleStatus = UserLifecycleStatuses.Active;
            u.IsActive        = false;
        }
        await engine.LogAsync(processId, null, OnboardingOffboardingAuditActions.ProcessCancelled, "Onboarding cancelled", cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<MyOnboardingStateDto> GetOrStartMyAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await context.Users.FindRequiredAsync(userId, nameof(User), cancellationToken);

        if (user.LifecycleStatus != UserLifecycleStatuses.Onboarding && user.LifecycleStatus != UserLifecycleStatuses.Candidate)
            return new MyOnboardingStateDto { LifecycleStatus = user.LifecycleStatus, ProcessId = null };

        var process = await context.OnboardingProcesses
            .FirstOrDefaultAsync(p => p.UserId == userId && p.Status != ProcessStatuses.Cancelled, cancellationToken);

        if (process is null)
        {
            var template = await context.ProcessTemplates.AsNoTracking()
                .Where(t => t.Kind == ProcessTemplateKinds.Onboarding)
                .OrderBy(t => t.Sequence)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new DomainConflictException("Bir işe alım şablonu bulunamadı.");

            process = new OnboardingProcess
            {
                UserId       = userId,
                StartDate    = DateTime.UtcNow.Date,
                TemplateId   = template.Id,
                TemplateName = template.Name,
                Status       = ProcessStatuses.InProgress,
                Items        = await engine.BuildChecklistItemsAsync(template.Id, cancellationToken),
                Documents    = await engine.BuildDocumentsAsync(template.Id, DateTime.UtcNow.Date, cancellationToken)
            };
            context.OnboardingProcesses.Add(process);
            await context.SaveChangesAsync(cancellationToken);
        }

        return new MyOnboardingStateDto { LifecycleStatus = user.LifecycleStatus, ProcessId = process.Id };
    }

    private async Task<ProcessTemplate> RequireOnboardingTemplateAsync(int templateId, CancellationToken cancellationToken)
        => await context.ProcessTemplates
            .FirstOrDefaultAsync(t => t.Id == templateId && t.Kind == ProcessTemplateKinds.Onboarding, cancellationToken)
            ?? throw new DomainValidationException("Geçersiz onboarding şablonu.");

    private async Task<string> GenerateUniqueSlugAsync(string email, CancellationToken cancellationToken)
    {
        var baseSlug = SlugGenerator.FromEmail(email);
        var slug = baseSlug;
        var suffix = 2;
        while (await context.Users.AnyAsync(u => u.Slug == slug, cancellationToken))
            slug = $"{baseSlug}-{suffix++}";
        return slug;
    }

    private static OnboardingProcessDto Map(OnboardingProcess p) => new()
    {
        Id           = p.Id,
        UserId       = p.UserId,
        EmployeeName = p.User != null ? AppFormatter.BuildFullName(p.User.FirstName, p.User.LastName) : null,
        EmployeeSlug = p.User?.Slug,
        StartDate    = p.StartDate,
        ManagerId    = p.ManagerId,
        ManagerName  = p.Manager != null ? AppFormatter.BuildFullName(p.Manager.FirstName, p.Manager.LastName) : null,
        TemplateName = p.TemplateName,
        Status       = p.Status,
        CompletedAt  = p.CompletedAt,
        CanComplete  = p.Status == ProcessStatuses.InProgress && AllRequiredDone(p),
        Items        = [.. p.Items.OrderBy(i => i.Sequence).Select(LifecycleEngine.MapItem)],
        Documents    = [.. p.Documents.OrderBy(d => d.Deadline).Select(LifecycleEngine.MapDocument)],
        AuditLog     = [.. p.AuditLog.OrderByDescending(a => a.CreatedAt).Select(LifecycleEngine.MapAuditEntry)]
    };
}
