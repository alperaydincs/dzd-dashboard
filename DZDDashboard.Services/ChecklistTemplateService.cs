using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Exceptions;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using DZDDashboard.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;

public class ChecklistTemplateService(AppDbContext context) : IChecklistTemplateService
{
    public async Task<List<ChecklistItemTemplateDto>> GetAsync(int processTemplateId, CancellationToken cancellationToken = default)
    {
        var rows = await context.ChecklistStepTemplates.AsNoTracking()
            .Where(t => t.ProcessTemplateId == processTemplateId)
            .OrderBy(t => t.Sequence)
            .ToListAsync(cancellationToken);
        return [.. rows.Select(Map)];
    }

    public async Task<ChecklistItemTemplateDto> CreateAsync(ChecklistItemTemplateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureTemplateExistsAsync(dto.ProcessTemplateId, cancellationToken);

        var entity = new ChecklistStepTemplate
        {
            ProcessTemplateId = dto.ProcessTemplateId,
            Title             = dto.Title.Trim(),
            Sequence          = dto.Sequence,
            IsRequired        = dto.IsRequired
        };
        context.ChecklistStepTemplates.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    public async Task UpdateAsync(int id, ChecklistItemTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await context.ChecklistStepTemplates.FindRequiredAsync(id, nameof(ChecklistStepTemplate), cancellationToken);

        entity.Title      = dto.Title.Trim();
        entity.Sequence   = dto.Sequence;
        entity.IsRequired = dto.IsRequired;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await context.ChecklistStepTemplates.FindRequiredAsync(id, nameof(ChecklistStepTemplate), cancellationToken);
        context.ChecklistStepTemplates.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureTemplateExistsAsync(int processTemplateId, CancellationToken cancellationToken)
    {
        if (!await context.ProcessTemplates.AnyAsync(t => t.Id == processTemplateId, cancellationToken))
            throw new DomainValidationException("Geçersiz süreç şablonu.");
    }

    private static ChecklistItemTemplateDto Map(ChecklistStepTemplate t) => new()
    {
        Id                = t.Id,
        ProcessTemplateId = t.ProcessTemplateId,
        Title             = t.Title,
        Sequence          = t.Sequence,
        IsRequired        = t.IsRequired
    };
}

public class DocumentTemplateService(AppDbContext context) : IDocumentTemplateService
{
    public async Task<List<DocumentTemplateDto>> GetAsync(int processTemplateId, CancellationToken cancellationToken = default)
    {
        var rows = await context.DocumentTemplates.AsNoTracking()
            .Where(t => t.ProcessTemplateId == processTemplateId)
            .OrderBy(t => t.Sequence)
            .ToListAsync(cancellationToken);
        return [.. rows.Select(Map)];
    }

    public async Task<DocumentTemplateDto> CreateAsync(DocumentTemplateDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureTemplateExistsAsync(dto.ProcessTemplateId, cancellationToken);

        var entity = new DocumentTemplate
        {
            ProcessTemplateId = dto.ProcessTemplateId,
            Name              = dto.Name.Trim(),
            Sequence          = dto.Sequence,
            IsRequired        = dto.IsRequired,
            DeadlineDays      = dto.DeadlineDays
        };
        context.DocumentTemplates.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    public async Task UpdateAsync(int id, DocumentTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await context.DocumentTemplates.FindRequiredAsync(id, nameof(DocumentTemplate), cancellationToken);

        entity.Name         = dto.Name.Trim();
        entity.Sequence     = dto.Sequence;
        entity.IsRequired   = dto.IsRequired;
        entity.DeadlineDays = dto.DeadlineDays;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await context.DocumentTemplates.FindRequiredAsync(id, nameof(DocumentTemplate), cancellationToken);
        context.DocumentTemplates.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureTemplateExistsAsync(int processTemplateId, CancellationToken cancellationToken)
    {
        if (!await context.ProcessTemplates.AnyAsync(t => t.Id == processTemplateId, cancellationToken))
            throw new DomainValidationException("Geçersiz süreç şablonu.");
    }

    private static DocumentTemplateDto Map(DocumentTemplate t) => new()
    {
        Id                = t.Id,
        ProcessTemplateId = t.ProcessTemplateId,
        Name              = t.Name,
        Sequence          = t.Sequence,
        IsRequired        = t.IsRequired,
        DeadlineDays      = t.DeadlineDays
    };
}
