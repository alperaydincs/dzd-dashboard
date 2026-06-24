using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Exceptions;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using DZDDashboard.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;

public class LookupService(AppDbContext context) : ILookupService
{
    public Task<List<LookupValueDto>> GetAsync(string category, CancellationToken cancellationToken = default)
    {
        EnsureValidCategory(category);
        return category switch
        {
            LookupCategories.AdditionalPaymentType => ReadAsync(context.AdditionalPaymentTypes, category, cancellationToken),
            LookupCategories.DeductionType         => ReadAsync(context.DeductionTypes, category, cancellationToken),
            LookupCategories.ContractType          => ReadAsync(context.ContractTypes, category, cancellationToken),
            LookupCategories.WorkModel             => ReadAsync(context.WorkModels, category, cancellationToken),
            LookupCategories.EducationLevel        => ReadAsync(context.EducationLevels, category, cancellationToken),
            _                                      => ReadAsync(context.DependentTypes, category, cancellationToken),
        };
    }

    public Task<LookupValueDto> CreateAsync(LookupValueDto dto, CancellationToken cancellationToken = default)
    {
        EnsureValidCategory(dto.Category);
        return dto.Category switch
        {
            LookupCategories.AdditionalPaymentType => CreateAsync(context.AdditionalPaymentTypes, dto, cancellationToken),
            LookupCategories.DeductionType         => CreateAsync(context.DeductionTypes, dto, cancellationToken),
            LookupCategories.ContractType          => CreateAsync(context.ContractTypes, dto, cancellationToken),
            LookupCategories.WorkModel             => CreateAsync(context.WorkModels, dto, cancellationToken),
            LookupCategories.EducationLevel        => CreateAsync(context.EducationLevels, dto, cancellationToken),
            _                                      => CreateAsync(context.DependentTypes, dto, cancellationToken),
        };
    }

    public Task UpdateAsync(int id, LookupValueDto dto, CancellationToken cancellationToken = default)
    {
        EnsureValidCategory(dto.Category);
        return dto.Category switch
        {
            LookupCategories.AdditionalPaymentType => UpdateAsync(context.AdditionalPaymentTypes, id, dto, cancellationToken),
            LookupCategories.DeductionType         => UpdateAsync(context.DeductionTypes, id, dto, cancellationToken),
            LookupCategories.ContractType          => UpdateAsync(context.ContractTypes, id, dto, cancellationToken),
            LookupCategories.WorkModel             => UpdateAsync(context.WorkModels, id, dto, cancellationToken),
            LookupCategories.EducationLevel        => UpdateAsync(context.EducationLevels, id, dto, cancellationToken),
            _                                      => UpdateAsync(context.DependentTypes, id, dto, cancellationToken),
        };
    }

    public Task DeleteAsync(string category, int id, CancellationToken cancellationToken = default)
    {
        EnsureValidCategory(category);
        return category switch
        {
            LookupCategories.AdditionalPaymentType => RequireRemoveAsync(context.AdditionalPaymentTypes, id, cancellationToken),
            LookupCategories.DeductionType         => RequireRemoveAsync(context.DeductionTypes, id, cancellationToken),
            LookupCategories.ContractType          => RequireRemoveAsync(context.ContractTypes, id, cancellationToken),
            LookupCategories.WorkModel             => RequireRemoveAsync(context.WorkModels, id, cancellationToken),
            LookupCategories.EducationLevel        => RequireRemoveAsync(context.EducationLevels, id, cancellationToken),
            _                                      => RequireRemoveAsync(context.DependentTypes, id, cancellationToken),
        };
    }

    private async Task RequireRemoveAsync<T>(DbSet<T> set, int id, CancellationToken ct) where T : NamedTypeEntity
    {
        if (!await RemoveAsync(set, id, ct))
            throw new EntityNotFoundException(typeof(T).Name, id);
    }

    private static async Task<List<LookupValueDto>> ReadAsync<T>(DbSet<T> set, string category, CancellationToken ct)
        where T : NamedTypeEntity
    {
        var rows = await set.AsNoTracking().OrderBy(x => x.Name).ToListAsync(ct);
        return [.. rows.Select(x => new LookupValueDto { Id = x.Id, Category = category, Value = x.Name, Sequence = x.Id })];
    }

    private async Task<LookupValueDto> CreateAsync<T>(DbSet<T> set, LookupValueDto dto, CancellationToken ct)
        where T : NamedTypeEntity, new()
    {
        var name = dto.Value.Trim();
        if (string.IsNullOrWhiteSpace(name)) throw new DomainValidationException("Değer boş olamaz.");
        if (await set.AnyAsync(x => x.Name == name, ct)) throw new DomainConflictException("Bu değer zaten var.");

        var entity = new T { Name = name };
        set.Add(entity);
        await context.SaveChangesAsync(ct);
        return new LookupValueDto { Id = entity.Id, Category = dto.Category, Value = entity.Name, Sequence = entity.Id };
    }

    private async Task UpdateAsync<T>(DbSet<T> set, int id, LookupValueDto dto, CancellationToken ct)
        where T : NamedTypeEntity
    {
        var name = dto.Value.Trim();
        if (string.IsNullOrWhiteSpace(name)) throw new DomainValidationException("Değer boş olamaz.");
        var entity = await set.FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new EntityNotFoundException(typeof(T).Name, id);
        if (await set.AnyAsync(x => x.Name == name && x.Id != id, ct)) throw new DomainConflictException("Bu değer zaten var.");
        entity.Name = name;
        await context.SaveChangesAsync(ct);
    }

    private async Task<bool> RemoveAsync<T>(DbSet<T> set, int id, CancellationToken ct)
        where T : NamedTypeEntity
    {
        var entity = await set.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return false;
        set.Remove(entity);
        await context.SaveChangesAsync(ct);
        return true;
    }

    private static void EnsureValidCategory(string category)
    {
        if (!LookupCategories.All.Contains(category))
            throw new DomainValidationException("Geçersiz kategori.");
    }
}
