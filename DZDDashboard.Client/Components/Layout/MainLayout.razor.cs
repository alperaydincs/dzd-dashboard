using System.Security.Claims;
using DZDDashboard.Client.Services;
using DZDDashboard.Client.Theme;
using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using MudBlazor;

namespace DZDDashboard.Client.Components.Layout;

public partial class MainLayout : IDisposable
{
    [Inject] private AuthenticationStateProvider  AuthStateProvider  { get; set; } = default!;
    [Inject] private NavigationManager            Nav                { get; set; } = default!;
    [Inject] private IUserClientService           UserService        { get; set; } = default!;
    [Inject] private INotificationCenterService   NotificationCenter { get; set; } = default!;
    [Inject] private IUserAvatarState             AvatarState        { get; set; } = default!;
    [Inject] private IMyOnboardingClientService   MyOnboarding       { get; set; } = default!;
    [Inject] private IOnboardingClientService     OnboardingService  { get; set; } = default!;
    [Inject] private IOffboardingClientService    OffboardingService { get; set; } = default!;

    private const string OnboardingPath = "my-onboarding";
    private bool _onboardingMode;

    private const string DefaultTitle = "DZD Dashboard";
    private const string LoadingText  = "Loading…";

    private enum NavSection
    {
        None, Dashboard, Employees, Onboarding, Offboarding, Departments, Positions,
        Attendance, LeaveManagement, Training, Performance, Company, Settings, MyProfile
    }

    private sealed record NavLink(NavSection Section, string Href, string Icon, string Label, string? RequiredRoles = null);

    private static readonly IReadOnlyList<NavLink> NavLinks =
    [
        new(NavSection.Dashboard,       "/",                     DzdIcons.LayoutDashboard, "nav.dashboard"),
        new(NavSection.Employees,       "/employees",            DzdIcons.Users,           "nav.employees"),
        new(NavSection.Onboarding,      "/onboarding-offboarding", DzdIcons.UserPlus,       "nav.onboardingOffboarding", Roles.AdminOrHr),
    ];

    private static readonly NavLink SettingsNavLink =
        new(NavSection.Settings, "/settings", DzdIcons.Settings, "nav.settings", Roles.AdminOrHr);

    private static readonly Dictionary<NavSection, string> SectionTitles = new()
    {
        [NavSection.Employees]       = "nav.employees",
        [NavSection.Onboarding]      = "nav.onboardingOffboarding",
        [NavSection.Offboarding]     = "nav.onboardingOffboarding",
        [NavSection.Departments]     = "nav.departments",
        [NavSection.Positions]       = "nav.positions",
        [NavSection.Attendance]      = "nav.attendance",
        [NavSection.LeaveManagement] = "nav.leaveManagement",
        [NavSection.Training]        = "nav.training",
        [NavSection.Performance]     = "nav.performance",
        [NavSection.Company]         = "nav.company",
        [NavSection.Settings]        = "nav.settings",
        [NavSection.MyProfile]       = "nav.myProfile",
    };

    private sealed record UserHeader(string Name, string? AvatarSrc, int? ColorIndex)
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
            await RefreshOnboardingModeAsync();
            await RefreshDueSoonDocumentNotificationsAsync();
        }
        finally
        {
            _ready = true;
            await InvokeAsync(StateHasChanged);
        }
    }

    private bool _dueSoonNotificationsLoaded;

    private async Task RefreshDueSoonDocumentNotificationsAsync()
    {
        if (_dueSoonNotificationsLoaded) return;
        try
        {
            _dueSoonNotificationsLoaded = true;
            var onboardingDue  = await OnboardingService.GetDueSoonDocumentsAsync() ?? [];
            var offboardingDue = await OffboardingService.GetDueSoonDocumentsAsync() ?? [];
            foreach (var due in onboardingDue.Concat(offboardingDue))
                NotificationCenter.Add(due.EmployeeName ?? Loc["appbar.notifications"], string.Format(Loc["notifications.documentDueSoon"], due.DocumentName));
        }
        catch
        {
            // Non-admin/HR users are not authorized for this endpoint — safe to ignore.
        }
    }

    private async Task RefreshOnboardingModeAsync()
    {
        try
        {
            var state = await MyOnboarding.GetStateAsync();
            _onboardingMode = state?.LifecycleStatus == UserLifecycleStatuses.Onboarding
                           || state?.LifecycleStatus == UserLifecycleStatuses.Candidate;
            if (_onboardingMode) EnsureOnboardingRoute();
        }
        catch
        {
            _onboardingMode = false;
        }
    }

    private void EnsureOnboardingRoute()
    {
        var path = Nav.ToBaseRelativePath(Nav.Uri).Split('?')[0];
        if (!path.Equals(OnboardingPath, StringComparison.OrdinalIgnoreCase))
            Nav.NavigateTo("/" + OnboardingPath, forceLoad: false, replace: true);
    }

    private async Task<UserHeader> LoadHeaderAsync(ClaimsPrincipal user)
    {
        var fallbackName = user.Identity?.Name ?? string.Empty;
        try
        {
            var profile = await UserService.GetMyProfileAsync();
            var name    = AppFormatter.BuildFullName(profile?.FirstName, profile?.LastName);

            // The avatar image is served on demand from the /avatars/me proxy; we only need to
            // know whether one exists and its version for cache-busting. No base64 payload here.
            var avatarSrc = profile is { HasAvatar: true }
                ? AvatarUrl.Mine(profile.AvatarUpdatedAt)
                : null;

            return new UserHeader(
                string.IsNullOrWhiteSpace(name) ? fallbackName : name,
                avatarSrc,
                profile?.AvatarColorIndex);
        }
        catch
        {
            return new UserHeader(fallbackName, null, null);
        }
    }

    private void OnAuthStateChanged(Task<AuthenticationState> task) => _ = HandleAuthStateChangedAsync(task);

    private async Task HandleAuthStateChangedAsync(Task<AuthenticationState> task)
    {
        // The auth-state task may fault on sign-out/refresh races; we re-read state below regardless.
        try { await task; } catch (Exception) { /* intentionally ignored — state is re-fetched next */ }
        await RefreshSessionAsync();
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        SetActiveSection(e.Location);
        if (_onboardingMode) EnsureOnboardingRoute();
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

    private async Task SetCulture(string culture) => await JS.InvokeVoidAsync("dzdSetCulture", culture);

    private void SetActiveSection(string location)
    {
        var path = Nav.ToBaseRelativePath(location);

        _activeSection = true switch
        {
            _ when string.IsNullOrEmpty(path)                                    => NavSection.Dashboard,
            _ when Seg(path, "onboarding-offboarding")                           => NavSection.Onboarding,
            _ when Seg(path, "onboarding")                                       => NavSection.Onboarding,
            _ when Seg(path, "offboarding")                                      => NavSection.Offboarding,
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
