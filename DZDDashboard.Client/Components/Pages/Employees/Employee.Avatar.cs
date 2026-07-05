using DZDDashboard.Client.Components.Pages.Employees.Dialogs;
using DZDDashboard.Client.Models;
using DZDDashboard.Client.Services;
using MudBlazor;

namespace DZDDashboard.Client.Components.Pages.Employees;

public partial class Employee
{
    private async Task OpenAvatarUploadDialog()
    {
        if (!SelfService || _profile is null) return;

        var parameters = new DialogParameters
        {
            ["CurrentAvatarUrl"]  = _avatarDataUrl,
            ["UserFullName"]      = _fullName,
            ["CurrentColorIndex"] = _profile.AvatarColorIndex
        };

        var dialog = await DialogService.ShowAsync<AvatarUploadDialog>(Loc["employeeProfile.updateProfilePhoto"], parameters);
        var result = await dialog.Result;
        if (result is not { Canceled: false } || result.Data is not AvatarDialogResult dialogResult) return;

        if (dialogResult.ColorChanged)
        {
            var colorResp = await UserService.UpdateMyAvatarColorAsync(dialogResult.ColorIndex);
            if (!colorResp.IsSuccessStatusCode) Snackbar.Add(Loc["employeeProfile.avatarColorUpdateFailed"], Severity.Error);
        }

        if (dialogResult.Removed) await RemoveMyAvatarAsync();
        else if (dialogResult.File is not null) await UploadMyAvatarAsync(dialogResult.File);
        else if (dialogResult.ColorChanged) await RefreshProfileAsync();

        AvatarState.NotifyChanged();
        await InvokeAsync(StateHasChanged);
    }

    private async Task RemoveMyAvatarAsync()
    {
        try
        {
            var response = await UserService.RemoveMyAvatarAsync();
            if (response.IsSuccessStatusCode)
            {
                Snackbar.Add(Loc["employeeProfile.avatarRemoved"], Severity.Success);
                await RefreshProfileAsync();
            }
            else
            {
                Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(response) ?? Loc["employeeProfile.avatarRemoveFailed"], Severity.Error);
            }
        }
        catch
        {
            Snackbar.Add(Loc["employeeProfile.avatarRemoveFailed"], Severity.Error);
        }
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
                Snackbar.Add(Loc["employeeProfile.avatarUpdated"], Severity.Success);
                await RefreshProfileAsync();
            }
            else
            {
                Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(response) ?? Loc["employeeProfile.avatarUploadFailed"], Severity.Error);
            }
        }
        catch
        {
            Snackbar.Add(Loc["employeeProfile.uploadFailedRetry"], Severity.Error);
        }
    }
}
