using MapsterMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DZDDashboard.Common.Exceptions;
using DZDDashboard.Common.Utils;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DZDDashboard.Services;

/// <summary>
/// User aggregate service. The implementation is split across partial files by
/// responsibility — <c>UserService.Read.cs</c> (queries / projections),
/// <c>UserService.Write.cs</c> (mutations &amp; org-chart wiring) and
/// <c>UserService.Sync.cs</c> (Entra provisioning). This file holds the
/// constructor and the helpers shared between them.
/// </summary>
public partial class UserService(
    IMapper mapper,
    AppDbContext context,
    IFileStorageService fileStorage,
    IReportsToCalculator reportsToCalculator,
    IOptions<OnboardingOptions> onboardingOptions,
    ILogger<UserService> logger)
    : IUserService, IUserReadService, IUserWriteService, IUserSyncService
{
    public async Task<string> GenerateUniqueSlugAsync(string? email, string? firstName, string? lastName, CancellationToken cancellationToken = default)
    {
        var baseSlug = string.IsNullOrWhiteSpace(email)
            ? SlugGenerator.FromName(firstName, lastName)
            : SlugGenerator.FromEmail(email);
        var slug = baseSlug;
        var suffix = 2;
        while (await context.Users.AnyAsync(u => u.Slug == slug, cancellationToken))
            slug = $"{baseSlug}-{suffix++}";
        return slug;
    }

    private async Task<User> RequireUserAsync(int userId, CancellationToken cancellationToken = default)
        => await context.Users.FindAsync([userId], cancellationToken)
           ?? throw new EntityNotFoundException("User", userId);

    private async Task<User> RequireUserWithAsync(int userId,
        Expression<Func<User, object?>> include,
        CancellationToken cancellationToken = default)
        => await context.Users
               .Include(include)
               .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
           ?? throw new EntityNotFoundException("User", userId);

    /// <summary>
    /// Throws <see cref="EntityNotFoundException"/> when an optional foreign-key
    /// id is supplied but no matching row exists. Centralizes the FK-existence
    /// guard that the write methods would otherwise repeat per relation.
    /// </summary>
    private async Task EnsureExistsAsync<TEntity>(int? id, CancellationToken cancellationToken)
        where TEntity : class
    {
        if (id.HasValue &&
            !await context.Set<TEntity>().AnyAsync(e => EF.Property<int>(e, "Id") == id.Value, cancellationToken))
            throw new EntityNotFoundException(typeof(TEntity).Name, id.Value);
    }

    private void MergeCollection<TEntity, TDto>(
        ICollection<TEntity> existing,
        IEnumerable<TDto>    incoming,
        Func<TDto,    bool>    isValid,
        Func<TDto,    int>     getDtoId,
        Func<TEntity, int>     getEntityId,
        Action<TEntity>        removeEntity,
        Func<TDto, TEntity>    createEntity)
        where TEntity : class
    {
        var valid   = incoming.Where(isValid).ToList();
        var keepIds = valid.Select(getDtoId).Where(id => id > 0).ToHashSet();

        foreach (var entity in existing.Where(e => !keepIds.Contains(getEntityId(e))).ToList())
            removeEntity(entity);

        foreach (var dto in valid)
        {
            var id     = getDtoId(dto);
            var match  = id > 0 ? existing.FirstOrDefault(e => getEntityId(e) == id) : null;

            if (match != null) mapper.Map(dto, match);
            else               existing.Add(createEntity(dto));
        }
    }
}
