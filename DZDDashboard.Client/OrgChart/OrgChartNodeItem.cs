using DZDDashboard.Common.DTOs.Organization;
using DZDDashboard.Common.DTOs.Users;

namespace DZDDashboard.Client.OrgChart;

public class OrgChartNodeItem
{
    public OrganizationPositionDto Position { get; set; } = default!;
    public List<UserDto> Users { get; set; } = new();
    public List<OrgChartNodeItem> Children { get; set; } = new();
    public bool IsExpanded { get; set; } = true;
    public bool HasUsersInSubtree { get; set; }
}
