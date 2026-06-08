using DZDDashboard.Data;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;

/// <summary>
/// Recalculates ReportsToId for users based on the organisation position hierarchy.
/// Scoped with the calling service's DbContext — no separate tracking context needed.
/// </summary>
public class ReportsToCalculator(AppDbContext context) : IReportsToCalculator
{
    /// <remarks>
    /// <b>Audit note:</b> The <c>ExecuteUpdateAsync</c> calls bypass both the EF change tracker
    /// and <c>AppDbContext.ApplyAuditInfo</c>, so <c>ModifiedAt</c> / <c>ModifiedById</c> are
    /// NOT updated on User rows whose <c>ReportsToId</c> changes. This is intentional — manager
    /// recalculation is a system-derived value, not a user-initiated edit, and stamping every
    /// affected user as "modified by" the current admin would produce misleading audit trails.
    /// </remarks>
    public async Task RecalculateAsync(CancellationToken cancellationToken = default)
    {
        // Load only the position hierarchy (compact projection — no entity tracking needed)
        var parentByPositionId = await context.OrganizationPositions
            .AsNoTracking()
            .Select(p => new { p.Id, p.ParentId })
            .ToDictionaryAsync(p => p.Id, p => p.ParentId, cancellationToken);

        // Load only the two fields needed for the "nearest manager" walk — avoids loading 40+ User columns
        var positionedUsers = await context.Users
            .Where(u => u.OrganizationPositionId.HasValue)
            .Select(u => new { u.Id, u.OrganizationPositionId })
            .ToListAsync(cancellationToken);

        // Index user IDs by position for O(1) lookup during the hierarchy walk
        var userIdsByPosition = positionedUsers
            .GroupBy(u => u.OrganizationPositionId!.Value)
            .ToDictionary(g => g.Key, g => g.OrderBy(u => u.Id).Select(u => u.Id).ToList());

        // Clear stale ReportsToId for unpositioned users in a single batch UPDATE
        await context.Users
            .Where(u => !u.OrganizationPositionId.HasValue && u.ReportsToId != null)
            .ExecuteUpdateAsync(
                s => s.SetProperty(u => u.ReportsToId, (int?)null),
                cancellationToken);

        // Compute new ReportsToId per positioned user
        var updates = positionedUsers
            .Select(u => new
            {
                u.Id,
                NewReportsToId = FindNearestManagerId(
                    u.OrganizationPositionId!.Value, u.Id,
                    parentByPositionId, userIdsByPosition)
            })
            .ToList();

        // Apply updates — group by new manager ID to minimise DB round-trips
        foreach (var group in updates.GroupBy(u => u.NewReportsToId))
        {
            var ids = group.Select(x => x.Id).ToList();
            var newManagerId = group.Key;
            await context.Users
                .Where(u => ids.Contains(u.Id))
                .ExecuteUpdateAsync(
                    s => s.SetProperty(u => u.ReportsToId, newManagerId),
                    cancellationToken);
        }
    }

    /// <summary>
    /// Walks the position hierarchy upward from <paramref name="positionId"/> to find
    /// the first ancestor position that has an assigned user (the "nearest manager").
    /// </summary>
    private static int? FindNearestManagerId(
        int positionId,
        int currentUserId,
        IReadOnlyDictionary<int, int?> parentByPositionId,
        IReadOnlyDictionary<int, List<int>> userIdsByPosition)
    {
        if (!parentByPositionId.TryGetValue(positionId, out var parentId))
            return null;

        while (parentId.HasValue)
        {
            if (userIdsByPosition.TryGetValue(parentId.Value, out var managerIds))
            {
                var managerId = managerIds.FirstOrDefault(id => id != currentUserId);
                if (managerId > 0) return managerId;
            }

            if (!parentByPositionId.TryGetValue(parentId.Value, out parentId))
                break;
        }

        return null;
    }
}
