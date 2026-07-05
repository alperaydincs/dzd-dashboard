using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Client.Components.OrgChart;

public static class OrgChartBuilder
{
    public static List<OrgChartNodeItem> Build(List<OrganizationPositionDto> positions)
    {
        var childMap = positions
            .Where(p => p.ParentId.HasValue)
            .GroupBy(p => p.ParentId!.Value)
            .ToDictionary(g => g.Key, g => g.OrderBy(p => p.Name).ToList());

        return positions
            .Where(p => p.ParentId == null)
            .OrderBy(p => p.Name)
            .Select(root => CreateNode(root, childMap))
            .ToList();
    }

    private static OrgChartNodeItem CreateNode(
        OrganizationPositionDto pos,
        Dictionary<int, List<OrganizationPositionDto>> childMap)
    {
        var node = new OrgChartNodeItem
        {
            Position   = pos,
            User       = pos.User,
            Children   = [],
            IsExpanded = true
        };

        if (childMap.TryGetValue(pos.Id, out var children))
            foreach (var child in children)
                node.Children.Add(CreateNode(child, childMap));

        return node;
    }

    public static List<OrgChartNodeItem> FilterByUserIds(List<OrgChartNodeItem> nodes, HashSet<int> matchingUserIds)
    {
        var result = new List<OrgChartNodeItem>();

        foreach (var node in nodes)
        {
            var filteredChildren = FilterByUserIds(node.Children, matchingUserIds);
            var nodeMatches = node.User != null && matchingUserIds.Contains(node.User.Id);

            if (!nodeMatches && filteredChildren.Count == 0)
                continue;

            result.Add(new OrgChartNodeItem
            {
                Position = node.Position,
                User = nodeMatches ? node.User : null,
                Children = filteredChildren,
                IsExpanded = true
            });
        }

        return result;
    }
}
