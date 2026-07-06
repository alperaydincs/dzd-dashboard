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

    public async Task<OnboardingProcessDto?> CompleteItemAsync(int id, int itemId)
        => await PostAsync<OnboardingProcessDto>(ApiRoutes.Onboarding.ItemComplete(id, itemId), new { });

    public async Task<OnboardingProcessDto?> ReopenItemAsync(int id, int itemId)
        => await PostAsync<OnboardingProcessDto>(ApiRoutes.Onboarding.ItemReopen(id, itemId), new { });

    public async Task<OnboardingProcessDto?> AddDocumentAsync(int id, AddProcessDocumentDto dto)
        => await PostAsync<OnboardingProcessDto>(ApiRoutes.Onboarding.Documents(id), dto);

    public async Task<HttpResponseMessage> UploadDocumentAsync(int id, int documentId, string fileName, string contentType, Stream content)
    {
        using var form = new MultipartFormDataContent();
        var streamContent = new StreamContent(content);
        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
        form.Add(streamContent, "file", fileName);
        return await PostMultipartAsync(ApiRoutes.Onboarding.DocumentUpload(id, documentId), form);
    }

    public async Task<OnboardingProcessDto?> ApproveDocumentAsync(int id, int documentId)
        => await PostAsync<OnboardingProcessDto>(ApiRoutes.Onboarding.DocumentApprove(id, documentId), new { });

    public async Task<OnboardingProcessDto?> RequestDocumentCorrectionAsync(int id, int documentId)
        => await PostAsync<OnboardingProcessDto>(ApiRoutes.Onboarding.DocumentRequestCorrection(id, documentId), new { });

    public async Task<OnboardingProcessDto?> ReopenDocumentAsync(int id, int documentId)
        => await PostAsync<OnboardingProcessDto>(ApiRoutes.Onboarding.DocumentReopen(id, documentId), new { });

    public async Task<OnboardingProcessDto?> DeleteDocumentAsync(int id, int documentId)
    {
        var response = await DeleteAsync(ApiRoutes.Onboarding.Document(id, documentId));
        return response.IsSuccessStatusCode ? await GetAsync(id) : null;
    }

    public async Task<byte[]?> DownloadDocumentAsync(int id, int documentId)
    {
        var resp = await ApiClient.GetAsync(ApiRoutes.Onboarding.Document(id, documentId));
        return resp.IsSuccessStatusCode ? await resp.Content.ReadAsByteArrayAsync() : null;
    }

    public async Task<List<DueSoonDocumentDto>?> GetDueSoonDocumentsAsync()
        => await GetAsync<List<DueSoonDocumentDto>>(ApiRoutes.Onboarding.DueSoonDocuments);
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

    public async Task<OffboardingProcessDto?> CompleteItemAsync(int id, int itemId)
        => await PostAsync<OffboardingProcessDto>(ApiRoutes.Offboarding.ItemComplete(id, itemId), new { });

    public async Task<OffboardingProcessDto?> ReopenItemAsync(int id, int itemId)
        => await PostAsync<OffboardingProcessDto>(ApiRoutes.Offboarding.ItemReopen(id, itemId), new { });

    public async Task<OffboardingProcessDto?> AddDocumentAsync(int id, AddProcessDocumentDto dto)
        => await PostAsync<OffboardingProcessDto>(ApiRoutes.Offboarding.Documents(id), dto);

    public async Task<HttpResponseMessage> UploadDocumentAsync(int id, int documentId, string fileName, string contentType, Stream content)
    {
        using var form = new MultipartFormDataContent();
        var streamContent = new StreamContent(content);
        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
        form.Add(streamContent, "file", fileName);
        return await PostMultipartAsync(ApiRoutes.Offboarding.DocumentUpload(id, documentId), form);
    }

    public async Task<OffboardingProcessDto?> ApproveDocumentAsync(int id, int documentId)
        => await PostAsync<OffboardingProcessDto>(ApiRoutes.Offboarding.DocumentApprove(id, documentId), new { });

    public async Task<OffboardingProcessDto?> RequestDocumentCorrectionAsync(int id, int documentId)
        => await PostAsync<OffboardingProcessDto>(ApiRoutes.Offboarding.DocumentRequestCorrection(id, documentId), new { });

    public async Task<OffboardingProcessDto?> ReopenDocumentAsync(int id, int documentId)
        => await PostAsync<OffboardingProcessDto>(ApiRoutes.Offboarding.DocumentReopen(id, documentId), new { });

    public async Task<OffboardingProcessDto?> DeleteDocumentAsync(int id, int documentId)
    {
        var response = await DeleteAsync(ApiRoutes.Offboarding.Document(id, documentId));
        return response.IsSuccessStatusCode ? await GetAsync(id) : null;
    }

    public async Task<byte[]?> DownloadDocumentAsync(int id, int documentId)
    {
        var resp = await ApiClient.GetAsync(ApiRoutes.Offboarding.Document(id, documentId));
        return resp.IsSuccessStatusCode ? await resp.Content.ReadAsByteArrayAsync() : null;
    }

    public async Task<List<DueSoonDocumentDto>?> GetDueSoonDocumentsAsync()
        => await GetAsync<List<DueSoonDocumentDto>>(ApiRoutes.Offboarding.DueSoonDocuments);
}
