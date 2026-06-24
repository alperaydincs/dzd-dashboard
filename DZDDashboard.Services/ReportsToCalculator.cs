using DZDDashboard.Data;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;

public class ReportsToCalculator(AppDbContext context) : IReportsToCalculator
{
    public async Task RecalculateAsync(CancellationToken cancellationToken = default)
    {
        var parentByPositionId = await context.OrganizationPositions
            .AsNoTracking()
            .Select(p => new { p.Id, p.ParentId })
            .ToDictionaryAsync(p => p.Id, p => p.ParentId, cancellationToken);

        var positionedUsers = await context.Users
            .Where(u => u.OrganizationPositionId.HasValue)
            .Select(u => new { u.Id, u.OrganizationPositionId })
            .ToListAsync(cancellationToken);

        var userIdsByPosition = positionedUsers
            .GroupBy(u => u.OrganizationPositionId!.Value)
            .ToDictionary(g => g.Key, g => g.OrderBy(u => u.Id).Select(u => u.Id).ToList());


        var updates = positionedUsers
            .Select(u => new
            {
                u.Id,
                NewReportsToId = FindNearestManagerId(
                    u.OrganizationPositionId!.Value, u.Id,
                    parentByPositionId, userIdsByPosition)
            })
            .ToList();

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
