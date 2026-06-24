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
    [Inject] private IPaymentClientService PaymentService { get; set; } = default!;
    [Inject] private IUserAvatarState AvatarState { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;

    private MyPaymentSummaryDto? _myPaymentSummary;
    private bool _myPaymentSummaryLoading;

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
            _error = "Employee not selected. Please open a profile from the Employees page.";
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
            if (_profile is null) { _error = "Could not load your profile."; return; }
            _userId = _profile.Id;

            try { MergeSensitiveInfo(_profile, await UserService.GetMySensitiveInfoAsync()); } catch { }

            _fullName           = AppFormatter.BuildFullName(_profile.FirstName, _profile.LastName);
            _employmentDuration = AppFormatter.FormatDurationFrom(_profile.UserStartDate ?? _profile.PositionStartDate);

            try
            {
                if (_profile.Avatar is { ContentBase64: { Length: > 0 } b64 })
                    _avatarDataUrl = $"data:{_profile.Avatar.ContentType ?? "image/png"};base64,{b64}";
                else
                {
                    var avatar = await UserService.GetMyAvatarAsync();
                    if (avatar is { ContentBase64: { Length: > 0 } ab64 })
                        _avatarDataUrl = $"data:{avatar.ContentType ?? "image/png"};base64,{ab64}";
                }
            }
            catch { }

            SyncIntlPhoneInputs();
            _educationHistoryRecords = MapEducationHistories(_profile);
            _positionHistory = MapPositionHistory(_profile);

            _ = LoadMyPaymentSummaryAsync();
        }
        catch (Exception)
        {
            _error = "Failed to load your profile.";
            Snackbar.Add(_error, Severity.Error);
        }
        finally { _loading = false; }
    }

    private async Task LoadMyPaymentSummaryAsync()
    {
        _myPaymentSummaryLoading = true;
        try { _myPaymentSummary = await PaymentService.GetMyPaymentSummaryAsync(); }
        catch { _myPaymentSummary = null; }
        finally
        {
            _myPaymentSummaryLoading = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task LoadData()
    {
        _loading = true;
        _error   = null;

        try
        {
            _profile = await UserService.GetEmployeeCardBySlugAsync(Slug!);
            if (_profile is null) { _error = "User profile not found."; return; }
            _userId = _profile.Id;

            var sensitiveInfoTask = UserService.GetSensitiveInfoAsync(_userId);
            var avatarTask        = UserService.GetUserAvatarAsync(_userId);
            await Task.WhenAll(sensitiveInfoTask, avatarTask);

            try { _payrollLocations = await OrgService.GetPayrollLocationsAsync(); } catch { }

            MergeSensitiveInfo(_profile, await sensitiveInfoTask);

            _fullName           = AppFormatter.BuildFullName(_profile.FirstName, _profile.LastName);
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
            catch { }

            SyncIntlPhoneInputs();
            _educationHistoryRecords = MapEducationHistories(_profile);
            _positionHistory = MapPositionHistory(_profile);
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
        if (_userId <= 0) return;

        var cardTask     = SelfService ? UserService.GetMyCardAsync()         : UserService.GetEmployeeCardAsync(_userId);
        var piiTask      = SelfService ? UserService.GetMySensitiveInfoAsync() : UserService.GetSensitiveInfoAsync(_userId);
        await Task.WhenAll(cardTask, piiTask);

        var latestProfile = await cardTask;
        if (latestProfile is null) return;

        MergeSensitiveInfo(latestProfile, await piiTask);
        _profile = latestProfile;

        _fullName           = AppFormatter.BuildFullName(_profile.FirstName, _profile.LastName);
        _employmentDuration = AppFormatter.FormatDurationFrom(_profile.UserStartDate ?? _profile.PositionStartDate);

        if (_profile.Avatar is { ContentBase64: { Length: > 0 } b64 })
            _avatarDataUrl = $"data:{_profile.Avatar.ContentType ?? "image/png"};base64,{b64}";

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
                Level          = x.Level ?? string.Empty,
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

    private bool _isEditingActionInProgress;

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
            LegalAddress        = _profile!.LegalAddress,
            LegalAddressCity    = _profile.LegalAddressCity,
            LegalAddressCountry = _profile.LegalAddressCountry,
            CurrentAddress      = _profile.CurrentAddress,
            City                = _profile.City,
            Country             = _profile.Country
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
            _profile.LegalAddress        = backup.LegalAddress;
            _profile.LegalAddressCity    = backup.LegalAddressCity;
            _profile.LegalAddressCountry = backup.LegalAddressCountry;
            _profile.CurrentAddress      = backup.CurrentAddress;
            _profile.City                = backup.City;
            _profile.Country             = backup.Country;
        }
    }

    private void CancelEducationInfoEdit()
    {
        var backup = _educationEdit.Cancel();
        _educationHistoryRecords = backup?.Select(CloneEducationHistoryRecord).ToList() ?? [];
    }

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
            () => UserService.UpdateBasicInfoAsync(_userId, dto),
            "Basic information updated successfully.",
            "Failed to update basic information.",
            refreshProfileOnSuccess: true,
            onSuccess: () => _basicInfoEdit.Commit());

        if (_profile is not null && !string.Equals(_profile.Slug, Slug, StringComparison.OrdinalIgnoreCase))
            Nav.NavigateTo($"/employee/{_profile.Slug}?tab=personal", forceLoad: true);
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

        Func<Task<HttpResponseMessage>> request = SelfService
            ? () => UserService.UpdateMyContactInfoAsync(new UpdateContactInfoDto
                {
                    WorkPhoneNumber     = dto.PhoneNumber,
                    PersonalEmail       = dto.PersonalEmail,
                    PersonalPhoneNumber = dto.PersonalPhoneNumber
                })
            : () => UserService.UpdateContactsAsync(_userId, dto);

        await ExecuteSaveRequestAsync(
            request,
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
            () => UserService.UpdateCitizenshipInfoAsync(_userId, dto),
            "Citizenship information updated successfully.",
            "Failed to update citizenship information.",
            refreshProfileOnSuccess: true,
            onSuccess: () => { _citizenshipEdit.Commit(); });
    }

    private async Task SaveEmergencyInfoAsync()
    {
        if (_profile is null) return;

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
            SelfService
                ? () => UserService.UpdateMyEmergencyContactsAsync(dto)
                : () => UserService.UpdateEmergencyContactsAsync(_userId, dto),
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
            LegalAddress        = _profile.LegalAddress,
            LegalAddressCity    = _profile.LegalAddressCity,
            LegalAddressCountry = _profile.LegalAddressCountry,
            CurrentAddress      = _profile.CurrentAddress,
            City                = _profile.City,
            Country             = _profile.Country
        };

        await ExecuteSaveRequestAsync(
            () => UserService.UpdateAddressInfoAsync(_userId, dto),
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
            () => UserService.UpdateEducationInfoAsync(_userId, dto),
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
            SelfService
                ? () => UserService.UpdateMyFamilyInfoAsync(dto)
                : () => UserService.UpdateFamilyInfoAsync(_userId, dto),
            "Family information updated successfully.",
            "Failed to update family information.",
            refreshProfileOnSuccess: true,
            onSuccess: () => { _familyEdit.Commit(); });
    }

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

    private void SetView(int index)
    {
        _activeViewIndex = index;
        if (!SelfService && index == 2 && !_career.IsLoaded)
            _ = LoadCareerDataAsync();
        if (SelfService && index == 3 && _myPaymentSummary is null)
            _ = LoadMyPaymentSummaryAsync();
    }

    private async Task LoadCareerDataAsync()
    {
        try
        {
            var tCompanies  = OrgService.GetCompaniesAsync();
            var tDepts      = OrgService.GetDepartmentsAsync();
            var tTeams      = OrgService.GetTeamsAsync();
            var tJobs       = OrgService.GetJobsAsync();
            var tPaths      = OrgService.GetCareerPathsAsync();

            await Task.WhenAll(tCompanies, tDepts, tTeams, tJobs, tPaths);

            _career.Companies   = tCompanies.Result;
            _career.AllDepts    = tDepts.Result;
            _career.AllTeams    = tTeams.Result;
            _career.AllJobs     = tJobs.Result;
            _career.CareerPaths = tPaths.Result;
            _career.MarkLoaded();
            _ = LoadCvDocumentsAsync();

            _career.CompanyName = _profile?.CompanyName;
            _career.DeptId      = _profile?.DepartmentId;
            _career.TeamId      = _profile?.TeamId;
            _career.PathId      = _profile?.CareerPathId;
            _career.JobId       = _profile?.JobId;
            _career.Grade       = _profile?.Grade;

            _career.FilteredDepts = FilteredDeptsForCompany(_career.CompanyName);
            _career.FilteredTeams = FilteredTeamsForDept(_career.DeptId);
            PrefillManagerFromProfile();

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

    private void PrefillManagerFromProfile()
    {
        _career.SelectedManager = _profile?.ReportsTo is { } r
            ? new UserSearchResultDto
            {
                Id               = r.Id,
                FirstName        = r.FirstName,
                LastName         = r.LastName,
                AvatarColorIndex = r.AvatarColorIndex,
                AvatarContentType = r.Avatar?.ContentType,
                AvatarBase64      = r.Avatar?.ContentBase64,
            }
            : null;
    }

    private bool _careerSaving;


    private async Task OpenCareerAssignmentDialogAsync(PositionHistoryDto? current)
    {
        if (_userId <= 0) return;

        var initial = new CareerAssignmentDialog.CareerAssignmentResult
        {
            ManagerId    = _career.SelectedManager?.Id,
            Manager      = _career.SelectedManager,
            CompanyName  = _profile?.CompanyName,
            DepartmentId = _profile?.DepartmentId,
            TeamId       = _profile?.TeamId,
            PathId       = _profile?.CareerPathId,
            JobId        = _profile?.JobId,
            Grade        = _profile?.Grade,
            StartDate    = current?.StartDate ?? _profile?.PositionStartDate ?? DateTime.Today,
            EndDate      = current?.EndDate
        };

        var parameters = new DialogParameters
        {
            [nameof(CareerAssignmentDialog.Companies)]     = _career.Companies,
            [nameof(CareerAssignmentDialog.AllDepts)]      = _career.AllDepts,
            [nameof(CareerAssignmentDialog.AllTeams)]      = _career.AllTeams,
            [nameof(CareerAssignmentDialog.CareerPaths)]   = _career.CareerPaths,
            [nameof(CareerAssignmentDialog.ExcludeUserId)] = _userId,
            [nameof(CareerAssignmentDialog.InitialModel)]  = initial
        };
        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        var dialog  = await DialogService.ShowAsync<CareerAssignmentDialog>(
            current is null ? "Assign Position" : "Edit Career Assignment", parameters, options);
        var result = await dialog.Result;
        if (result is null || result.Canceled || result.Data is not CareerAssignmentDialog.CareerAssignmentResult data) return;

        await SaveAssignmentAsync(data);
    }

    private async Task SaveAssignmentAsync(CareerAssignmentDialog.CareerAssignmentResult data)
    {
        if (_careerSaving) return;

        var manager     = data.Manager;
        var needNewNode = manager is { OrganizationPositionId: not null } && _profile?.OrganizationPositionId is null;
        string? newPositionName = null;
        if (needNewNode)
        {
            var p = new DialogParameters
            {
                [nameof(PositionNameDialog.ManagerName)] = AppFormatter.BuildFullName(manager!.FirstName, manager.LastName)
            };
            var o  = new DialogOptions { MaxWidth = MaxWidth.ExtraSmall, FullWidth = true, CloseOnEscapeKey = true };
            var pd = await DialogService.ShowAsync<PositionNameDialog>("New Position", p, o);
            var pr = await pd.Result;
            if (pr is not { Canceled: false, Data: string name } || string.IsNullOrWhiteSpace(name)) return;
            newPositionName = name;
        }

        _careerSaving = true;
        try
        {
            var dto = new UpdateCareerAssignmentDto
            {
                CompanyName     = data.CompanyName,
                DepartmentId    = data.DepartmentId,
                TeamId          = data.TeamId,
                CareerPathId    = data.PathId,
                JobId           = data.JobId,
                Grade           = data.Grade,
                ManagerId       = data.ManagerId,
                NewPositionName = newPositionName
            };
            var resp = await UserService.UpdateCareerAssignmentAsync(_userId, dto);
            if (!resp.IsSuccessStatusCode)
            {
                Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? "Failed to save assignment.", Severity.Error);
                return;
            }

            var posDto = new UpdatePositionHistoryDto
            {
                CompanyName  = data.CompanyName,
                DepartmentId = data.DepartmentId,
                TeamId       = data.TeamId,
                StartDate    = data.StartDate ?? DateTime.Today,
                EndDate      = data.EndDate
            };
            await UserService.UpdateCurrentPositionAsync(_userId, posDto);

            await RefreshProfileAsync();
            ApplyProfileToCareerTab();
            Snackbar.Add("Career assignment saved.", Severity.Success);
        }
        finally { _careerSaving = false; }
    }

    private async Task OpenPromotionDialogAsync()
    {
        if (_userId <= 0) return;

        var path = _profile?.CareerPathId is int id ? _career.CareerPaths.FirstOrDefault(p => p.Id == id) : null;
        if (path is null)
        {
            Snackbar.Add("Assign a career path first.", Severity.Warning);
            return;
        }

        var parameters = new DialogParameters
        {
            [nameof(PromotionDialog.Path)]  = path,
            [nameof(PromotionDialog.JobId)] = _profile?.JobId,
            [nameof(PromotionDialog.Grade)] = _profile?.Grade
        };
        var dialog = await DialogService.ShowAsync<PromotionDialog>("Promotion", parameters, SmallEscDialog);
        var result = await dialog.Result;
        if (result is null || result.Canceled || result.Data is not PromotionDialog.PromotionResult data) return;

        if (_careerSaving) return;
        _careerSaving = true;
        try
        {
            var dto = new UpdateCareerAssignmentDto
            {
                CompanyName  = _profile?.CompanyName,
                DepartmentId = _profile?.DepartmentId,
                TeamId       = _profile?.TeamId,
                CareerPathId = _profile?.CareerPathId,
                JobId        = data.JobId,
                Grade        = data.Grade
            };
            var resp = await UserService.UpdateCareerAssignmentAsync(_userId, dto);
            if (resp.IsSuccessStatusCode)
            {
                await RefreshProfileAsync();
                ApplyProfileToCareerTab();
                Snackbar.Add("Employee promoted.", Severity.Success);
            }
            else
            {
                Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? "Promotion failed.", Severity.Error);
            }
        }
        finally { _careerSaving = false; }
    }


    private async Task LoadCvDocumentsAsync()
    {
        try { _cvDocuments = await UserService.GetUserDocumentsAsync(_userId); }
        catch { _cvDocuments = []; }
        await InvokeAsync(StateHasChanged);
    }

    private async Task OnCvFileSelected(InputFileChangeEventArgs e)
    {
        if (_userId <= 0 || e.FileCount == 0 || e.File is not { } file) return;

        if (file.Size > DocumentConstants.MaxFileSizeBytes)
        {
            Snackbar.Add($"File size cannot exceed {DocumentConstants.MaxFileSizeBytes / 1024 / 1024} MB.", Severity.Warning);
            return;
        }
        if (!DocumentConstants.AllowedMimeTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
        {
            Snackbar.Add("Unsupported file type. Allowed: PDF, Word, Excel, PNG, JPG.", Severity.Warning);
            return;
        }

        _cvUploading = true;
        try
        {
            await using var stream = file.OpenReadStream(DocumentConstants.MaxFileSizeBytes);
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);

            using var content     = new MultipartFormDataContent();
            using var fileContent = new ByteArrayContent(ms.ToArray());
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "file", file.Name);

            var resp = await UserService.UploadUserDocumentAsync(_userId, content);
            if (resp.IsSuccessStatusCode)
            {
                await LoadCvDocumentsAsync();
                Snackbar.Add("Document uploaded.", Severity.Success);
            }
            else
            {
                Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? "Upload failed.", Severity.Error);
            }
        }
        catch
        {
            Snackbar.Add("Upload failed. Please try again.", Severity.Error);
        }
        finally { _cvUploading = false; }
    }

    private async Task DownloadDocumentAsync(UserDocumentDto doc)
    {
        try
        {
            var bytes = await UserService.DownloadUserDocumentAsync(_userId, doc.Id);
            if (bytes is null) { Snackbar.Add("Download failed.", Severity.Error); return; }
            await JS.InvokeVoidAsync("dzdDownloadFile", doc.FileName, Convert.ToBase64String(bytes), doc.ContentType);
        }
        catch { Snackbar.Add("Download failed.", Severity.Error); }
    }

    private async Task DeleteDocumentAsync(UserDocumentDto doc)
    {
        var resp = await UserService.DeleteUserDocumentAsync(_userId, doc.Id);
        if (resp.IsSuccessStatusCode)
        {
            await LoadCvDocumentsAsync();
            Snackbar.Add("Document deleted.", Severity.Success);
        }
        else
        {
            Snackbar.Add("Delete failed.", Severity.Error);
        }
    }

    private static string FormatFileSize(long bytes)
        => bytes >= 1024 * 1024 ? $"{bytes / 1024.0 / 1024.0:0.#} MB"
         : bytes >= 1024 ? $"{bytes / 1024.0:0.#} KB"
         : $"{bytes} B";


    private CareerPathDto? AssignedPath()
        => _profile?.CareerPathId is int id ? _career.CareerPaths.FirstOrDefault(p => p.Id == id) : null;

    private List<CareerMapRuleDto> PathRulesOrdered()
        => AssignedPath()?.Rules.OrderBy(r => r.Grade).ToList() ?? [];

    private CareerMapRuleDto? NextGradeRule()
    {
        var rules = PathRulesOrdered();
        var grade = _profile?.Grade;
        return grade is null ? rules.FirstOrDefault() : rules.FirstOrDefault(r => r.Grade > grade);
    }

    private static string FormatPositionRange(PositionHistoryDto p)
        => $"{p.StartDate:MMM d, yyyy} - {(p.EndDate is { } e ? e.ToString("MMM d, yyyy") : "Present")}";

    private static string FormatRoleDuration(RoleDurationDto d)
        => d.Years is > 0 and var y ? $"{y} year{(y == 1 ? "" : "s")}"
         : d.Months is > 0 and var m ? $"{m} month{(m == 1 ? "" : "s")}"
         : "-";


    internal enum ReqStatus { Completed, InProgress, Pending }

    internal sealed record GradeRequirement(
        string Icon,
        string IconBg,
        string IconColor,
        string Name,
        string Required,
        string CurrentStatus,
        int Progress,        ReqStatus Status);

    private static int ToMonths(RoleDurationDto d) => d.Years.GetValueOrDefault() * 12 + d.Months.GetValueOrDefault();

    private static int MonthsSince(DateTime? start)
    {
        if (start is null) return 0;
        var now = DateTime.UtcNow.Date;
        var total = (now.Year - start.Value.Year) * 12 + (now.Month - start.Value.Month);
        if (now.Day < start.Value.Day) total--;
        return Math.Max(0, total);
    }

    private CareerMapRuleDto? CurrentGradeRule()
    {
        var rules = PathRulesOrdered();
        var grade = _profile?.Grade;
        return grade is null ? rules.FirstOrDefault() : rules.FirstOrDefault(r => r.Grade == grade);
    }

    private List<GradeRequirement> NextGradeRequirements()
    {
        if (CurrentGradeRule() is not { } next) return [];

        var rows = new List<GradeRequirement>();

        static GradeRequirement Duration(string icon, string bg, string fg, string name,
            RoleDurationDto required, int actualMonths, string currentStatus)
        {
            var requiredMonths = ToMonths(required);
            var progress = requiredMonths <= 0 ? 100 : Math.Clamp(actualMonths * 100 / requiredMonths, 0, 100);
            var status = progress >= 100 ? ReqStatus.Completed : actualMonths > 0 ? ReqStatus.InProgress : ReqStatus.Pending;
            return new(icon, bg, fg, name, FormatRoleDuration(required), currentStatus, progress, status);
        }

        if (ToMonths(next.MinExperience) > 0)
            rows.Add(Duration(DzdIcons.Clock, "#E8EBFF", "#2B38F5", "Minimum Experience",
                next.MinExperience, MonthsSince(_profile?.UserStartDate),
                AppFormatter.FormatDurationFrom(_profile?.UserStartDate) ?? "-"));

        if (ToMonths(next.MinRoleTime) > 0)
            rows.Add(Duration(DzdIcons.CalendarCheck, "#FFF1E8", "#F46036", "Time in Current Role",
                next.MinRoleTime, MonthsSince(_profile?.PositionStartDate),
                AppFormatter.FormatDurationFrom(_profile?.PositionStartDate) ?? "-"));

        if (next.ProjectObjective is int po and > 0)
            rows.Add(new(DzdIcons.Target, "#E3F0EE", "#70A37F", "Project Goal Achievement",
                $"{po} project{(po == 1 ? "" : "s")}", "Pending review", 0, ReqStatus.Pending));

        if (next.ManagerPerformanceEvaluation)
            rows.Add(new(DzdIcons.UserRound, "#FFF8E8", "#FEA82F", "Manager Performance Rating",
                "Required", "Pending review", 0, ReqStatus.Pending));

        if (next.TechnicalInterview)
            rows.Add(new(DzdIcons.ShieldCheck, "#E8EBFF", "#2B38F5", "Technical Interview",
                "Required", "Pending review", 0, ReqStatus.Pending));

        if (next.CaseStudy)
            rows.Add(new(DzdIcons.FileText, "#E8EBFF", "#2B38F5", "Case Study",
                "Required", "Pending review", 0, ReqStatus.Pending));

        if (next.EnglishProficiency)
            rows.Add(new(DzdIcons.Languages, "#E3F0EE", "#70A37F", "English Proficiency",
                "Required", "Pending review", 0, ReqStatus.Pending));

        if (next.AssessmentCenterApplication)
            rows.Add(new(DzdIcons.Target, "#FFF8E8", "#FEA82F", "Assessment Center",
                "Required", "Pending review", 0, ReqStatus.Pending));

        if (next.CommitteeApproval)
            rows.Add(new(DzdIcons.Users, "#FFF1E8", "#F46036", "Committee Approval",
                "Required", "Pending review", 0, ReqStatus.Pending));

        return rows;
    }


    private void ApplyProfileToCareerTab()
    {
        _career.CompanyName   = _profile?.CompanyName;
        _career.DeptId        = _profile?.DepartmentId;
        _career.TeamId        = _profile?.TeamId;
        _career.PathId        = _profile?.CareerPathId;
        _career.JobId         = _profile?.JobId;
        _career.Grade         = _profile?.Grade;
        PrefillManagerFromProfile();
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

    private List<DepartmentDto> FilteredDeptsForCompany(string? companyName)
        => string.IsNullOrEmpty(companyName)
            ? []
            : _career.AllDepts
                .Where(d => _career.Companies.Any(c => c.Name == companyName && c.Id == d.CompanyId))
                .ToList();

    private List<TeamDto> FilteredTeamsForDept(int? deptId)
        => deptId.HasValue
            ? _career.AllTeams.Where(t => t.DepartmentId == deptId).ToList()
            : [];

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

    private async Task OpenAvatarUploadDialog()
    {
        if (!SelfService || _profile is null) return;

        var parameters = new DialogParameters
        {
            ["CurrentAvatarUrl"]  = _avatarDataUrl,
            ["UserFullName"]      = _fullName,
            ["CurrentColorIndex"] = _profile.AvatarColorIndex
        };

        var dialog = await DialogService.ShowAsync<AvatarUploadDialog>("Update Profile Photo", parameters);
        var result = await dialog.Result;
        if (result is not { Canceled: false } || result.Data is not AvatarDialogResult dialogResult) return;

        if (dialogResult.ColorChanged)
        {
            var colorResp = await UserService.UpdateMyAvatarColorAsync(dialogResult.ColorIndex);
            if (!colorResp.IsSuccessStatusCode) Snackbar.Add("Failed to update avatar color.", Severity.Error);
        }

        if (dialogResult.File is not null) await UploadMyAvatarAsync(dialogResult.File);
        else if (dialogResult.ColorChanged) await RefreshProfileAsync();

        AvatarState.NotifyChanged();
        await InvokeAsync(StateHasChanged);
    }

    private async Task UploadMyAvatarAsync(AvatarUploadResult fileResult)
    {
        try
        {
            using var content       = new MultipartFormDataContent();
            using var stream        = new MemoryStream(fileResult.Content);
            using var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(fileResult.ContentType);
            content.Add(streamContent, name: "file", fileName: fileResult.FileName);

            var response = await UserService.UpdateMyProfileAvatarAsync(content);
            if (response.IsSuccessStatusCode)
            {
                Snackbar.Add("Avatar updated successfully.", Severity.Success);
                await RefreshProfileAsync();
            }
            else
            {
                Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(response) ?? "Failed to upload avatar.", Severity.Error);
            }
        }
        catch
        {
            Snackbar.Add("Upload failed. Please try again.", Severity.Error);
        }
    }

    private void Back() => Nav.NavigateTo("/employees");
    private string GetManagerDisplayName()
    {
        if (_profile?.ReportsTo is null) return "-";
        var name = AppFormatter.BuildFullName(_profile.ReportsTo.FirstName, _profile.ReportsTo.LastName);
        return string.IsNullOrWhiteSpace(name) ? "-" : name;
    }
    private string GetManagerDepartmentName() => _profile?.ReportsTo?.Department?.Name ?? "-";

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
