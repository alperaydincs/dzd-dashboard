using DZDDashboard.Client.Components.Pages.Employees.Dialogs;
using DZDDashboard.Client.Services;
using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Utils;
using IntlTelInputBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace DZDDashboard.Client.Components.Pages.Employees;

/// <summary>Code-behind for EmployeeCard.razor — separated from UI markup for maintainability.</summary>
public partial class EmployeeCard
{
    private static readonly DialogOptions SmallEscDialog =
        new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small, FullWidth = true };

    /// <summary>Route parameter — preferred navigation method. Falls back to EmployeeNavigationState.</summary>
    [Parameter] public int EmployeeId { get; set; }

    [Inject] private NavigationManager Nav { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] private IDialogService DialogService { get; set; } = default!;
    [Inject] private IUserClientService UserService { get; set; } = default!;
    [Inject] private IOrganizationClientService OrgService { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;

    // ── State ────────────────────────────────────────────────────────────────
    private bool _loading;
    private string? _error;
    private EmployeeCardDto? _profile;
    // ── Per-section edit state (IsEditing + rollback snapshot) ───────────────
    private readonly SectionEditState<EmployeeCardDto>              _basicInfoEdit   = new();
    private readonly SectionEditState<EmployeeCardDto>              _contactsEdit    = new();
    private readonly SectionEditState<EmployeeCardDto>              _citizenshipEdit = new();
    private readonly SectionEditState<EmployeeCardDto>              _emergencyEdit   = new();
    private readonly SectionEditState<EmployeeCardDto>              _familyEdit      = new();
    private readonly SectionEditState<EmployeeCardDto>              _addressEdit     = new();
    private readonly SectionEditState<List<EducationHistoryRecord>> _educationEdit   = new();

    private string _avatarDataUrl = string.Empty;
    private string _fullName = string.Empty;
    private string _initials = "?";
    private string? _employmentDuration;
    private List<EducationHistoryRecord> _educationHistoryRecords = [];
    private IntlTel _workPhoneIntl = new();
    private IntlTel _personalPhoneIntl = new();
    private Dictionary<EmergencyContactDto, IntlTel> _emergencyContactPhones = [];
    private int _activeViewIndex;
    private List<PayrollLocationDto> _payrollLocations = [];

    // Career tab state — all 15 fields encapsulated in CareerTabState
    private readonly CareerTabState _career = new();

    // ── Lifecycle ─────────────────────────────────────────────────────────────
    // @attribute [Authorize(Roles = Roles.Admin)] on the razor file enforces access before
    // OnInitializedAsync runs — no manual role check needed here.
    protected override async Task OnInitializedAsync()
    {
        if (EmployeeId <= 0)
        {
            _error = "Employee not selected. Please open a profile from the Employees page.";
            return;
        }
        await LoadData();
    }

    // ── Data loading ──────────────────────────────────────────────────────────
    private async Task LoadData()
    {
        _loading = true;
        _error   = null;

        try
        {
            var selectedId  = EmployeeId;

            // Critical calls run in parallel — any failure surfaces to the outer catch.
            // sensitiveInfoTask fetches PII via SensitiveDataPolicy endpoint because the card endpoint
            // intentionally does not return PII (gated behind the sensitive-info endpoint for compliance).
            var profileTask       = UserService.GetEmployeeCardAsync(selectedId);
            var sensitiveInfoTask = UserService.GetSensitiveInfoAsync(selectedId);
            var avatarTask        = UserService.GetUserAvatarAsync(selectedId);
            await Task.WhenAll(profileTask, sensitiveInfoTask, avatarTask);

            // Non-critical: payroll locations enrich dropdowns but don't block rendering.
            // Isolated so a failure here does not abort the whole card load.
            try { _payrollLocations = await OrgService.GetPayrollLocationsAsync(); } catch { /* non-critical */ }

            _profile = await profileTask;
            if (_profile is not null)
                MergeSensitiveInfo(_profile, await sensitiveInfoTask);

            if (_profile is null) { _error = "User profile not found."; return; }

            _fullName           = AppFormatter.BuildFullName(_profile.FirstName, _profile.LastName);
            _initials           = AppFormatter.GetInitials(_fullName);
            _employmentDuration = AppFormatter.FormatDurationFrom(_profile.UserStartDate ?? _profile.PositionStartDate);

            try
            {
                if (_profile.Avatar != null && !string.IsNullOrEmpty(_profile.Avatar.ContentBase64))
                    _avatarDataUrl = $"data:{_profile.Avatar.ContentType ?? "image/png"};base64,{_profile.Avatar.ContentBase64}";
                else
                {
                    var avatar = await avatarTask;
                    if (avatar != null && !string.IsNullOrEmpty(avatar.ContentBase64))
                        _avatarDataUrl = $"data:{avatar.ContentType ?? "image/png"};base64,{avatar.ContentBase64}";
                }
            }
            catch { /* avatar load is non-critical */ }

            SyncIntlPhoneInputs();
            _educationHistoryRecords = MapEducationHistories(_profile);
        }
        catch (HttpRequestException)
        {
            _error = "Failed to load employee profile.";
            Snackbar.Add(_error, Severity.Error);
        }
        catch (Exception)
        {
            _error = "Unexpected error while loading profile.";
            Snackbar.Add(_error, Severity.Error);
        }
        finally { _loading = false; }
    }

    private async Task RefreshProfileAsync()
    {
        if (EmployeeId <= 0) return;

        var cardTask     = UserService.GetEmployeeCardAsync(EmployeeId);
        var piiTask      = UserService.GetSensitiveInfoAsync(EmployeeId);
        await Task.WhenAll(cardTask, piiTask);

        var latestProfile = await cardTask;
        if (latestProfile is null) return;

        MergeSensitiveInfo(latestProfile, await piiTask);
        _profile = latestProfile;

        _fullName           = AppFormatter.BuildFullName(_profile.FirstName, _profile.LastName);
        _initials           = AppFormatter.GetInitials(_fullName);
        _employmentDuration = AppFormatter.FormatDurationFrom(_profile.UserStartDate ?? _profile.PositionStartDate);

        SyncIntlPhoneInputs();
        _educationHistoryRecords = MapEducationHistories(_profile);
    }

    /// <summary>
    /// Merges PII fields from <paramref name="pii"/> into the card DTO.
    /// The card endpoint intentionally returns null for PII fields (gated by SensitiveDataPolicy).
    /// This helper populates them from the dedicated sensitive-info endpoint so the edit sections
    /// can show the current values when the admin opens them.
    /// </summary>
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
        card.LegalAddress       = pii.LegalAddress;
        card.CurrentAddress     = pii.CurrentAddress;
        card.City               = pii.City;
        card.Country            = pii.Country;
        card.Children           = pii.Children
            .Select(c => new ChildInfoDto { Id = c.Id, FullName = c.FullName, DateOfBirth = c.DateOfBirth })
            .ToList();
    }

    private static List<EducationHistoryRecord> MapEducationHistories(EmployeeCardDto? profile)
        => (profile?.EducationHistories ?? [])
            .Select(x => new EducationHistoryRecord
            {
                Id             = x.Id,
                Level          = x.Level ?? string.Empty,
                Institution    = x.Institution ?? string.Empty,
                Program        = x.Program,
                GraduationDate = x.GraduationDate,
                Status         = x.Status
            })
            .ToList();

    // ── Edit start / cancel ───────────────────────────────────────────────────
    private bool _isEditingActionInProgress;

    /// <summary>
    /// Shared guard + try/finally wrapper for all "start edit section" actions.
    /// Eliminates the identical boilerplate repeated across seven Start*Edit methods.
    /// </summary>
    private async Task StartEditAsync(Action configure)
    {
        if (_isEditingActionInProgress || _profile is null) return;
        _isEditingActionInProgress = true;
        try
        {
            configure();
            await InvokeAsync(StateHasChanged);
        }
        finally { _isEditingActionInProgress = false; }
    }

    private Task StartBasicInfoEdit() => StartEditAsync(() =>
        _basicInfoEdit.Begin(new EmployeeCardDto
        {
            FirstName          = _profile!.FirstName,
            LastName           = _profile.LastName,
            RegistrationNumber = _profile.RegistrationNumber,
            UserStartDate      = _profile.UserStartDate,
            PositionStartDate  = _profile.PositionStartDate,
            ContractType       = _profile.ContractType,
            ContractEndDate    = _profile.ContractEndDate,
            WorkModel          = _profile.WorkModel,
            PayrollLocationId  = _profile.PayrollLocationId
        }));

    private Task StartContactsEdit() => StartEditAsync(() =>
    {
        _contactsEdit.Begin(new EmployeeCardDto
        {
            Email               = _profile!.Email,
            PhoneNumber         = _profile.PhoneNumber,
            PersonalEmail       = _profile.PersonalEmail,
            PersonalPhoneNumber = _profile.PersonalPhoneNumber
        });
        SyncIntlPhoneInputs();
    });

    private Task StartCitizenshipInfoEdit() => StartEditAsync(() =>
        _citizenshipEdit.Begin(new EmployeeCardDto
        {
            DateOfBirth       = _profile!.DateOfBirth,
            Gender            = _profile.Gender,
            Nationality       = _profile.Nationality,
            CitizenshipNumber = _profile.CitizenshipNumber,
            DisabilityStatus  = _profile.DisabilityStatus,
            DisabilityDegree  = _profile.DisabilityDegree
        }));

    private Task StartEmergencyInfoEdit() => StartEditAsync(() =>
    {
        _emergencyEdit.Begin(new EmployeeCardDto
        {
            EmergencyContacts = _profile!.EmergencyContacts?
                .Select(c => new EmergencyContactDto
                {
                    Id = c.Id, FullName = c.FullName,
                    Relationship = c.Relationship, PhoneNumber = c.PhoneNumber
                }).ToList() ?? []
        });
        _emergencyContactPhones = _profile.EmergencyContacts?
            .ToDictionary(c => c, c => new IntlTel { Number = c.PhoneNumber })
            ?? [];
    });

    private void OnEmergencyPhoneChanged(EmergencyContactDto contact, IntlTel newValue)
    {
        _emergencyContactPhones[contact] = newValue;
        contact.PhoneNumber              = newValue.Number;
    }

    private Task StartAddressInfoEdit() => StartEditAsync(() =>
        _addressEdit.Begin(new EmployeeCardDto
        {
            LegalAddress   = _profile!.LegalAddress,
            CurrentAddress = _profile.CurrentAddress,
            City           = _profile.City,
            Country        = _profile.Country
        }));

    private Task StartEducationInfoEdit() => StartEditAsync(() =>
        _educationEdit.Begin(_educationHistoryRecords.Select(CloneEducationHistoryRecord).ToList()));

    private void CancelBasicInfoEdit()
    {
        var backup = _basicInfoEdit.Cancel();
        if (backup is not null && _profile is not null)
        {
            _profile.FirstName          = backup.FirstName;
            _profile.LastName           = backup.LastName;
            _profile.RegistrationNumber = backup.RegistrationNumber;
            _profile.UserStartDate      = backup.UserStartDate;
            _profile.PositionStartDate  = backup.PositionStartDate;
            _profile.ContractType       = backup.ContractType;
            _profile.ContractEndDate    = backup.ContractEndDate;
            _profile.WorkModel          = backup.WorkModel;
            _profile.PayrollLocationId  = backup.PayrollLocationId;
        }
    }

    private void CancelContactsEdit()
    {
        var backup = _contactsEdit.Cancel();
        if (backup is not null && _profile is not null)
        {
            _profile.Email               = backup.Email;
            _profile.PhoneNumber         = backup.PhoneNumber;
            _profile.PersonalEmail       = backup.PersonalEmail;
            _profile.PersonalPhoneNumber = backup.PersonalPhoneNumber;
            SyncIntlPhoneInputs();
        }
    }

    private void CancelCitizenshipInfoEdit()
    {
        var backup = _citizenshipEdit.Cancel();
        if (backup is not null && _profile is not null)
        {
            _profile.DateOfBirth       = backup.DateOfBirth;
            _profile.Gender            = backup.Gender;
            _profile.Nationality       = backup.Nationality;
            _profile.CitizenshipNumber = backup.CitizenshipNumber;
            _profile.DisabilityStatus  = backup.DisabilityStatus;
            _profile.DisabilityDegree  = backup.DisabilityDegree;
        }
    }

    private void CancelEmergencyInfoEdit()
    {
        var backup = _emergencyEdit.Cancel();
        if (backup is not null && _profile is not null)
        {
            _profile.EmergencyContacts = backup.EmergencyContacts?
                .Select(c => new EmergencyContactDto
                {
                    Id = c.Id, FullName = c.FullName,
                    Relationship = c.Relationship, PhoneNumber = c.PhoneNumber
                }).ToList() ?? [];
        }
        _emergencyContactPhones.Clear();
    }

    private void AddEmergencyContact()
    {
        if (_profile == null) return;
        _profile.EmergencyContacts ??= [];

        var newContact = new EmergencyContactDto();
        _profile.EmergencyContacts.Add(newContact);
        _emergencyContactPhones[newContact] = new IntlTel();
    }

    private Task StartFamilyInfoEdit() => StartEditAsync(() =>
        _familyEdit.Begin(new EmployeeCardDto
        {
            MaritalStatus  = _profile!.MaritalStatus,
            SpouseFullName = _profile.SpouseFullName,
            Children       = _profile.Children?
                .Select(c => new ChildInfoDto { Id = c.Id, FullName = c.FullName, DateOfBirth = c.DateOfBirth })
                .ToList() ?? []
        }));

    private void AddChild()
    {
        if (_profile == null) return;
        _profile.Children ??= [];
        _profile.Children.Add(new ChildInfoDto { DateOfBirth = null });
    }

    private void RemoveChild(ChildInfoDto child)
    {
        _profile?.Children?.Remove(child);
    }

    private void CancelFamilyInfoEdit()
    {
        var backup = _familyEdit.Cancel();
        if (backup is not null && _profile is not null)
        {
            _profile.MaritalStatus  = backup.MaritalStatus;
            _profile.SpouseFullName = backup.SpouseFullName;
            _profile.Children       = backup.Children?
                .Select(c => new ChildInfoDto { Id = c.Id, FullName = c.FullName, DateOfBirth = c.DateOfBirth })
                .ToList() ?? [];
        }
    }

    private void CancelAddressInfoEdit()
    {
        var backup = _addressEdit.Cancel();
        if (backup is not null && _profile is not null)
        {
            _profile.LegalAddress   = backup.LegalAddress;
            _profile.CurrentAddress = backup.CurrentAddress;
            _profile.City           = backup.City;
            _profile.Country        = backup.Country;
        }
    }

    private void CancelEducationInfoEdit()
    {
        var backup = _educationEdit.Cancel();
        _educationHistoryRecords = backup?.Select(CloneEducationHistoryRecord).ToList() ?? [];
    }

    // ── Save operations ───────────────────────────────────────────────────────
    private async Task SaveBasicInfoAsync()
    {
        if (_profile is null) return;
        var dto = new UpdateBasicInfoDto
        {
            FirstName          = _profile.FirstName,
            LastName           = _profile.LastName,
            RegistrationNumber = _profile.RegistrationNumber,
            UserStartDate      = _profile.UserStartDate,
            PositionStartDate  = _profile.PositionStartDate,
            ContractType       = _profile.ContractType,
            ContractEndDate    = _profile.ContractEndDate,
            WorkModel          = _profile.WorkModel,
            PayrollLocationId  = _profile.PayrollLocationId
        };

        await ExecuteSaveRequestAsync(
            () => UserService.UpdateBasicInfoAsync(EmployeeId, dto),
            "Basic information updated successfully.",
            "Failed to update basic information.",
            refreshProfileOnSuccess: true,
            onSuccess: () => _basicInfoEdit.Commit());
    }

    private async Task SaveContactsAsync()
    {
        if (_profile is null) return;

        var dto = new UpdateContactsDto
        {
            Email               = _profile.Email,
            PhoneNumber         = _workPhoneIntl?.Number,
            PersonalEmail       = _profile.PersonalEmail,
            PersonalPhoneNumber = _personalPhoneIntl?.Number
        };

        if (!IsEmailValidOrEmpty(dto.Email))        { Snackbar.Add("Please enter a valid work email address.", Severity.Error); return; }
        if (!IsEmailValidOrEmpty(dto.PersonalEmail)) { Snackbar.Add("Please enter a valid personal email address.", Severity.Error); return; }

        await ExecuteSaveRequestAsync(
            () => UserService.UpdateContactsAsync(EmployeeId, dto),
            "Contact information updated successfully.",
            "Failed to update contact information.",
            refreshProfileOnSuccess: true,
            onSuccess: () => _contactsEdit.Commit());
    }

    private async Task SaveCitizenshipInfoAsync()
    {
        if (_profile is null) return;
        var dto = new UpdateCitizenshipInfoDto
        {
            DateOfBirth       = _profile.DateOfBirth,
            Gender            = _profile.Gender,
            Nationality       = _profile.Nationality,
            CitizenshipNumber = _profile.CitizenshipNumber,
            DisabilityStatus  = _profile.DisabilityStatus,
            DisabilityDegree  = _profile.DisabilityDegree
        };

        await ExecuteSaveRequestAsync(
            () => UserService.UpdateCitizenshipInfoAsync(EmployeeId, dto),
            "Citizenship information updated successfully.",
            "Failed to update citizenship information.",
            refreshProfileOnSuccess: true,
            onSuccess: () => { _citizenshipEdit.Commit(); });
    }

    private async Task SaveEmergencyInfoAsync()
    {
        if (_profile is null) return;

        // Sync IntlTel values back
        if (_profile.EmergencyContacts != null)
        {
            foreach (var contact in _profile.EmergencyContacts)
            {
                if (_emergencyContactPhones.TryGetValue(contact, out var intl))
                    contact.PhoneNumber = intl.Number;
            }
        }

        var dto = new UpdateEmergencyContactsDto
        {
            EmergencyContacts = _profile.EmergencyContacts?
                .Select(c => new EmergencyContactDto
                {
                    Id = c.Id, FullName = c.FullName,
                    Relationship = c.Relationship, PhoneNumber = c.PhoneNumber
                }).ToList() ?? []
        };

        await ExecuteSaveRequestAsync(
            () => UserService.UpdateEmergencyContactsAsync(EmployeeId, dto),
            "Emergency contact updated successfully.",
            "Failed to update emergency contact.",
            refreshProfileOnSuccess: true,
            onSuccess: () => { _emergencyEdit.Commit(); _emergencyContactPhones.Clear(); });
    }

    private async Task SaveAddressInfoAsync()
    {
        if (_profile is null) return;
        var dto = new UpdateAddressInfoDto
        {
            LegalAddress   = _profile.LegalAddress,
            CurrentAddress = _profile.CurrentAddress,
            City           = _profile.City,
            Country        = _profile.Country
        };

        await ExecuteSaveRequestAsync(
            () => UserService.UpdateAddressInfoAsync(EmployeeId, dto),
            "Address information updated successfully.",
            "Failed to update address information.",
            refreshProfileOnSuccess: true,
            onSuccess: () => { _addressEdit.Commit(); });
    }

    private async Task SaveEducationInfoAsync()
    {
        if (_profile is null) return;

        var dto = new UpdateEducationInfoDto
        {
            EducationHistories = _educationHistoryRecords.Select(r => new EducationHistoryDto
            {
                Id = r.Id ?? 0, Level = r.Level, Institution = r.Institution,
                Program = r.Program, GraduationDate = r.GraduationDate, Status = r.Status
            }).ToList()
        };

        await ExecuteSaveRequestAsync(
            () => UserService.UpdateEducationInfoAsync(EmployeeId, dto),
            "Education information updated successfully.",
            "Failed to update education information.",
            refreshProfileOnSuccess: true,
            onSuccess: () => { _educationEdit.Commit(); });
    }

    private async Task SaveFamilyInfoAsync()
    {
        if (_profile is null) return;
        var dto = new UpdateFamilyInfoDto
        {
            MaritalStatus  = _profile.MaritalStatus,
            SpouseFullName = _profile.SpouseFullName,
            Children       = _profile.Children?
                .Select(c => new ChildInfoDto { Id = c.Id, FullName = c.FullName, DateOfBirth = c.DateOfBirth })
                .ToList() ?? []
        };

        await ExecuteSaveRequestAsync(
            () => UserService.UpdateFamilyInfoAsync(EmployeeId, dto),
            "Family information updated successfully.",
            "Failed to update family information.",
            refreshProfileOnSuccess: true,
            onSuccess: () => { _familyEdit.Commit(); });
    }

    // ── Education dialog helpers ───────────────────────────────────────────────
    private async Task OpenAddEducationHistoryDialogAsync()
    {
        var options = SmallEscDialog;
        var dialog  = await DialogService.ShowAsync<AddEducationHistoryDialog>("Add Education History", new DialogParameters(), options);
        var result = await dialog.Result;

        if (result is null || result.Canceled || result.Data is not AddEducationHistoryDialog.AddEducationHistoryDialogResult data) return;

        _educationHistoryRecords.Add(new EducationHistoryRecord
        {
            Level = data.Level, Institution = data.Institution,
            Program = data.Program, GraduationDate = data.GraduationDate, Status = data.Status
        });
    }

    private async Task EditEducationHistoryRecordAsync(EducationHistoryRecord record)
    {
        if (!_educationEdit.IsEditing) await StartEducationInfoEdit();

        var parameters = new DialogParameters
        {
            [nameof(AddEducationHistoryDialog.InitialModel)] = new AddEducationHistoryDialog.AddEducationHistoryDialogResult
            {
                Level = record.Level, Institution = record.Institution,
                Program = record.Program, GraduationDate = record.GraduationDate,
                Status = record.Status ?? EducationStatuses.Completed
            },
            [nameof(AddEducationHistoryDialog.ConfirmButtonText)] = "Update"
        };
        var options = SmallEscDialog;

        var dialog = await DialogService.ShowAsync<AddEducationHistoryDialog>("Edit Education History", parameters, options);
        var result = await dialog.Result;

        if (result is null || result.Canceled || result.Data is not AddEducationHistoryDialog.AddEducationHistoryDialogResult data) return;

        record.Level          = data.Level;
        record.Institution    = data.Institution;
        record.Program        = data.Program;
        record.GraduationDate = data.GraduationDate;
        record.Status         = data.Status;
    }

    private async Task RemoveEducationHistoryRecordAsync(EducationHistoryRecord record)
    {
        if (!_educationEdit.IsEditing) await StartEducationInfoEdit();
        _educationHistoryRecords.Remove(record);
    }

    private static EducationHistoryRecord CloneEducationHistoryRecord(EducationHistoryRecord source) => new()
    {
        Id = source.Id, Level = source.Level, Institution = source.Institution,
        Program = source.Program, GraduationDate = source.GraduationDate, Status = source.Status
    };

    private static Color GetEducationStatusColor(string? status)
    {
        if (status is null) return Color.Default;
        if (status.Equals(EducationStatuses.Completed,  StringComparison.OrdinalIgnoreCase)) return Color.Success;
        if (status.Equals(EducationStatuses.InProgress, StringComparison.OrdinalIgnoreCase)) return Color.Warning;
        if (status.Equals(EducationStatuses.Planned,    StringComparison.OrdinalIgnoreCase)) return Color.Info;
        return Color.Default;
    }

    private static Color GetEducationLevelColor(string? level)
    {
        if (level is null) return Color.Default;
        if (level.Equals(EducationLevels.HighSchool, StringComparison.OrdinalIgnoreCase)) return Color.Success;
        if (level.Equals(EducationLevels.Associate,  StringComparison.OrdinalIgnoreCase)) return Color.Info;
        if (level.Equals(EducationLevels.Bachelors,  StringComparison.OrdinalIgnoreCase)) return Color.Primary;
        if (level.Equals(EducationLevels.Masters,    StringComparison.OrdinalIgnoreCase)) return Color.Secondary;
        if (level.Equals(EducationLevels.Phd,        StringComparison.OrdinalIgnoreCase)) return Color.Error;
        return Color.Default;
    }

    // ── Career tab ─────────────────────────────────────────────────────────────
    private void SetView(int index)
    {
        _activeViewIndex = index;
        if (index == 2 && !_career.IsLoaded)
            _ = LoadCareerDataAsync();
    }

    private async Task LoadCareerDataAsync()
    {
        try
        {
            // All 5 calls independent — run in parallel
            var tCompanies  = OrgService.GetCompaniesAsync();
            var tDepts      = OrgService.GetDepartmentsAsync();
            var tTeams      = OrgService.GetTeamsAsync();
            var tJobs       = OrgService.GetJobsAsync();
            var tPaths      = OrgService.GetCareerPathsAsync();

            await Task.WhenAll(tCompanies, tDepts, tTeams, tJobs, tPaths);

            // Tasks already completed — .Result is safe and avoids redundant async overhead
            _career.Companies   = tCompanies.Result;
            _career.AllDepts    = tDepts.Result;
            _career.AllTeams    = tTeams.Result;
            _career.AllJobs     = tJobs.Result;
            _career.CareerPaths = tPaths.Result;
            _career.MarkLoaded();

            _career.CompanyName = _profile?.CompanyName;
            _career.DeptId      = _profile?.DepartmentId;
            _career.TeamId      = _profile?.TeamId;
            _career.PathId      = _profile?.CareerPathId;
            _career.JobId       = _profile?.JobId;
            _career.Grade       = _profile?.Grade;

            _career.FilteredDepts = FilteredDeptsForCompany(_career.CompanyName);
            _career.FilteredTeams = FilteredTeamsForDept(_career.DeptId);

            if (_career.PathId.HasValue)
            {
                var path = _career.CareerPaths.FirstOrDefault(p => p.Id == _career.PathId);
                if (path != null)
                {
                    _career.PathJobs = path.Rules.SelectMany(r => r.PositionJobs).DistinctBy(j => j.Id).ToList();
                    if (_career.JobId.HasValue)
                        _career.JobGrades = path.Rules
                            .Where(r => r.PositionJobIds.Contains(_career.JobId.Value))
                            .Select(r => r.Grade).OrderBy(g => g).ToList();
                }
            }

            StateHasChanged();
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load career data. Please try again.", Severity.Error);
        }
    }

    private void OnCareerCompanyChanged(string? name)
    {
        _career.CompanyName   = name;
        _career.DeptId        = null;
        _career.TeamId        = null;
        _career.FilteredDepts = FilteredDeptsForCompany(name);
        _career.FilteredTeams = [];
    }

    private void OnCareerDeptChanged(int? deptId)
    {
        _career.DeptId        = deptId;
        _career.TeamId        = null;
        _career.FilteredTeams = FilteredTeamsForDept(deptId);
    }

    private void OnCareerPathChanged(int? pathId)
    {
        _career.PathId    = pathId;
        _career.JobId     = null;
        _career.Grade     = null;
        _career.JobGrades = [];

        var path = _career.CareerPaths.FirstOrDefault(p => p.Id == pathId);
        _career.PathJobs  = path is null ? [] : path.Rules.SelectMany(r => r.PositionJobs).DistinctBy(j => j.Id).ToList();
    }

    private void OnCareerJobChanged(int? jobId)
    {
        _career.JobId = jobId;
        _career.Grade = null;

        var path = _career.CareerPaths.FirstOrDefault(p => p.Id == _career.PathId);
        _career.JobGrades = (path is null || !jobId.HasValue)
            ? []
            : path.Rules.Where(r => r.PositionJobIds.Contains(jobId.Value))
                .Select(r => r.Grade).OrderBy(g => g).ToList();

        if (_career.JobGrades.Count == 1) _career.Grade = _career.JobGrades[0];
    }

    private void CancelCareerEdit()
    {
        _career.CancelEdit();
        ApplyProfileToCareerTab();
    }

    private bool _careerSaving;

    private async Task SaveCareerAssignmentAsync()
    {
        if (EmployeeId <= 0 || _careerSaving) return;
        _careerSaving = true;
        try
        {
            var dto = new UpdateCareerAssignmentDto
            {
                CompanyName  = _career.CompanyName,
                DepartmentId = _career.DeptId,
                TeamId       = _career.TeamId,
                CareerPathId = _career.PathId,
                JobId        = _career.JobId,
                Grade        = _career.Grade,
            };

            var resp = await UserService.UpdateCareerAssignmentAsync(EmployeeId, dto);
            if (resp.IsSuccessStatusCode)
            {
                // Refresh only the profile (card + sensitive-info) — reference data (companies/depts/jobs/
                // career paths) is unchanged and does not need reloading. This replaces the previous
                // LoadData() + LoadCareerDataAsync() double-reload (9 API calls → 2 API calls).
                await RefreshProfileAsync();
                ApplyProfileToCareerTab();
                Snackbar.Add("Career assignment saved.", Severity.Success);
            }
            else
            {
                Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? "Failed to save career assignment.", Severity.Error);
            }
        }
        finally { _careerSaving = false; }
    }

    // ── Career helpers ────────────────────────────────────────────────────────

    /// <summary>
    /// Applies the current <see cref="_profile"/> values to the career tab state and
    /// recomputes the filtered department/team lists from in-memory reference data.
    /// Used after a career-assignment save to avoid reloading unchanged reference data.
    /// </summary>
    private void ApplyProfileToCareerTab()
    {
        _career.CompanyName   = _profile?.CompanyName;
        _career.DeptId        = _profile?.DepartmentId;
        _career.TeamId        = _profile?.TeamId;
        _career.PathId        = _profile?.CareerPathId;
        _career.JobId         = _profile?.JobId;
        _career.Grade         = _profile?.Grade;
        _career.FilteredDepts = FilteredDeptsForCompany(_career.CompanyName);
        _career.FilteredTeams = FilteredTeamsForDept(_career.DeptId);

        var path = _career.PathId.HasValue
            ? _career.CareerPaths.FirstOrDefault(p => p.Id == _career.PathId)
            : null;

        _career.PathJobs  = path is null ? [] : path.Rules.SelectMany(r => r.PositionJobs).DistinctBy(j => j.Id).ToList();
        _career.JobGrades = (path is null || !_career.JobId.HasValue)
            ? []
            : path.Rules.Where(r => r.PositionJobIds.Contains(_career.JobId.Value))
                .Select(r => r.Grade).OrderBy(g => g).ToList();
    }

    /// <summary>Returns departments that belong to the company with the given <paramref name="companyName"/>.</summary>
    private List<DepartmentDto> FilteredDeptsForCompany(string? companyName)
        => string.IsNullOrEmpty(companyName)
            ? []
            : _career.AllDepts
                .Where(d => _career.Companies.Any(c => c.Name == companyName && c.Id == d.CompanyId))
                .ToList();

    /// <summary>Returns teams that belong to the department with the given <paramref name="deptId"/>.</summary>
    private List<TeamDto> FilteredTeamsForDept(int? deptId)
        => deptId.HasValue
            ? _career.AllTeams.Where(t => t.DepartmentId == deptId).ToList()
            : [];

    // ── Utilities ─────────────────────────────────────────────────────────────
    private void RemoveEmergencyContact(EmergencyContactDto contact)
    {
        _profile?.EmergencyContacts?.Remove(contact);
        _emergencyContactPhones.Remove(contact);
    }

    private async Task ExecuteSaveRequestAsync(
        Func<Task<HttpResponseMessage>> request,
        string successMessage,
        string failureMessage,
        bool refreshProfileOnSuccess = false,
        Action? onSuccess = null)
    {
        try
        {
            var response = await request();
            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await ApiServiceBase.TryReadProblemDetailAsync(response);
                Snackbar.Add(errorMsg ?? failureMessage, Severity.Error);
                return;
            }

            onSuccess?.Invoke();
            if (refreshProfileOnSuccess) await RefreshProfileAsync();
            await InvokeAsync(StateHasChanged);
            Snackbar.Add(successMessage, Severity.Success);
        }
        catch (Exception)
        {
            Snackbar.Add("An unexpected error occurred. Please try again.", Severity.Error);
        }
    }

    private static bool IsEmailValidOrEmpty(string? email)
        => string.IsNullOrWhiteSpace(email) || AppFormatter.IsValidEmail(email);

    private async Task CopyToClipboard(string? text)
    {
        if (string.IsNullOrEmpty(text)) return;
        try
        {
            await JS.InvokeVoidAsync("navigator.clipboard.writeText", text);
            Snackbar.Add("Copied to clipboard", Severity.Normal);
        }
        catch
        {
            Snackbar.Add("Failed to copy to clipboard", Severity.Error);
        }
    }

    private void SyncIntlPhoneInputs()
    {
        _workPhoneIntl     = new IntlTel { Number = _profile?.PhoneNumber };
        _personalPhoneIntl = new IntlTel { Number = _profile?.PersonalPhoneNumber };
    }

    private string? GetWorkPhoneValue()     => _profile?.PhoneNumber;
    private string? GetPersonalPhoneValue() => _profile?.PersonalPhoneNumber;
    private string GetWorkPhoneDisplay()     => GetWorkPhoneValue() ?? "-";
    private string GetPersonalPhoneDisplay() => GetPersonalPhoneValue() ?? "-";

    private static string? FirstNonEmpty(params string?[] values)
    {
        foreach (var v in values)
            if (!string.IsNullOrWhiteSpace(v)) return v;
        return null;
    }

    private IntlTel GetEmergencyPhone(EmergencyContactDto contact)
    {
        if (!_emergencyContactPhones.TryGetValue(contact, out var intl))
        {
            intl = new IntlTel { Number = contact.PhoneNumber };
            _emergencyContactPhones[contact] = intl;
        }
        return intl;
    }

    private void Back() => Nav.NavigateTo("/employees");
    private string GetManagerDisplayName()
    {
        if (_profile?.ReportsTo is null) return "-";
        var name = AppFormatter.BuildFullName(_profile.ReportsTo.FirstName, _profile.ReportsTo.LastName);
        return string.IsNullOrWhiteSpace(name) ? "-" : name;
    }
    private string GetManagerDepartmentName() => _profile?.ReportsTo?.Department?.Name ?? "-";

    // ── Nested types ──────────────────────────────────────────────────────────
    private sealed class EducationHistoryRecord
    {
        public int?      Id             { get; set; }
        public string    Level          { get; set; } = string.Empty;
        public string    Institution    { get; set; } = string.Empty;
        public string?   Program        { get; set; }
        public DateTime? GraduationDate { get; set; }
        public string?   Status         { get; set; }
    }
}
