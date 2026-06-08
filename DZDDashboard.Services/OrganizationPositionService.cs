using AutoMapper;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Exceptions;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using DZDDashboard.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;

// Interface is in Abstractions/IOrganizationPositionService.cs

public class OrganizationPositionService(
    AppDbContext context,
    IMapper mapper,
    IReportsToCalculator reportsToCalculator) : IOrganizationPositionService
{
    public async Task<List<OrganizationPositionDto>> GetAllPositionsAsync(CancellationToken cancellationToken = default)
    {
        var positions = await PositionsWithDetails()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        var dtos = mapper.Map<List<OrganizationPositionDto>>(positions);
        ComputeLevels(dtos);
        return dtos;
    }

    public async Task<OrganizationPositionDto> CreatePositionAsync(CreateOrganizationPositionDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.ParentId.HasValue)
        {
            var parentExists = await context.OrganizationPositions.AnyAsync(p => p.Id == dto.ParentId.Value, cancellationToken);
            if (!parentExists) throw new EntityNotFoundException("OrganizationPosition (parent)", dto.ParentId.Value);
        }

        var entity = mapper.Map<OrganizationPosition>(dto);
        context.OrganizationPositions.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(entity.Id, cancellationToken);
    }

    public async Task UpdatePositionAsync(UpdateOrganizationPositionDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await context.OrganizationPositions
            .FindRequiredAsync(dto.Id, nameof(OrganizationPosition), cancellationToken);

        if (dto.ParentId.HasValue)
        {
            if (dto.ParentId == dto.Id)
                throw new DomainValidationException("A position cannot be its own parent.");

            var parentExists = await context.OrganizationPositions.AnyAsync(p => p.Id == dto.ParentId.Value, cancellationToken);
            if (!parentExists)
                throw new EntityNotFoundException("OrganizationPosition (parent)", dto.ParentId.Value);

            if (await IsDescendantAsync(dto.Id, dto.ParentId.Value))
                throw new DomainConflictException("Cannot set a descendant as parent — circular dependency detected.");
        }

        // Transaction ensures the position change, user assignment, and org-chart recalculation
        // are atomic. If RecalculateAsync fails, the entire update is rolled back.
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        mapper.Map(dto, entity);
        await AssignPositionUserAsync(dto.Id, dto.UserId, cancellationToken);
        // Flush position + user changes to DB first so RecalculateAsync reads the updated state.
        await context.SaveChangesAsync(cancellationToken);
        await reportsToCalculator.RecalculateAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    public async Task DeletePositionAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await context.OrganizationPositions
            .Include(x => x.Children)
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new EntityNotFoundException("OrganizationPosition", id);

        if (entity.Children.Count > 0)
            throw new DomainConflictException("Cannot delete a position that has child positions.");

        if (entity.Users.Count > 0)
            throw new DomainConflictException("Cannot delete a position that has assigned users.");

        context.OrganizationPositions.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    // O(n) — loads entire positions table into memory for cycle detection.
    // Acceptable for small org charts (<500 nodes). Replace with a recursive SQL CTE if scale grows.
    private async Task<bool> IsDescendantAsync(int ancestorId, int potentialDescendantId)
    {
        var parentMap = await context.OrganizationPositions
            .AsNoTracking()
            .Select(p => new { p.Id, p.ParentId })
            .ToDictionaryAsync(p => p.Id, p => p.ParentId);

        var currentId = (int?)potentialDescendantId;
        while (currentId.HasValue && parentMap.TryGetValue(currentId.Value, out var parentId))
        {
            if (parentId == ancestorId) return true;
            currentId = parentId;
        }
        return false;
    }

    /// <summary>
    /// Updates the user-to-position assignment.
    /// <para><b>Save-ordering contract:</b> This method does NOT call <c>SaveChangesAsync</c>
    /// or <c>RecalculateAsync</c>. The caller (<see cref="UpdatePositionAsync"/>) must:
    /// <list type="number">
    ///   <item>Call this method (tracks user-assignment changes in the EF change tracker).</item>
    ///   <item>Call <c>SaveChangesAsync</c> to flush all tracked changes to the DB.</item>
    ///   <item>Call <c>RecalculateAsync</c> so the calculator reads the committed state.</item>
    /// </list>
    /// This ordering is required because <c>RecalculateAsync</c> uses <c>AsNoTracking</c>
    /// queries that bypass the EF change tracker and read directly from the DB.</para>
    /// </summary>
    private async Task AssignPositionUserAsync(int positionId, int? userId, CancellationToken cancellationToken = default)
    {
        var currentlyAssigned = await context.Users
            .Where(u => u.OrganizationPositionId == positionId)
            .ToListAsync(cancellationToken);

        foreach (var user in currentlyAssigned)
        {
            if (!userId.HasValue || user.Id != userId.Value)
                user.OrganizationPositionId = null;
        }

        if (userId.HasValue)
        {
            var toAssign = await context.Users.FindRequiredAsync(userId.Value, nameof(User), cancellationToken);
            toAssign.OrganizationPositionId = positionId;
        }
    }

    private async Task<OrganizationPositionDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var position = await PositionsWithDetails()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(OrganizationPosition), id);

        return mapper.Map<OrganizationPositionDto>(position);
    }

    /// <summary>Base query for position reads — single place to add/remove includes.</summary>
    private IQueryable<OrganizationPosition> PositionsWithDetails()
        => context.OrganizationPositions
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Users).ThenInclude(x => x.Job); // OrgChartUserDto needs Job; Avatar excluded (no base64)

    /// <summary>
    /// Computes the depth level for each position DTO using memoized recursion — O(n).
    /// Level 0 = root (no parent), Level 1 = direct child of root, etc.
    /// Cycle guard: once recursion depth exceeds the total node count, the sub-tree is treated as root-level (0).
    /// </summary>
    private static void ComputeLevels(IList<OrganizationPositionDto> dtos)
    {
        var byId   = dtos.ToDictionary(p => p.Id);
        var depths = new Dictionary<int, int>(dtos.Count);

        // Local recursive function with memoisation — each node is resolved at most once.
        int GetDepth(OrganizationPositionDto node, int guard = 0)
        {
            if (guard > dtos.Count) return 0; // cycle detected — stop recursion
            if (depths.TryGetValue(node.Id, out var cached)) return cached;

            var depth = node.ParentId.HasValue && byId.TryGetValue(node.ParentId.Value, out var parent)
                ? 1 + GetDepth(parent, guard + 1)
                : 0;

            depths[node.Id] = depth;
            return depth;
        }

        foreach (var dto in dtos)
            dto.Level = GetDepth(dto);
    }
}
