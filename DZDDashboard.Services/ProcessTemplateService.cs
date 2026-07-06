using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Exceptions;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using DZDDashboard.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;

public class ProcessTemplateService(AppDbContext context) : IProcessTemplateService
{
    public async Task<List<ProcessTemplateDto>> GetAsync(string kind, CancellationToken cancellationToken = default)
    {
        EnsureValidKind(kind);
        var rows = await context.ProcessTemplates.AsNoTracking()
            .Where(t => t.Kind == kind)
            .OrderBy(t => t.Sequence)
            .ToListAsync(cancellationToken);
        return [.. rows.Select(Map)];
    }

    public async Task<ProcessTemplateDto> GetOneAsync(int id, CancellationToken cancellationToken = default)
        => Map(await context.ProcessTemplates.FindRequiredAsync(id, nameof(ProcessTemplate), cancellationToken));

    public async Task<ProcessTemplateDto> CreateAsync(ProcessTemplateDto dto, CancellationToken cancellationToken = default)
    {
        EnsureValidKind(dto.Kind);
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new DomainValidationException("Şablon adı zorunludur.");

        var nextSequence = await context.ProcessTemplates
            .Where(t => t.Kind == dto.Kind)
            .Select(t => (int?)t.Sequence)
            .MaxAsync(cancellationToken) ?? 0;

        var entity = new ProcessTemplate
        {
            Kind     = dto.Kind,
            Name     = dto.Name.Trim(),
            Sequence = nextSequence + 1
        };
        context.ProcessTemplates.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    public async Task UpdateAsync(int id, ProcessTemplateDto dto, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new DomainValidationException("Şablon adı zorunludur.");

        var entity = await context.ProcessTemplates.FindRequiredAsync(id, nameof(ProcessTemplate), cancellationToken);
        entity.Name = dto.Name.Trim();
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await context.ProcessTemplates.FindRequiredAsync(id, nameof(ProcessTemplate), cancellationToken);

        var inUse = entity.Kind == ProcessTemplateKinds.Onboarding
            ? await context.OnboardingProcesses.AnyAsync(p => p.TemplateId == id, cancellationToken)
            : await context.OffboardingProcesses.AnyAsync(p => p.TemplateId == id, cancellationToken);
        if (inUse)
            throw new DomainConflictException("Bu şablonu kullanan işe alım/işten çıkış süreçleri var, silinemez.");

        context.ProcessTemplates.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static void EnsureValidKind(string kind)
    {
        if (!ProcessTemplateKinds.All.Contains(kind))
            throw new DomainValidationException("Geçersiz şablon türü.");
    }

    private static ProcessTemplateDto Map(ProcessTemplate t) => new()
    {
        Id       = t.Id,
        Kind     = t.Kind,
        Name     = t.Name,
        Sequence = t.Sequence
    };
}
