using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Exceptions;
using DZDDashboard.Common.Utils;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using DZDDashboard.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;

public class ChecklistTemplateService(AppDbContext context) : IChecklistTemplateService
{
    public async Task<List<ChecklistStepTemplateDto>> GetAsync(string processType, CancellationToken cancellationToken = default)
    {
        EnsureValidProcessType(processType);
        var rows = await context.ChecklistStepTemplates.AsNoTracking()
            .Where(t => t.ProcessType == processType)
            .OrderBy(t => t.Sequence)
            .ToListAsync(cancellationToken);
        return [.. rows.Select(Map)];
    }

    public async Task<ChecklistStepTemplateDto> CreateAsync(ChecklistStepTemplateDto dto, CancellationToken cancellationToken = default)
    {
        EnsureValidProcessType(dto.ProcessType);
        EnsureValidBenefitKind(dto.BenefitKind);

        var stepKey = string.IsNullOrWhiteSpace(dto.StepKey) ? SlugGenerator.Slugify(dto.Title) : dto.StepKey.Trim();
        if (await context.ChecklistStepTemplates.AnyAsync(t => t.ProcessType == dto.ProcessType && t.StepKey == stepKey, cancellationToken))
            throw new DomainConflictException("Bu süreç tipinde aynı anahtara sahip bir adım zaten var.");

        var entity = new ChecklistStepTemplate
        {
            ProcessType      = dto.ProcessType,
            StepKey          = stepKey,
            Title            = dto.Title.Trim(),
            Sequence         = dto.Sequence,
            IsRequired       = dto.IsRequired,
            IsGate           = dto.IsGate,
            RequiresDocument = dto.RequiresDocument,
            IsEnabled        = dto.IsEnabled,
            BenefitKind      = dto.BenefitKind
        };
        context.ChecklistStepTemplates.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    public async Task UpdateAsync(int id, ChecklistStepTemplateDto dto, CancellationToken cancellationToken = default)
    {
        EnsureValidBenefitKind(dto.BenefitKind);
        var entity = await context.ChecklistStepTemplates.FindRequiredAsync(id, nameof(ChecklistStepTemplate), cancellationToken);

        entity.Title            = dto.Title.Trim();
        entity.Sequence         = dto.Sequence;
        entity.IsRequired       = dto.IsRequired;
        entity.IsGate           = dto.IsGate;
        entity.RequiresDocument = dto.RequiresDocument;
        entity.IsEnabled        = dto.IsEnabled;
        entity.BenefitKind      = dto.BenefitKind;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await context.ChecklistStepTemplates.FindRequiredAsync(id, nameof(ChecklistStepTemplate), cancellationToken);
        context.ChecklistStepTemplates.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static void EnsureValidProcessType(string processType)
    {
        if (!TemplateProcessTypes.All.Contains(processType))
            throw new DomainValidationException("Geçersiz süreç tipi.");
    }

    private static void EnsureValidBenefitKind(string benefitKind)
    {
        if (benefitKind is not (ChecklistBenefitKinds.None or ChecklistBenefitKinds.Bes or ChecklistBenefitKinds.Oss))
            throw new DomainValidationException("Geçersiz yan hak tipi.");
    }

    private static ChecklistStepTemplateDto Map(ChecklistStepTemplate t) => new()
    {
        Id          = t.Id,
        ProcessType = t.ProcessType,
        StepKey     = t.StepKey,
        Title       = t.Title,
        Sequence    = t.Sequence,
        IsRequired  = t.IsRequired,
        IsGate      = t.IsGate,
        RequiresDocument = t.RequiresDocument,
        IsEnabled   = t.IsEnabled,
        BenefitKind = t.BenefitKind
    };
}
