using DZDDashboard.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Data.History;

/// <summary>
/// Helpers for reading "who/when last touched this row" back out of the history tables now
/// that live entities no longer carry ModifiedAt/ModifiedById themselves.
/// </summary>
public static class HistoryQueryExtensions
{
    /// <summary>Latest history row for a single source id (its own Insert row if never updated).</summary>
    public static Task<THistory?> GetLatestHistoryAsync<THistory>(this DbContext db, int sourceId, CancellationToken cancellationToken)
        where THistory : class, IHistoryEntity
        => db.Set<THistory>()
            .Where(h => h.Id == sourceId)
            .OrderByDescending(h => h.HistoryRecordedAt)
            .FirstOrDefaultAsync(cancellationToken)!;

    /// <summary>Latest history row per source id, for a batch of ids in one query.</summary>
    public static async Task<Dictionary<int, THistory>> GetLatestHistoryByIdAsync<THistory>(
        this DbContext db, IReadOnlyCollection<int> sourceIds, CancellationToken cancellationToken)
        where THistory : class, IHistoryEntity
    {
        var rows = await db.Set<THistory>()
            .Where(h => sourceIds.Contains(h.Id))
            .ToListAsync(cancellationToken);

        return rows
            .GroupBy(h => h.Id)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(h => h.HistoryRecordedAt).First());
    }
}
