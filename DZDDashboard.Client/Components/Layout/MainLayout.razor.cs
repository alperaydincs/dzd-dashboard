using System.Security.Claims;
using DZDDashboard.Client.Services;
using DZDDashboard.Client.Theme;
using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;

namespace DZDDashboard.Client.Components.Layout;

public partial class MainLayout : IDisposable
{
    [Inject] private AuthenticationStateProvider  AuthStateProvider  { get; set; } = default!;
    [Inject] private NavigationManager            Nav                { get; set; } = default!;
    [Inject] private IUserClientService           UserService        { get; set; } = default!;
    [Inject] private INotificationCenterService   NotificationCenter { get; set; } = default!;
    [Inject] private IUserAvatarState             AvatarState        { get; set; } = default!;

    private const string DefaultTitle = "DZD Dashboard";
    private const string LoadingText  = "Loading…";

    private enum NavSection
    {
        None, Dashboard, Employees, Departments, Positions,
        Attendance, LeaveManagement, Training, Performance, Company, Settings, MyProfile
    }

    private sealed record NavLink(NavSection Section, string Href, string Icon, string Label, string? RequiredRoles = null);

    private static readonly IReadOnlyList<NavLink> NavLinks =
    [
        new(NavSection.Dashboard,       "/",                 DzdIcons.LayoutDashboard,   "Dashboard"),
        new(NavSection.Employees,       "/employees",        DzdIcons.Users,             "Employees"),
        new(NavSection.Departments,     "/departments",      DzdIcons.Building2,         "Departments"),
        new(NavSection.Positions,       "/positions",        DzdIcons.IdCard,            "Positions"),
        new(NavSection.Attendance,      "/attendance",       DzdIcons.Clock3,            "Attendance"),
        new(NavSection.LeaveManagement, "/leave-management", DzdIcons.CalendarCheck,     "Leave Management"),
        new(NavSection.Training,        "/training",         DzdIcons.GraduationCap,     "Training"),
        new(NavSection.Performance,     "/performance",      DzdIcons.TrendingUp,        "Performance"),
        new(NavSection.Company,         "/company",          DzdIcons.BriefcaseBusiness, "Company"),
        new(NavSection.Settings,        "/settings",         DzdIcons.Settings,          "Settings", Roles.AdminOrHr),
    ];

    private static readonly Dictionary<NavSection, string> SectionTitles = new()
    {
        [NavSection.Employees]       = "Employees",
        [NavSection.Departments]     = "Departments",
        [NavSection.Positions]       = "Positions",
        [NavSection.Attendance]      = "Attendance",
        [NavSection.LeaveManagement] = "Leave Management",
        [NavSection.Training]        = "Training",
        [NavSection.Performance]     = "Performance",
        [NavSection.Company]         = "Company",
        [NavSection.Settings]        = "Settings",
        [NavSection.MyProfile]       = "My Profile",
    };

    private sealed record UserHeader(string Name, string? AvatarDataUrl, int? ColorIndex)
    {
        public static readonly UserHeader Empty = new(string.Empty, null, null);
    }

    private readonly MudTheme _myTheme = AppTheme.Default;
    private bool       _drawerOpen = true;
    private bool       _ready;
    private NavSection _activeSection  = NavSection.Dashboard;
    private UserHeader _header         = UserHeader.Empty;
    private IReadOnlyList<AppNotification> _notifications = [];
    private int        _unreadNotifications;

    private bool   IsActive(NavSection section) => _activeSection == section;
    private string PageTitle => SectionTitles.GetValueOrDefault(_activeSection, DefaultTitle);
    private void   DrawerToggle() => _drawerOpen = !_drawerOpen;

    protected override async Task OnInitializedAsync()
    {
        SetActiveSection(Nav.Uri);
        AuthStateProvider.AuthenticationStateChanged += OnAuthStateChanged;
        Nav.LocationChanged                          += OnLocationChanged;
        NotificationCenter.Changed                   += OnNotificationsChanged;
        AvatarState.Changed                          += OnAvatarChanged;

        RefreshNotifications();
        await RefreshSessionAsync();
    }

