using DZDDashboard.Common.DTOs;
using Microsoft.AspNetCore.Components;

namespace DZDDashboard.Client.Services;

public class OnboardingClientService(IHttpClientFactory httpClientFactory, NavigationManager navigationManager)
    : ApiServiceBase(httpClientFactory, navigationManager), IOnboardingClientService
{
    public async Task<List<OnboardingListItemDto>?> GetAllAsync()
        => await GetAsync<List<OnboardingListItemDto>>(ApiRoutes.Onboarding.Base);

    public async Task<OnboardingProcessDto?> GetAsync(int id)
        => await GetAsync<OnboardingProcessDto>(ApiRoutes.Onboarding.Process(id));

    public async Task<OnboardingProcessDto?> StartAsync(StartOnboardingDto dto)
        => await PostAsync<OnboardingProcessDto>(ApiRoutes.Onboarding.Base, dto);

    public async Task<OnboardingProcessDto?> UpdateProcessAsync(int id, UpdateOnboardingProcessDto dto)
    {
        var response = await PutAsync(ApiRoutes.Onboarding.Process(id), dto);
        return response.IsSuccessStatusCode ? await GetAsync(id) : null;
    }

    public async Task<OnboardingProcessDto?> CompleteProcessAsync(int id)
        => await PostAsync<OnboardingProcessDto>(ApiRoutes.Onboarding.Complete(id), new { });

    public async Task<HttpResponseMessage> CancelAsync(int id)
        => await PostAsync(ApiRoutes.Onboarding.Cancel(id), new { });

    public async Task<OnboardingProcessDto?> DeleteEvidenceAsync(int id, int itemId)
    {
        var response = await DeleteAsync(ApiRoutes.Onboarding.ItemEvidence(id, itemId));
        return response.IsSuccessStatusCode ? await GetAsync(id) : null;
    }

    public async Task<OnboardingProcessDto?> CompleteItemAsync(int id, int itemId, CompleteChecklistItemDto dto)
        => await PostAsync<OnboardingProcessDto>(ApiRoutes.Onboarding.ItemComplete(id, itemId), dto);

    public async Task<OnboardingProcessDto?> SkipItemAsync(int id, int itemId)
        => await PostAsync<OnboardingProcessDto>(ApiRoutes.Onboarding.ItemSkip(id, itemId), new { });

    public async Task<OnboardingProcessDto?> ReopenItemAsync(int id, int itemId)
        => await PostAsync<OnboardingProcessDto>(ApiRoutes.Onboarding.ItemReopen(id, itemId), new { });

    public async Task<HttpResponseMessage> UpdateNoteAsync(int id, int itemId, UpdateChecklistNoteDto dto)
        => await PutAsync(ApiRoutes.Onboarding.ItemNote(id, itemId), dto);

    public async Task<HttpResponseMessage> UploadEvidenceAsync(int id, int itemId, string fileName, string contentType, Stream content)
    {
        using var form = new MultipartFormDataContent();
        var streamContent = new StreamContent(content);
        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
        form.Add(streamContent, "file", fileName);
        return await PostMultipartAsync(ApiRoutes.Onboarding.ItemEvidence(id, itemId), form);
    }

    public async Task<byte[]?> DownloadEvidenceAsync(int id, int itemId)
    {
        var resp = await ApiClient.GetAsync(ApiRoutes.Onboarding.ItemEvidence(id, itemId));
        return resp.IsSuccessStatusCode ? await resp.Content.ReadAsByteArrayAsync() : null;
    }
}

public class MyOnboardingClientService(IHttpClientFactory httpClientFactory, NavigationManager navigationManager)
    : ApiServiceBase(httpClientFactory, navigationManager), IMyOnboardingClientService
{
    public async Task<MyOnboardingStateDto?> GetStateAsync()
        => await GetAsync<MyOnboardingStateDto>(ApiRoutes.MyOnboarding.State);

    public async Task<List<UserDocumentDto>?> GetDocumentsAsync()
        => await GetAsync<List<UserDocumentDto>>(ApiRoutes.MyOnboarding.Documents);

    public async Task<HttpResponseMessage> UploadDocumentAsync(string fileName, string contentType, Stream content)
    {
        using var form = new MultipartFormDataContent();
        var streamContent = new StreamContent(content);
        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
        form.Add(streamContent, "file", fileName);
        return await PostMultipartAsync(ApiRoutes.MyOnboarding.Documents, form);
    }

    public async Task<HttpResponseMessage> DeleteDocumentAsync(int docId)
        => await DeleteAsync(ApiRoutes.MyOnboarding.Document(docId));

    public async Task<byte[]?> DownloadDocumentAsync(int docId)
    {
        var resp = await ApiClient.GetAsync(ApiRoutes.MyOnboarding.DocumentContent(docId));
        return resp.IsSuccessStatusCode ? await resp.Content.ReadAsByteArrayAsync() : null;
    }
}

public class OffboardingClientService(IHttpClientFactory httpClientFactory, NavigationManager navigationManager)
    : ApiServiceBase(httpClientFactory, navigationManager), IOffboardingClientService
{
    public async Task<List<OffboardingListItemDto>?> GetAllAsync()
        => await GetAsync<List<OffboardingListItemDto>>(ApiRoutes.Offboarding.Base);

    public async Task<OffboardingProcessDto?> GetAsync(int id)
        => await GetAsync<OffboardingProcessDto>(ApiRoutes.Offboarding.Process(id));

    public async Task<OffboardingProcessDto?> StartAsync(StartOffboardingDto dto)
        => await PostAsync<OffboardingProcessDto>(ApiRoutes.Offboarding.Base, dto);

    public async Task<OffboardingProcessDto?> CompleteItemAsync(int id, int itemId, CompleteChecklistItemDto dto)
        => await PostAsync<OffboardingProcessDto>(ApiRoutes.Offboarding.ItemComplete(id, itemId), dto);

    public async Task<OffboardingProcessDto?> SkipItemAsync(int id, int itemId)
        => await PostAsync<OffboardingProcessDto>(ApiRoutes.Offboarding.ItemSkip(id, itemId), new { });

    public async Task<OffboardingProcessDto?> ReopenItemAsync(int id, int itemId)
        => await PostAsync<OffboardingProcessDto>(ApiRoutes.Offboarding.ItemReopen(id, itemId), new { });

    public async Task<HttpResponseMessage> UpdateNoteAsync(int id, int itemId, UpdateChecklistNoteDto dto)
        => await PutAsync(ApiRoutes.Offboarding.ItemNote(id, itemId), dto);

    public async Task<HttpResponseMessage> UploadEvidenceAsync(int id, int itemId, string fileName, string contentType, Stream content)
    {
        using var form = new MultipartFormDataContent();
        var streamContent = new StreamContent(content);
        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
        form.Add(streamContent, "file", fileName);
        return await PostMultipartAsync(ApiRoutes.Offboarding.ItemEvidence(id, itemId), form);
    }

    public async Task<OffboardingProcessDto?> DeleteEvidenceAsync(int id, int itemId)
    {
        var response = await DeleteAsync(ApiRoutes.Offboarding.ItemEvidence(id, itemId));
        return response.IsSuccessStatusCode ? await GetAsync(id) : null;
    }

    public async Task<byte[]?> DownloadEvidenceAsync(int id, int itemId)
    {
        var resp = await ApiClient.GetAsync(ApiRoutes.Offboarding.ItemEvidence(id, itemId));
        return resp.IsSuccessStatusCode ? await resp.Content.ReadAsByteArrayAsync() : null;
    }
}
