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

public class OffboardingService(AppDbContext context, IAuditProvider audit, ChecklistEngine engine) : IOffboardingService
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
                Type           = p.Type,
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
        if (!OffboardingTypes.All.Contains(dto.Type))
            throw new DomainValidationException("Geçersiz offboarding türü.");

        var user = await context.Users.FindRequiredAsync(dto.UserId, nameof(User), cancellationToken);

        var hasOpen = await context.OffboardingProcesses
            .AnyAsync(p => p.UserId == dto.UserId && p.Status != ProcessStatuses.Cancelled, cancellationToken);
        if (hasOpen)
            throw new DomainConflictException("Bu çalışan için zaten bir offboarding süreci mevcut.");

        user.LifecycleStatus = UserLifecycleStatuses.Offboarding;

        var process = new OffboardingProcess
        {
            UserId   = user.Id,
            Type     = dto.Type,
            ExitDate = dto.ExitDate,
            Status   = ProcessStatuses.InProgress,
            Items    = await engine.BuildItemsAsync(TemplateProcessTypes.ForOffboarding(dto.Type), cancellationToken)
        };
        context.OffboardingProcesses.Add(process);
        await context.SaveChangesAsync(cancellationToken);

        return await GetAsync(process.Id, cancellationToken);
    }

    public async Task<OffboardingProcessDto> CompleteItemAsync(int processId, int itemId, CompleteChecklistItemDto dto, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.CompleteAsync(RequireItem(process, itemId), process.Items, process.UserId, process.ExitDate, dto, cancellationToken);
        await RecomputeAsync(process, cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OffboardingProcessDto> SkipItemAsync(int processId, int itemId, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.SkipAsync(RequireItem(process, itemId), cancellationToken);
        await RecomputeAsync(process, cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OffboardingProcessDto> ReopenItemAsync(int processId, int itemId, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.ReopenAsync(RequireItem(process, itemId), process.UserId, cancellationToken);
        await RecomputeAsync(process, cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OffboardingProcessDto> UpdateItemNoteAsync(int processId, int itemId, UpdateChecklistNoteDto dto, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.UpdateNoteAsync(RequireItem(process, itemId), dto.Note, cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OffboardingProcessDto> UploadDocumentAsync(int processId, int itemId, string fileName, string contentType, byte[] content, CancellationToken cancellationToken = default)
    {
        var process = await LoadAsync(processId, cancellationToken);
        await engine.UploadDocumentAsync(RequireItem(process, itemId), fileName, contentType, content, cancellationToken);
        return await GetAsync(processId, cancellationToken);
    }

    public async Task<OffboardingProcessDto> DeleteDocumentAsync(int processId, int itemId, CancellationToken cancellationToken = default)
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

    private async Task<OffboardingProcess> LoadAsync(int processId, CancellationToken cancellationToken)
        => await context.OffboardingProcesses
            .Include(p => p.User)
            .Include(p => p.Items).ThenInclude(i => i.Dependents).ThenInclude(d => d.DependentTypeRef)
            .Include(p => p.Items).ThenInclude(i => i.CompletedBy)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == processId, cancellationToken)
           ?? throw new EntityNotFoundException(nameof(OffboardingProcess), processId);

    private static ChecklistItem RequireItem(OffboardingProcess process, int itemId)
        => process.Items.FirstOrDefault(i => i.Id == itemId)
           ?? throw new EntityNotFoundException(nameof(ChecklistItem), itemId);

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

    private static OffboardingProcessDto Map(OffboardingProcess p)
    {
        var ordered = p.Items.OrderBy(i => i.Sequence).ToList();
        return new OffboardingProcessDto
        {
            Id           = p.Id,
            UserId       = p.UserId,
            EmployeeName = p.User != null ? AppFormatter.BuildFullName(p.User.FirstName, p.User.LastName) : null,
            EmployeeSlug = p.User?.Slug,
            Type         = p.Type,
            ExitDate     = p.ExitDate,
            Status       = p.Status,
            CompletedAt  = p.CompletedAt,
            Items        = [.. ordered.Select(i => ChecklistEngine.Map(i, ordered))]
        };
    }
}
