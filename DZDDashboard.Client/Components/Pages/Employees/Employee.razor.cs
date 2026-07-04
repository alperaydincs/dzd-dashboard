using DZDDashboard.Client.Components.Pages;
using DZDDashboard.Client.Components.Pages.Employees.Dialogs;
using DZDDashboard.Client.Models;
using DZDDashboard.Client.Services;
using DZDDashboard.Client.Theme;
using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Utils;
using IntlTelInputBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;

namespace DZDDashboard.Client.Components.Pages.Employees;

public partial class Employee
{
    private static readonly DialogOptions SmallEscDialog =
        new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small, FullWidth = true };

    [Parameter] public string? Slug { get; set; }

    [Parameter] public bool SelfService { get; set; }

    private int _userId;

    [Inject] private NavigationManager Nav { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] private IDialogService DialogService { get; set; } = default!;
    [Inject] private IUserClientService UserService { get; set; } = default!;
    [Inject] private IOrganizationClientService OrgService { get; set; } = default!;
    [Inject] private ITrainingClientService TrainingService { get; set; } = default!;
    [Inject] private IUserAvatarState AvatarState { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;
    [Inject] private DZDDashboard.Client.Localization.AppLocalizer Loc { get; set; } = default!;
    [Inject] private DZDDashboard.Client.Localization.DomainLocalizer Domain { get; set; } = default!;

    private bool _loading;
    private string? _error;
    private EmployeeCardDto? _profile;
    private readonly SectionEditState<EmployeeCardDto>              _basicInfoEdit   = new();
    private readonly SectionEditState<EmployeeCardDto>              _contactsEdit    = new();
    private readonly SectionEditState<EmployeeCardDto>              _citizenshipEdit = new();
    private readonly SectionEditState<EmployeeCardDto>              _emergencyEdit   = new();
    private readonly SectionEditState<EmployeeCardDto>              _familyEdit      = new();
    private readonly SectionEditState<EmployeeCardDto>              _addressEdit     = new();
    private readonly SectionEditState<List<EducationHistoryRecord>> _educationEdit   = new();

    private string _avatarDataUrl = string.Empty;
    private string _fullName = string.Empty;
    private string? _employmentDuration;
    private List<EducationHistoryRecord> _educationHistoryRecords = [];
    private IntlTel _workPhoneIntl = new();
    private IntlTel _personalPhoneIntl = new();
    private Dictionary<EmergencyContactDto, IntlTel> _emergencyContactPhones = [];
    private int _activeViewIndex;
    private List<PayrollLocationDto> _payrollLocations = [];

    private readonly CareerTabState _career = new();

    private TrainingProgressSummaryDto? _trainingProgress;
    private bool _trainingLoading;
    private bool _trainingLoaded;
    private TrainingProgressStatus? _trainingFilter; // null = All

    private readonly (TrainingProgressStatus? Value, string LabelKey)[] _trainingFilters =
    [
        (null,                              "trainings.filterAll"),
        (TrainingProgressStatus.Completed,  "trainings.statusCompleted"),
        (TrainingProgressStatus.InProgress, "trainings.statusInProgress"),
        (TrainingProgressStatus.Upcoming,   "trainings.statusUpcoming")
    ];

    private List<TrainingProgressItemDto> FilteredTrainings()
    {
        var items = _trainingProgress?.Items ?? [];
        return _trainingFilter is null
            ? items
            : items.Where(i => i.Status == _trainingFilter).ToList();
    }

    private (string Text, string Icon, string Class) TrainingStatusChip(TrainingProgressStatus status) => status switch
    {
        TrainingProgressStatus.Completed  => (Loc["trainings.statusCompleted"],  DzdIcons.CircleCheck, "trn-chip-done"),
        TrainingProgressStatus.InProgress => (Loc["trainings.statusInProgress"], DzdIcons.Loader,      "trn-chip-progress"),
        _                                 => (Loc["trainings.statusUpcoming"],   DzdIcons.Clock,       "trn-chip-upcoming")
    };

    private async Task LoadTrainingsAsync()
    {
        _trainingLoading = true;
        try
        {
            _trainingProgress = SelfService
                ? await TrainingService.GetMyProgressAsync()
                : await TrainingService.GetUserProgressAsync(_userId);
        }
        catch
        {
            _trainingProgress = null;
        }
        finally
        {
            _trainingLoaded  = true;
            _trainingLoading = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    private List<PositionHistoryDto> _positionHistory = [];

    private List<UserDocumentDto> _cvDocuments = [];
    private bool _cvUploading;
    private const string CvAcceptList = ".pdf,.doc,.docx,.xls,.xlsx,.png,.jpg,.jpeg";

    protected override async Task OnInitializedAsync()
    {
        if (SelfService)
        {
            await LoadSelfData();
            return;
        }

        if (string.IsNullOrWhiteSpace(Slug))
        {
            _error = Loc["employeeProfile.notSelected"];
            return;
        }

        var openPersonalInfo = new Uri(Nav.Uri).Query.Contains("tab=personal", StringComparison.OrdinalIgnoreCase);
        if (openPersonalInfo)
        {
            _activeViewIndex = 1;
            Nav.NavigateTo($"/employee/{Slug}", replace: true);
        }

        await LoadData();
    }

    private async Task LoadSelfData()
    {
        _loading = true;
        _error   = null;
        try
        {
            _profile = await UserService.GetMyCardAsync();
            if (_profile is null) { _error = Loc["employeeProfile.myProfileLoadFailed"]; return; }
            _userId = _profile.Id;

            MergeSensitiveInfo(_profile, await UserService.GetMySensitiveInfoAsync());

            _fullName           = AppFormatter.BuildFullName(_profile.FirstName, _profile.LastName);
            _employmentDuration = FormatDurationLocalized(_profile.UserStartDate ?? _profile.PositionStartDate);

            var avatar = await UserService.GetMyAvatarAsync();
            if (avatar is { ContentBase64: { Length: > 0 } ab64 })
                _avatarDataUrl = $"data:{avatar.ContentType ?? "image/png"};base64,{ab64}";

            SyncIntlPhoneInputs();
            _educationHistoryRecords = MapEducationHistories(_profile);
            _positionHistory = MapPositionHistory(_profile);
        }
        catch (Exception)
        {
            _error = Loc["employeeProfile.loadFailed"];
            Snackbar.Add(_error, Severity.Error);
        }
        finally { _loading = false; }
    }

    private async Task LoadData()
    {
        _loading = true;
        _error   = null;

        try
        {
            _profile = await UserService.GetEmployeeCardBySlugAsync(Slug!);
            if (_profile is null) { _error = Loc["employeeProfile.userNotFound"]; return; }
            _userId = _profile.Id;

            var sensitiveInfoTask = UserService.GetSensitiveInfoAsync(_userId);
            var avatarTask        = UserService.GetUserAvatarAsync(_userId);
            await Task.WhenAll(sensitiveInfoTask, avatarTask);

            _payrollLocations = await OrgService.GetPayrollLocationsAsync();

            MergeSensitiveInfo(_profile, await sensitiveInfoTask);

            _fullName           = AppFormatter.BuildFullName(_profile.FirstName, _profile.LastName);
            _employmentDuration = FormatDurationLocalized(_profile.UserStartDate ?? _profile.PositionStartDate);

            var avatar = await avatarTask;
            if (avatar != null && !string.IsNullOrEmpty(avatar.ContentBase64))
                _avatarDataUrl = $"data:{avatar.ContentType ?? "image/png"};base64,{avatar.ContentBase64}";

            SyncIntlPhoneInputs();
            _educationHistoryRecords = MapEducationHistories(_profile);
            _positionHistory = MapPositionHistory(_profile);
        }
        catch (HttpRequestException)
        {
            _error = Loc["employeeProfile.employeeLoadFailed"];
            Snackbar.Add(_error, Severity.Error);
        }
        catch (Exception)
        {
            _error = Loc["employeeProfile.unexpectedLoadError"];
            Snackbar.Add(_error, Severity.Error);
        }
        finally { _loading = false; }
    }

    private string? GetContractTypeName(string? code)   => string.IsNullOrEmpty(code) ? null : Domain.Label(DomainCategories.ContractType, code);
    private string? GetWorkModelName(string? code)      => string.IsNullOrEmpty(code) ? null : Domain.Label(DomainCategories.WorkModel, code);
    private string? GetEducationLevelName(string? code) => string.IsNullOrEmpty(code) ? null : Domain.Label(DomainCategories.EducationLevel, code);
    private string GetEducationStatusName(string? code) => string.IsNullOrEmpty(code) ? "-" : Domain.Label(DomainCategories.EducationStatus, code);
    private string GetPositionChangeTypeName(string? code) => string.IsNullOrEmpty(code) ? "-" : Domain.Label(DomainCategories.PositionChangeType, code);

    private async Task RefreshProfileAsync()
    {
        if (_userId <= 0) return;

        var cardTask     = SelfService ? UserService.GetMyCardAsync()         : UserService.GetEmployeeCardAsync(_userId);
        var piiTask      = SelfService ? UserService.GetMySensitiveInfoAsync() : UserService.GetSensitiveInfoAsync(_userId);
        await Task.WhenAll(cardTask, piiTask);

        var latestProfile = await cardTask;
        if (latestProfile is null) return;

        MergeSensitiveInfo(latestProfile, await piiTask);
        _profile = latestProfile;

        _fullName           = AppFormatter.BuildFullName(_profile.FirstName, _profile.LastName);
        _employmentDuration = FormatDurationLocalized(_profile.UserStartDate ?? _profile.PositionStartDate);

        var avatar = SelfService ? await UserService.GetMyAvatarAsync() : await UserService.GetUserAvatarAsync(_userId);
        if (avatar is { ContentBase64: { Length: > 0 } b64 })
            _avatarDataUrl = $"data:{avatar.ContentType ?? "image/png"};base64,{b64}";

        SyncIntlPhoneInputs();
        _educationHistoryRecords = MapEducationHistories(_profile);
        _positionHistory = MapPositionHistory(_profile);
    }

    private static void MergeSensitiveInfo(EmployeeCardDto card, EmployeeSensitiveInfoDto? pii)
    {
        if (pii is null) return;
        card.DateOfBirth        = pii.DateOfBirth;
        card.Gender             = pii.Gender;
        card.Nationality        = pii.Nationality;
        card.CitizenshipNumber  = pii.CitizenshipNumber;
        card.DisabilityStatus   = pii.DisabilityStatus;
        card.DisabilityDegree   = pii.DisabilityDegree;
        card.MaritalStatus      = pii.MaritalStatus;
        card.SpouseFullName     = pii.SpouseFullName;
        card.PersonalEmail      = pii.PersonalEmail;
        card.PersonalPhoneNumber = pii.PersonalPhoneNumber;
        card.LegalAddress        = pii.LegalAddress;
        card.LegalAddressCity    = pii.LegalAddressCity;
        card.LegalAddressCountry = pii.LegalAddressCountry;
        card.CurrentAddress      = pii.CurrentAddress;
        card.CurrentAddressChangedAt = pii.CurrentAddressChangedAt;
        card.City                = pii.City;
        card.Country             = pii.Country;
        card.Children           = pii.Children
            .Select(c => new ChildInfoDto { Id = c.Id, FullName = c.FullName, DateOfBirth = c.DateOfBirth })
            .ToList();
    }

    private static List<EducationHistoryRecord> MapEducationHistories(EmployeeCardDto? profile)
        => (profile?.EducationHistories ?? [])
            .Select(x => new EducationHistoryRecord
            {
                Id             = x.Id,
                EducationLevel = x.EducationLevel,
                Institution    = x.Institution ?? string.Empty,
                Program        = x.Program,
                GraduationDate = x.GraduationDate,
                Status         = x.Status
            })
            .ToList();

    private static List<PositionHistoryDto> MapPositionHistory(EmployeeCardDto? profile)
        => (profile?.PositionHistories ?? [])
            .OrderByDescending(p => p.EndDate == null)
            .ThenByDescending(p => p.StartDate)
            .ToList();

    private void SetView(int index)
    {
        _activeViewIndex = index;
        if (index == 2 && !_career.IsLoaded)
            _ = LoadCareerDataAsync();
        if (index == 4 && !_trainingLoaded)
            _ = LoadTrainingsAsync();
    }

    private async Task CopyToClipboard(string? text)
    {
        if (string.IsNullOrEmpty(text)) return;
        try
        {
            await JS.InvokeVoidAsync("navigator.clipboard.writeText", text);
            Snackbar.Add(Loc["employeeProfile.copiedToClipboard"], Severity.Normal);
        }
        catch
        {
            Snackbar.Add(Loc["employeeProfile.copyFailed"], Severity.Error);
        }
    }

    private void SyncIntlPhoneInputs()
    {
        _workPhoneIntl     = new IntlTel { Number = _profile?.PhoneNumber };
        _personalPhoneIntl = new IntlTel { Number = _profile?.PersonalPhoneNumber };
    }

    private string? GetWorkPhoneValue()     => _profile?.PhoneNumber;
    private string? GetPersonalPhoneValue() => _profile?.PersonalPhoneNumber;
    private string GetWorkPhoneDisplay()     => GetWorkPhoneValue()     is { } v ? AppFormatter.FormatPhoneDisplay(v) : Loc["common.notAvailable"];
    private string GetPersonalPhoneDisplay() => GetPersonalPhoneValue() is { } v ? AppFormatter.FormatPhoneDisplay(v) : Loc["common.notAvailable"];

    private string? FormatDurationLocalized(DateTime? start)
    {
        if (start is null) return null;
        var (years, months) = AppFormatter.GetElapsedYearsMonths(start);
        if (years <= 0 && months <= 0) return Loc["duration.lessThanMonth"];

        var yearsPart  = years  > 0 ? string.Format(years  == 1 ? Loc["duration.yearSingular"]  : Loc["duration.yearPlural"],  years)  : null;
        var monthsPart = months > 0 ? string.Format(months == 1 ? Loc["duration.monthSingular"] : Loc["duration.monthPlural"], months) : null;
        return string.Join(" ", new[] { yearsPart, monthsPart }.Where(s => s is not null));
    }

    private static string? FirstNonEmpty(params string?[] values)
    {
        foreach (var v in values)
            if (!string.IsNullOrWhiteSpace(v)) return v;
        return null;
    }

    private void Back() => Nav.NavigateTo("/employees");
    private string GetManagerDisplayName()
    {
        if (_profile?.ReportsTo is null) return Loc["common.notAvailable"];
        var name = AppFormatter.BuildFullName(_profile.ReportsTo.FirstName, _profile.ReportsTo.LastName);
        return string.IsNullOrWhiteSpace(name) ? Loc["common.notAvailable"] : name;
    }
    private string? GetManagerAvatarUrl() => _profile?.ReportsTo is { HasAvatar: true } m ? AvatarUrl.For(m.Id, m.AvatarUpdatedAt) : null;
    private string GetManagerEmailDisplay() => _profile?.ReportsTo?.Email ?? Loc["common.notAvailable"];
    private string GetManagerPhoneDisplay() => _profile?.ReportsTo?.PhoneNumber is { } v ? AppFormatter.FormatPhoneDisplay(v) : Loc["common.notAvailable"];

    private sealed class EducationHistoryRecord
    {
        public int?      Id               { get; set; }
        public string?   EducationLevel   { get; set; }
        public string    Institution      { get; set; } = string.Empty;
        public string?   Program          { get; set; }
        public DateTime? GraduationDate   { get; set; }
        public string?   Status           { get; set; }
    }

    private static Color LifecycleColor(string? status, bool isActive) => status switch
    {
        UserLifecycleStatuses.Active      => Color.Success,
        UserLifecycleStatuses.Onboarding  => Color.Info,
        UserLifecycleStatuses.Candidate   => Color.Info,
        UserLifecycleStatuses.Offboarding => Color.Warning,
        UserLifecycleStatuses.Exited      => Color.Error,
        _                                 => isActive ? Color.Success : Color.Default
    };

    private string LifecycleLabel(string? status, bool isActive) => status switch
    {
        UserLifecycleStatuses.Active      => Loc["common.active"],
        UserLifecycleStatuses.Onboarding  => Loc["employeeProfile.statusOnboarding"],
        UserLifecycleStatuses.Candidate   => Loc["employeeProfile.statusCandidate"],
        UserLifecycleStatuses.Offboarding => Loc["employeeProfile.statusOffboarding"],
        UserLifecycleStatuses.Exited      => Loc["employeeProfile.statusExited"],
        _                                 => isActive ? Loc["common.active"] : Loc["common.inactive"]
    };
}
