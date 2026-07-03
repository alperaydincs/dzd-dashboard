using DZDDashboard.Client.Services;
using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;

namespace DZDDashboard.Client.Components.Pages.Employees;

public partial class Employee
{
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
            Snackbar.Add(string.Format(Loc["employeeProfile.fileSizeExceeds"], DocumentConstants.MaxFileSizeBytes / 1024 / 1024), Severity.Warning);
            return;
        }
        if (!DocumentConstants.AllowedMimeTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
        {
            Snackbar.Add(Loc["employeeProfile.unsupportedCvFileType"], Severity.Warning);
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
                Snackbar.Add(Loc["onboardingDetail.documentUploaded"], Severity.Success);
            }
            else
            {
                Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? Loc["employeeProfile.uploadFailed"], Severity.Error);
            }
        }
        catch
        {
            Snackbar.Add(Loc["employeeProfile.uploadFailedRetry"], Severity.Error);
        }
        finally { _cvUploading = false; }
    }

    private async Task DownloadDocumentAsync(UserDocumentDto doc)
    {
        try
        {
            var bytes = await UserService.DownloadUserDocumentAsync(_userId, doc.Id);
            if (bytes is null) { Snackbar.Add(Loc["employeeProfile.downloadFailed"], Severity.Error); return; }
            await JS.InvokeVoidAsync("dzdDownloadFile", doc.FileName, Convert.ToBase64String(bytes), doc.ContentType);
        }
        catch { Snackbar.Add(Loc["employeeProfile.downloadFailed"], Severity.Error); }
    }

    private async Task DeleteDocumentAsync(UserDocumentDto doc)
    {
        var resp = await UserService.DeleteUserDocumentAsync(_userId, doc.Id);
        if (resp.IsSuccessStatusCode)
        {
            await LoadCvDocumentsAsync();
            Snackbar.Add(Loc["onboardingDetail.documentDeleted"], Severity.Success);
        }
        else
        {
            Snackbar.Add(Loc["employeeProfile.deleteFailed"], Severity.Error);
        }
    }

    private static string FormatFileSize(long bytes)
        => bytes >= 1024 * 1024 ? $"{bytes / 1024.0 / 1024.0:0.#} MB"
         : bytes >= 1024 ? $"{bytes / 1024.0:0.#} KB"
         : $"{bytes} B";
}
