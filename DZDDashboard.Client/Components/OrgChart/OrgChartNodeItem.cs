using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Client.Components.OrgChart;

public class OrgChartNodeItem
{
    public OrganizationPositionDto Position { get; set; } = default!;
    public UserDto? User { get; set; } 
    public List<OrgChartNodeItem> Children { get; set; } = new();
    public bool IsExpanded { get; set; } = true;
}
