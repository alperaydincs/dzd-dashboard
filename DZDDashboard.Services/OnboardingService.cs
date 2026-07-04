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
    ChecklistEngine engine,
    IFileStorageService fileStorage,
    IUserDocumentService documents) : IOnboardingService
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
                Status         = p.Status,
                CompletedCount = p.Items.Count(i => i.Status == ChecklistItemStatuses.Completed),
                TotalCount     = p.Items.Count
            })
            .ToListAsync(cancellationToken);

    public async Task<OnboardingProcessDto> GetAsync(int processId, CancellationToken cancellationToken = default)
        => Map(await LoadAsync(processId, cancellationToken));

    public async Task<OnboardingProcessDto> StartAsync(StartOnboardingDto dto, CancellationToken cancellationToken = default)
    {
        var user = new User
        {
            FirstName       = dto.FirstName.Trim(),
            LastName        = dto.LastName.Trim(),
            Email           = dto.Email!.Trim(),
            NormalizedEmail = dto.Email.Trim().ToUpperInvariant(),
            PersonalEmail   = dto.PersonalEmail?.Trim(),
            PhoneNumber     = dto.PhoneNumber?.Trim(),
            Slug            = await GenerateUniqueSlugAsync(dto.Email, cancellationToken),
            LifecycleStatus = UserLifecycleStatuses.Onboarding,
            IsActive        = false
        };
        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        var process = new OnboardingProcess
        {
            UserId    = user.Id,
            StartDate = dto.StartDate,
            ManagerId = dto.ManagerId,
            Status    = ProcessStatuses.InProgress,
            Items     = await engine.BuildItemsAsync(TemplateProcessTypes.Onboarding, cancellationToken)
        };
        context.OnboardingProcesses.Add(process);
        await context.SaveChangesAsync(cancellationToken);

        return await GetAsync(process.Id, cancellationToken);
    }

    public async Task<OnboardingProcessDto> CompleteItemAsync(int processId, int itemId, CompleteChecklistItemDto dto, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        var item = RequireItem(process, itemId);
        await engine.CompleteAsync(item, process.Items, process.UserId, process.StartDate, dto, cancellationToken);
        await RecomputeAsync(process, cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OnboardingProcessDto> SkipItemAsync(int processId, int itemId, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.SkipAsync(RequireItem(process, itemId), cancellationToken);
        await RecomputeAsync(process, cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OnboardingProcessDto> ReopenItemAsync(int processId, int itemId, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.ReopenAsync(RequireItem(process, itemId), process.UserId, cancellationToken);
        await RecomputeAsync(process, cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OnboardingProcessDto> UpdateItemNoteAsync(int processId, int itemId, UpdateChecklistNoteDto dto, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.UpdateNoteAsync(RequireItem(process, itemId), dto.Note, cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OnboardingProcessDto> UploadDocumentAsync(int processId, int itemId, string fileName, string contentType, byte[] content, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.UploadDocumentAsync(RequireItem(process, itemId), fileName, contentType, content, cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OnboardingProcessDto> DeleteDocumentAsync(int processId, int itemId, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.DeleteDocumentAsync(RequireItem(process, itemId), cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<(byte[] Content, string? ContentType, string FileName)?> GetDocumentAsync(int processId, int itemId, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        return await engine.GetDocumentAsync(RequireItem(process, itemId), cancellationToken);
    }

    private async Task<OnboardingProcess> LoadAsync(int processId, CancellationToken cancellationToken)
        => await context.OnboardingProcesses
            .Include(p => p.User)
            .Include(p => p.Manager)
            .Include(p => p.Items).ThenInclude(i => i.Dependents)
            .Include(p => p.Items).ThenInclude(i => i.CompletedBy)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == processId, cancellationToken)
           ?? throw new EntityNotFoundException(nameof(OnboardingProcess), processId);

    private static ChecklistItem RequireItem(OnboardingProcess process, int itemId)
        => process.Items.FirstOrDefault(i => i.Id == itemId)
           ?? throw new EntityNotFoundException(nameof(ChecklistItem), itemId);

    private async Task RecomputeAsync(OnboardingProcess process, CancellationToken cancellationToken)
    {
        var allRequiredDone = AllRequiredDone(process);

        if (!allRequiredDone && process.Status == ProcessStatuses.Completed)
        {
            process.Status      = ProcessStatuses.InProgress;
            process.CompletedAt = null;
            if (process.User is { } u)
            {
                u.LifecycleStatus = UserLifecycleStatuses.Onboarding;
                u.IsActive        = false;
            }
            await context.SaveChangesAsync(cancellationToken);
        }
    }

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
        await context.SaveChangesAsync(cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task CancelAsync(int processId, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);

        foreach (var item in process.Items.Where(i => i.DocumentStoredFileId.HasValue))
            await fileStorage.DeleteAsync(item.DocumentStoredFileId!.Value, cancellationToken);

        var userDocs = await context.UserDocuments
            .Where(d => d.UserId == process.UserId)
            .Select(d => d.Id)
            .ToListAsync(cancellationToken);
        foreach (var docId in userDocs)
            await documents.DeleteAsync(process.UserId, docId, cancellationToken);

        process.Status      = ProcessStatuses.Cancelled;
        process.CompletedAt = null;
        if (process.User is { } u)
        {
            u.LifecycleStatus = UserLifecycleStatuses.Active;
            u.IsActive        = false;
        }
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
            process = new OnboardingProcess
            {
                UserId    = userId,
                StartDate = DateTime.UtcNow.Date,
                Status    = ProcessStatuses.InProgress,
                Items     = await engine.BuildItemsAsync(TemplateProcessTypes.Onboarding, cancellationToken)
            };
            context.OnboardingProcesses.Add(process);
            await context.SaveChangesAsync(cancellationToken);
        }

        return new MyOnboardingStateDto { LifecycleStatus = user.LifecycleStatus, ProcessId = process.Id };
    }

    private async Task<string> GenerateUniqueSlugAsync(string email, CancellationToken cancellationToken)
    {
        var baseSlug = SlugGenerator.FromEmail(email);
        var slug = baseSlug;
        var suffix = 2;
        while (await context.Users.AnyAsync(u => u.Slug == slug, cancellationToken))
            slug = $"{baseSlug}-{suffix++}";
        return slug;
    }

    private static OnboardingProcessDto Map(OnboardingProcess p)
    {
        var ordered = p.Items.OrderBy(i => i.Sequence).ToList();
        return new OnboardingProcessDto
        {
            Id           = p.Id,
            UserId       = p.UserId,
            EmployeeName = p.User != null ? AppFormatter.BuildFullName(p.User.FirstName, p.User.LastName) : null,
            EmployeeSlug = p.User?.Slug,
            StartDate    = p.StartDate,
            ManagerId    = p.ManagerId,
            ManagerName  = p.Manager != null ? AppFormatter.BuildFullName(p.Manager.FirstName, p.Manager.LastName) : null,
            Status       = p.Status,
            CompletedAt  = p.CompletedAt,
            CanComplete  = p.Status == ProcessStatuses.InProgress && AllRequiredDone(p),
            Items        = [.. ordered.Select(i => ChecklistEngine.Map(i, ordered))]
        };
    }
}