    public void Dispose()
    {
        AuthStateProvider.AuthenticationStateChanged -= OnAuthStateChanged;
        Nav.LocationChanged                          -= OnLocationChanged;
        NotificationCenter.Changed                   -= OnNotificationsChanged;
        AvatarState.Changed                          -= OnAvatarChanged;
    }

    private async Task RefreshSessionAsync()
    {
        var user = (await AuthStateProvider.GetAuthenticationStateAsync()).User;

        if (user.Identity?.IsAuthenticated != true)
        {
            _header = UserHeader.Empty;
            _ready  = true;
            await InvokeAsync(StateHasChanged);
            return;
        }

        try
        {
            _header = await LoadHeaderAsync(user);
        }
        finally
        {
            _ready = true;
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task<UserHeader> LoadHeaderAsync(ClaimsPrincipal user)
    {
        var fallbackName = user.Identity?.Name ?? string.Empty;
        try
        {
            var profile = await UserService.GetMyProfileAsync();
            var name    = AppFormatter.BuildFullName(profile?.FirstName, profile?.LastName);
            var avatar  = await UserService.GetMyAvatarAsync();

            return new UserHeader(
                string.IsNullOrWhiteSpace(name) ? fallbackName : name,
                ToAvatarDataUrl(avatar),
                profile?.AvatarColorIndex);
        }
        catch
        {
            return new UserHeader(fallbackName, null, null);
        }
    }

    private static string? ToAvatarDataUrl(UserAvatarDto? avatar)
        => avatar is null || string.IsNullOrEmpty(avatar.ContentBase64)
            ? null
            : $"data:{avatar.ContentType ?? "image/png"};base64,{avatar.ContentBase64}";

    private void OnAuthStateChanged(Task<AuthenticationState> task) => _ = HandleAuthStateChangedAsync(task);

    private async Task HandleAuthStateChangedAsync(Task<AuthenticationState> task)
    {
        try { await task; } catch { }
        await RefreshSessionAsync();
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        SetActiveSection(e.Location);
        _ = InvokeAsync(StateHasChanged);
    }

    private void OnAvatarChanged() => _ = RefreshHeaderAsync();

    private async Task RefreshHeaderAsync()
    {
        var user = (await AuthStateProvider.GetAuthenticationStateAsync()).User;
        if (user.Identity?.IsAuthenticated != true) return;

        _header = await LoadHeaderAsync(user);
        await InvokeAsync(StateHasChanged);
    }

    private void OnNotificationsChanged()
    {
        RefreshNotifications();
        _ = InvokeAsync(StateHasChanged);
    }

    private void RefreshNotifications()
    {
        _notifications       = NotificationCenter.GetAll().ToList();
        _unreadNotifications = NotificationCenter.UnreadCount;
    }

    private void MarkNotificationAsRead(int id) => NotificationCenter.MarkAsRead(id);
    private void MarkAllNotificationsAsRead()   => NotificationCenter.MarkAllAsRead();

    private void Logout() => Nav.NavigateTo("/MicrosoftIdentity/Account/SignOut", forceLoad: true, replace: true);

    private void SetActiveSection(string location)
    {
        var path = Nav.ToBaseRelativePath(location);

        _activeSection = true switch
        {
            _ when string.IsNullOrEmpty(path)                                    => NavSection.Dashboard,
            _ when Seg(path, "employees") || Seg(path, "employee")               => NavSection.Employees,
            _ when Seg(path, "departments")                                      => NavSection.Departments,
            _ when Seg(path, "positions")                                        => NavSection.Positions,
            _ when Seg(path, "attendance")                                       => NavSection.Attendance,
            _ when Seg(path, "leave-management")                                 => NavSection.LeaveManagement,
            _ when Seg(path, "training")                                         => NavSection.Training,
            _ when Seg(path, "performance")                                      => NavSection.Performance,
            _ when Seg(path, "company")                                          => NavSection.Company,
            _ when Seg(path, "settings")                                         => NavSection.Settings,
            _ when path.Equals("my-profile", StringComparison.OrdinalIgnoreCase) => NavSection.MyProfile,
            _                                                                    => NavSection.None,
        };
    }

    private static bool Seg(string path, string segment)
        => path.Equals(segment, StringComparison.OrdinalIgnoreCase)
        || path.StartsWith(segment + "/", StringComparison.OrdinalIgnoreCase);
}
