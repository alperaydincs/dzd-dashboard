using MudBlazor;

namespace DZDDashboard.Client.Theme;

public static class AppTheme
{
    public static MudTheme Default => new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#1E88E5",
            Secondary = "#FF4081",
            AppbarBackground = "#FFFFFF",
            AppbarText = "#424242",
            Background = "#F5F5F5",
            DrawerBackground = "#FFFFFF",
            DrawerText = "rgba(0,0,0, 0.7)",
            Success = "#00C853",
            Warning = "#FFB74D",
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#90CAF9",
            Secondary = "#FF80AB",
            AppbarBackground = "#1E1E1E",
            Background = "#121212",
            DrawerBackground = "#1E1E1E",
            DrawerText = "rgba(255,255,255, 0.7)",
            Success = "#00E676",
            Warning = "#FFCC80"
        },
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "6px"
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
