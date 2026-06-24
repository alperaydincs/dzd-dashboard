using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Exceptions;
using DZDDashboard.Common.Utils;
using DZDDashboard.Common.Validation;
using DZDDashboard.Data;
using DZDDashboard.Data.Abstractions;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;

public class ChecklistEngine(
    AppDbContext context,
    IAuditProvider audit,
    IFileStorageService fileStorage,
    IPaymentService paymentService)
{
    public async Task<List<ChecklistItem>> BuildItemsAsync(string processType, CancellationToken cancellationToken)
    {
        var templates = await context.ChecklistStepTemplates.AsNoTracking()
            .Where(t => t.ProcessType == processType && t.IsEnabled)
            .OrderBy(t => t.Sequence)
            .ToListAsync(cancellationToken);

        IEnumerable<ChecklistStepDefinition> source = templates.Count > 0
            ? templates.Select(t => new ChecklistStepDefinition(t.StepKey, t.Title, t.Sequence, t.IsRequired, t.IsGate, t.BenefitKind, t.RequiresDocument))
            : FallbackCatalog(processType);

        return [.. source.Select(s => new ChecklistItem
        {
            StepKey          = s.Key,
            Title            = s.Title,
            Sequence         = s.Sequence,
            IsRequired       = s.IsRequired,
            IsGate           = s.IsGate,
            RequiresDocument = s.RequiresDocument,
            BenefitKind      = s.BenefitKind,
            Status           = ChecklistItemStatuses.Pending
        })];
    }

    private static IReadOnlyList<ChecklistStepDefinition> FallbackCatalog(string processType) => processType switch
    {
        TemplateProcessTypes.OffboardingResignation => OffboardingStepCatalog.ResignationSteps,
        TemplateProcessTypes.OffboardingTermination => OffboardingStepCatalog.TerminationSteps,
        _                                           => OnboardingStepCatalog.Steps
    };

    public async Task CompleteAsync(ChecklistItem item, IReadOnlyList<ChecklistItem> siblings, int userId, DateTime processStartDate, CompleteChecklistItemDto dto, CancellationToken cancellationToken)
    {
        if (item.Status == ChecklistItemStatuses.Completed) return;

        EnsureGateUnblocked(item, siblings);

        if (item.RequiresDocument && item.DocumentStoredFileId is null)
            throw new DomainValidationException("Bu adım tamamlanmadan önce bir belge yüklenmelidir.");

        if (item.BenefitKind != ChecklistBenefitKinds.None)
            ApplyBenefitInput(item, dto);

        if (dto.Note is not null) item.Note = dto.Note;

        item.Status        = ChecklistItemStatuses.Completed;
        item.CompletedAt   = audit.GetNow();
        item.CompletedById = audit.GetCurrentUserId();

        await context.SaveChangesAsync(cancellationToken);

        if (item.BenefitKind != ChecklistBenefitKinds.None && item.ReflectedBenefitRecordId is null)
        {
            var created = await paymentService.CreateBenefitRecordAsync(userId, BuildBenefitDto(item, processStartDate), cancellationToken);
            item.ReflectedBenefitRecordId = created.Id;
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task SkipAsync(ChecklistItem item, CancellationToken cancellationToken)
    {
        if (item.IsRequired)
            throw new DomainValidationException("Zorunlu bir adım atlanamaz.");

        item.Status        = ChecklistItemStatuses.Skipped;
        item.CompletedAt   = null;
        item.CompletedById = null;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task ReopenAsync(ChecklistItem item, int userId, CancellationToken cancellationToken)
    {
        if (item.ReflectedBenefitRecordId is { } benefitId)
        {
            await paymentService.DeleteBenefitRecordAsync(userId, benefitId, cancellationToken);
            item.ReflectedBenefitRecordId = null;
        }

        item.Status        = ChecklistItemStatuses.Pending;
        item.CompletedAt   = null;
        item.CompletedById = null;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateNoteAsync(ChecklistItem item, string? note, CancellationToken cancellationToken)
    {
        item.Note = note;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UploadDocumentAsync(ChecklistItem item, string fileName, string contentType, byte[] content, CancellationToken cancellationToken)
    {
        if (item.DocumentStoredFileId is { } existing)
            await fileStorage.DeleteAsync(existing, cancellationToken);

        var storageId = await fileStorage.SaveAsync(content, contentType, cancellationToken);
        item.DocumentStoredFileId = storageId;
        item.DocumentFileName     = fileName;
        item.DocumentContentType  = contentType;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteDocumentAsync(ChecklistItem item, CancellationToken cancellationToken)
    {
        if (item.DocumentStoredFileId is not { } storageId) return;
        await fileStorage.DeleteAsync(storageId, cancellationToken);
        item.DocumentStoredFileId = null;
        item.DocumentFileName     = null;
        item.DocumentContentType  = null;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<(byte[] Content, string? ContentType, string FileName)?> GetDocumentAsync(ChecklistItem item, CancellationToken cancellationToken)
    {
        if (item.DocumentStoredFileId is not { } storageId) return null;
        var file = await fileStorage.GetAsync(storageId, cancellationToken);
        if (file is null) return null;
        return (file.Value.Content, file.Value.ContentType ?? item.DocumentContentType, item.DocumentFileName ?? "document");
    }

    private static void EnsureGateUnblocked(ChecklistItem item, IReadOnlyList<ChecklistItem> siblings)
    {
        if (item.StepKey != OffboardingStepCatalog.PaymentDoneKey) return;

        var blocking = siblings.Any(s => s.Id != item.Id && s.IsRequired && s.Status != ChecklistItemStatuses.Completed);
        if (blocking)
            throw new DomainConflictException("Tüm işlemler tamamlanmadan ve zimmet teslim alınmadan ödeme adımı tamamlanamaz.");
    }

    private void ApplyBenefitInput(ChecklistItem item, CompleteChecklistItemDto dto)
    {
        item.ProviderName   = dto.ProviderName;
        item.Currency       = string.IsNullOrWhiteSpace(dto.Currency) ? Currencies.Try : dto.Currency;
        item.EmployeeAmount = dto.EmployeeAmount;
        item.EmployerAmount = dto.EmployerAmount;

        if (item.BenefitKind == ChecklistBenefitKinds.Bes)
        {
            if (dto.EmployeeAmount is null && dto.EmployerAmount is null)
                throw new DomainValidationException("BES adımı için işveren ve/veya çalışan tutarı girilmelidir.");
        }
        else if (item.BenefitKind == ChecklistBenefitKinds.Oss)
        {
            if (dto.EmployeeAmount is null)
                throw new DomainValidationException("ÖSS adımı için çalışan tutarı girilmelidir.");
            if (dto.Dependents.Count > ValidationConstants.MaxBenefitDependents)
                throw new DomainValidationException($"En fazla {ValidationConstants.MaxBenefitDependents} bağımlı eklenebilir.");
        }

        if (item.Dependents.Count > 0)
            context.ChecklistItemDependents.RemoveRange(item.Dependents);

        item.Dependents = [.. dto.Dependents.Select((d, index) => new ChecklistItemDependent
        {
            ChecklistItemId = item.Id,
            Order           = index + 1,
            DependentType   = d.DependentType,
            DependentName   = d.DependentName,
            Amount          = d.Amount
        })];
    }

    private static BenefitRecordDto BuildBenefitDto(ChecklistItem item, DateTime startDate)
    {
        if (item.BenefitKind == ChecklistBenefitKinds.Bes)
        {
            return new BenefitRecordDto
            {
                BenefitType  = BenefitTypes.PrivatePension,
                Payer        = BenefitPayers.Employer,
                BenefitName  = "BES",
                Amount       = (item.EmployerAmount ?? 0m) + (item.EmployeeAmount ?? 0m),
                Currency     = item.Currency ?? Currencies.Try,
                Period       = PaymentPeriods.Monthly,
                StartDate    = startDate,
                Source       = PaymentSources.Onboarding,
                ProviderName = item.ProviderName,
                EmployeeContributionAmount = item.EmployeeAmount,
                EmployerContributionAmount = item.EmployerAmount
            };
        }

        return new BenefitRecordDto
        {
            BenefitType  = BenefitTypes.PrivateHealthInsurance,
            Payer        = BenefitPayers.Employer,
            BenefitName  = "ÖSS",
            Amount       = (item.EmployeeAmount ?? 0m) + item.Dependents.Sum(d => d.Amount),
            Currency     = item.Currency ?? Currencies.Try,
            Period       = PaymentPeriods.Monthly,
            StartDate    = startDate,
            Source       = PaymentSources.Onboarding,
            ProviderName = item.ProviderName,
            Dependents   = [.. item.Dependents.OrderBy(d => d.Order).Select((d, index) => new BenefitDependentDto
            {
                Order         = index + 1,
                DependentType = d.DependentType,
                DependentName = d.DependentName,
                Amount        = d.Amount,
                StartDate     = startDate
            })]
        };
    }

    public static ChecklistItemDto Map(ChecklistItem item, IReadOnlyList<ChecklistItem> siblings)
    {
        var gateBlocked = item.StepKey == OffboardingStepCatalog.PaymentDoneKey
            && siblings.Any(s => s.Id != item.Id && s.IsRequired && s.Status != ChecklistItemStatuses.Completed);

        return new ChecklistItemDto
        {
            Id          = item.Id,
            StepKey     = item.StepKey,
            Title       = item.Title,
            Sequence    = item.Sequence,
            IsRequired  = item.IsRequired,
            IsGate      = item.IsGate,
            RequiresDocument = item.RequiresDocument,
            BenefitKind = item.BenefitKind,
            Status      = item.Status,
            Note        = item.Note,
            CompletedAt = item.CompletedAt,
            CompletedByName = item.CompletedBy != null
                ? AppFormatter.BuildFullName(item.CompletedBy.FirstName, item.CompletedBy.LastName)
                : null,
            HasDocument      = item.DocumentStoredFileId.HasValue,
            DocumentFileName = item.DocumentFileName,
            ProviderName     = item.ProviderName,
            Currency         = item.Currency,
            EmployeeAmount   = item.EmployeeAmount,
            EmployerAmount   = item.EmployerAmount,
            Dependents       = [.. item.Dependents.OrderBy(d => d.Order).Select(d => new ChecklistItemDependentInputDto
            {
                Order = d.Order, DependentType = d.DependentType, DependentName = d.DependentName, Amount = d.Amount
            })],
            IsActionable  = item.Status == ChecklistItemStatuses.Pending && !gateBlocked,
            BlockedReason = gateBlocked ? "Tüm işlemler ve zimmet iadesi tamamlanmadan bu adım aktifleşmez." : null
        };
    }
}
