using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;

internal static class ReportsToCalculator
{
    /// <summary>
    /// Tüm kullanıcıların ReportsToId değerini organizasyon pozisyon hiyerarşisine göre yeniden hesaplar.
    /// Her kullanıcı, pozisyon ağacındaki en yakın üst pozisyondaki kullanıcıya raporlanır.
    /// </summary>
    public static async Task RecalculateAsync(AppDbContext context)
    {
        var positions = await context.OrganizationPositions
            .AsNoTracking()
            .Select(p => new { p.Id, p.ParentId })
            .ToListAsync();

        var parentByPositionId = positions.ToDictionary(x => x.Id, x => x.ParentId);

        var allUsers = await context.Users.ToListAsync();

        var positionedUsers = allUsers
            .Where(u => u.OrganizationPositionId.HasValue)
            .ToList();

        var usersByPosition = positionedUsers
            .GroupBy(u => u.OrganizationPositionId!.Value)
            .ToDictionary(g => g.Key, g => g.OrderBy(u => u.Id).ToList());

        foreach (var user in positionedUsers)
        {
            user.ReportsToId = FindNearestAncestorManagerId(
                user.OrganizationPositionId!.Value,
                user.Id,
                parentByPositionId,
                usersByPosition);
        }

        foreach (var user in allUsers.Where(u => !u.OrganizationPositionId.HasValue && u.ReportsToId != null))
        {
            user.ReportsToId = null;
        }
    }

    private static int? FindNearestAncestorManagerId(
        int positionId,
        int currentUserId,
        IReadOnlyDictionary<int, int?> parentByPositionId,
        IReadOnlyDictionary<int, List<User>> usersByPosition)
    {
        if (!parentByPositionId.TryGetValue(positionId, out var parentId))
            return null;

        while (parentId.HasValue)
        {
            if (usersByPosition.TryGetValue(parentId.Value, out var managers))
            {
                var manager = managers.FirstOrDefault(u => u.Id != currentUserId);
                if (manager != null)
                    return manager.Id;
            }

            if (!parentByPositionId.TryGetValue(parentId.Value, out parentId))
                break;
        }

        return null;
    }
}
