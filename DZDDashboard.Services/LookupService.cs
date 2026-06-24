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
    public async Task<List<LookupValueDto>> GetAsync(string category, bool includeInactive, CancellationToken cancellationToken = default)
    {
        EnsureValidCategory(category);
        var query = context.LookupValues.AsNoTracking().Where(x => x.Category == category);
        if (!includeInactive) query = query.Where(x => x.IsActive);

        var rows = await query.OrderBy(x => x.Sequence).ThenBy(x => x.Value).ToListAsync(cancellationToken);
        return [.. rows.Select(Map)];
    }

    public async Task<LookupValueDto> CreateAsync(LookupValueDto dto, CancellationToken cancellationToken = default)
    {
        EnsureValidCategory(dto.Category);
        var value = dto.Value.Trim();
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainValidationException("Değer boş olamaz.");
        if (await context.LookupValues.AnyAsync(x => x.Category == dto.Category && x.Value == value, cancellationToken))
            throw new DomainConflictException("Bu kategoride aynı değer zaten var.");

        var entity = new LookupValue
        {
            Category = dto.Category,
            Value    = value,
            Sequence = dto.Sequence,
            IsActive = dto.IsActive
        };
        context.LookupValues.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return Map(entity);
    }

    public async Task UpdateAsync(int id, LookupValueDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await context.LookupValues.FindRequiredAsync(id, nameof(LookupValue), cancellationToken);
        var value = dto.Value.Trim();
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainValidationException("Değer boş olamaz.");
        if (await context.LookupValues.AnyAsync(x => x.Category == entity.Category && x.Value == value && x.Id != id, cancellationToken))
            throw new DomainConflictException("Bu kategoride aynı değer zaten var.");

        entity.Value    = value;
        entity.Sequence = dto.Sequence;
        entity.IsActive = dto.IsActive;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await context.LookupValues.FindRequiredAsync(id, nameof(LookupValue), cancellationToken);
        context.LookupValues.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static void EnsureValidCategory(string category)
    {
        if (!LookupCategories.All.Contains(category))
            throw new DomainValidationException("Geçersiz kategori.");
    }

    private static LookupValueDto Map(LookupValue x) => new()
    {
        Id = x.Id, Category = x.Category, Value = x.Value, Sequence = x.Sequence, IsActive = x.IsActive
    };
}
