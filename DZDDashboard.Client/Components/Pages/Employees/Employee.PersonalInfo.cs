using DZDDashboard.Client.Components.Pages.Employees.Dialogs;
using DZDDashboard.Client.Services;
using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Utils;
using IntlTelInputBlazor;
using MudBlazor;

namespace DZDDashboard.Client.Components.Pages.Employees;

public partial class Employee
{
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

    private void RemoveEmergencyContact(EmergencyContactDto contact)
    {
        _profile?.EmergencyContacts?.Remove(contact);
        _emergencyContactPhones.Remove(contact);
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
            Loc["employeeProfile.basicInfoUpdated"],
            Loc["employeeProfile.basicInfoUpdateFailed"],
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

        if (!IsEmailValidOrEmpty(dto.Email))        { Snackbar.Add(Loc["employeeProfile.invalidWorkEmail"], Severity.Error); return; }
        if (!IsEmailValidOrEmpty(dto.PersonalEmail)) { Snackbar.Add(Loc["employeeProfile.invalidPersonalEmail"], Severity.Error); return; }

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
            Loc["employeeProfile.contactInfoUpdated"],
            Loc["employeeProfile.contactInfoUpdateFailed"],
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
            Loc["employeeProfile.citizenshipInfoUpdated"],
            Loc["employeeProfile.citizenshipInfoUpdateFailed"],
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
            Loc["employeeProfile.emergencyContactUpdated"],
            Loc["employeeProfile.emergencyContactUpdateFailed"],
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
            SelfService
                ? () => UserService.UpdateMyAddressInfoAsync(dto)
                : () => UserService.UpdateAddressInfoAsync(_userId, dto),
            Loc["employeeProfile.addressInfoUpdated"],
            Loc["employeeProfile.addressInfoUpdateFailed"],
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
                Id = r.Id ?? 0, EducationLevel = r.EducationLevel, Institution = r.Institution,
                Program = r.Program, GraduationDate = r.GraduationDate, Status = r.Status
            }).ToList()
        };

        await ExecuteSaveRequestAsync(
            SelfService
                ? () => UserService.UpdateMyEducationInfoAsync(dto)
                : () => UserService.UpdateEducationInfoAsync(_userId, dto),
            Loc["employeeProfile.educationInfoUpdated"],
            Loc["employeeProfile.educationInfoUpdateFailed"],
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
            Loc["employeeProfile.familyInfoUpdated"],
            Loc["employeeProfile.familyInfoUpdateFailed"],
            refreshProfileOnSuccess: true,
            onSuccess: () => { _familyEdit.Commit(); });
    }

    private async Task OpenAddEducationHistoryDialogAsync()
    {
        var options = SmallEscDialog;
        var dialog  = await DialogService.ShowAsync<AddEducationHistoryDialog>(Loc["employeeProfile.addEducationHistory"], new DialogParameters(), options);
        var result = await dialog.Result;

        if (result is null || result.Canceled || result.Data is not AddEducationHistoryDialog.AddEducationHistoryDialogResult data) return;

        _educationHistoryRecords.Add(new EducationHistoryRecord
        {
            EducationLevel = data.EducationLevel, Institution = data.Institution,
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
                EducationLevel = record.EducationLevel, Institution = record.Institution,
                Program = record.Program, GraduationDate = record.GraduationDate,
                Status = record.Status ?? EducationStatuses.Completed
            },
            [nameof(AddEducationHistoryDialog.ConfirmButtonText)] = Loc["employeeProfile.update"]
        };
        var options = SmallEscDialog;

        var dialog = await DialogService.ShowAsync<AddEducationHistoryDialog>(Loc["employeeProfile.editEducationHistory"], parameters, options);
        var result = await dialog.Result;

        if (result is null || result.Canceled || result.Data is not AddEducationHistoryDialog.AddEducationHistoryDialogResult data) return;

        record.EducationLevel = data.EducationLevel;
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
        Id = source.Id, EducationLevel = source.EducationLevel, Institution = source.Institution,
        Program = source.Program, GraduationDate = source.GraduationDate, Status = source.Status
    };

    private static string GetEducationLevelAvatarClass(string? level)
    {
        if (level is null) return "edu-avatar-default";
        if (level.Equals(EducationLevels.HighSchool, StringComparison.OrdinalIgnoreCase)) return "edu-avatar-highschool";
        if (level.Equals(EducationLevels.Bachelors,  StringComparison.OrdinalIgnoreCase)) return "edu-avatar-bachelors";
        if (level.Equals(EducationLevels.Masters,    StringComparison.OrdinalIgnoreCase)) return "edu-avatar-masters";
        if (level.Equals(EducationLevels.Phd,        StringComparison.OrdinalIgnoreCase)) return "edu-avatar-phd";
        return "edu-avatar-default";
    }

    private static string GetEducationLevelChipClass(string? level)
    {
        if (level is null) return "edu-chip-default";
        if (level.Equals(EducationLevels.HighSchool, StringComparison.OrdinalIgnoreCase)) return "edu-chip-highschool";
        if (level.Equals(EducationLevels.Bachelors,  StringComparison.OrdinalIgnoreCase)) return "edu-chip-bachelors";
        if (level.Equals(EducationLevels.Masters,    StringComparison.OrdinalIgnoreCase)) return "edu-chip-masters";
        if (level.Equals(EducationLevels.Phd,        StringComparison.OrdinalIgnoreCase)) return "edu-chip-phd";
        return "edu-chip-default";
    }

    private static bool IsEmailValidOrEmpty(string? email)
        => string.IsNullOrWhiteSpace(email) || AppFormatter.IsValidEmail(email);

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
            Snackbar.Add(Loc["employeeProfile.unexpectedError"], Severity.Error);
        }
    }
}
