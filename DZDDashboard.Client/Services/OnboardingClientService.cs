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

    public string EvidenceUrl(int id, int itemId) => ApiRoutes.Onboarding.ItemEvidence(id, itemId);
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

    public string EvidenceUrl(int id, int itemId) => ApiRoutes.Offboarding.ItemEvidence(id, itemId);
}
