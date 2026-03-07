using MudBlazor;

namespace DZDDashboard.Client.Theme;

public static class AppTheme
{
    public static MudTheme Default => new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#2B38F5",
            Secondary = "#70A37F",
            Tertiary = "#F46036",
            Info = "#193CB8",
            Success = "#016630",
            Warning = "#FEA82F",
            Error = "#E7000B",
            Dark = "#0A0A0A",
            TextPrimary = "#0A0A0A",
            TextSecondary = "#717182",
            TextDisabled = "#9A9AAF",
            AppbarBackground = "#FEFEFF",
            AppbarText = "#0A0A0A",
            Background = "#FEFEFF",
            Surface = "#FAFAFA",
            DrawerBackground = "#FAFAFA",
            DrawerText = "#717182",
            LinesDefault = "#E5E6EA",
            LinesInputs = "#E5E6EA"
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#6A76FF",
            Secondary = "#89B597",
            Tertiary = "#FF8D6C",
            Info = "#7AA0FF",
            Success = "#46A171",
            Warning = "#FFBF63",
            Error = "#FF6573",
            AppbarBackground = "#161825",
            Background = "#0D101B",
            Surface = "#161A26",
            DrawerBackground = "#131724",
            DrawerText = "#C8CBD8"
        },
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "10px"
        },
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Arial", "Helvetica", "sans-serif"]
            }
        }
    };
}
