using DZDDashboard.Common.DTOs;
using MudBlazor;

namespace DZDDashboard.Client.Utils;

public static class UiHelpers
{
    private static Color ColorFromId(int id) => (id % 6) switch
    {
        0 => Color.Primary,
        1 => Color.Secondary,
        2 => Color.Info,
        3 => Color.Success,
        4 => Color.Warning,
        _ => Color.Error
    };

    /// <summary>Consistent avatar colour based on user id.</summary>
    public static Color GetAvatarColor(UserDto? user)
        => user is null ? Color.Primary : ColorFromId(user.Id);

    /// <summary>Overload for lightweight summary lists.</summary>
    public static Color GetAvatarColor(UserSummaryDto? user)
        => user is null ? Color.Primary : ColorFromId(user.Id);

    /// <summary>Overload for org-chart nodes.</summary>
    public static Color GetAvatarColor(OrgChartUserDto? user)
        => user is null ? Color.Primary : ColorFromId(user.Id);
}
