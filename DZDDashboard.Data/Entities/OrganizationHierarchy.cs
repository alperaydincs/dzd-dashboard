namespace DZDDashboard.Data.Entities;

/// <summary>
/// Immutable view of the organization-position parent graph. Encapsulates the
/// pure hierarchy rules (ancestry / cycle detection) that previously lived
/// inline in the user service, keeping that domain knowledge with the domain.
/// </summary>
public sealed class OrganizationHierarchy(IReadOnlyDictionary<int, int?> parentByPositionId)
{
    /// <summary>
    /// True when <paramref name="descendantId"/> sits somewhere below
    /// <paramref name="ancestorId"/> in the position tree. Walks parent links
    /// upward with a guard against malformed cyclic data.
    /// </summary>
    public bool IsDescendant(int ancestorId, int descendantId)
    {
        var currentId = (int?)descendantId;
        var guard = 0;
        while (currentId.HasValue && parentByPositionId.TryGetValue(currentId.Value, out var parentId))
        {
            if (parentId == ancestorId) return true;
            currentId = parentId;
            if (++guard > parentByPositionId.Count) break;
        }
        return false;
    }
}
