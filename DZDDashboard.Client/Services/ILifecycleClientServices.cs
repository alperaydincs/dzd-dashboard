using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Client.Services;

public interface IOnboardingClientService
{
    Task<List<OnboardingListItemDto>?> GetAllAsync();
    Task<OnboardingProcessDto?> GetAsync(int id);
    Task<OnboardingProcessDto?> StartAsync(StartOnboardingDto dto);
    Task<OnboardingProcessDto?> CompleteItemAsync(int id, int itemId, CompleteChecklistItemDto dto);
    Task<OnboardingProcessDto?> SkipItemAsync(int id, int itemId);
    Task<OnboardingProcessDto?> ReopenItemAsync(int id, int itemId);
    Task<HttpResponseMessage> UpdateNoteAsync(int id, int itemId, UpdateChecklistNoteDto dto);
    Task<HttpResponseMessage> UploadEvidenceAsync(int id, int itemId, string fileName, string contentType, Stream content);
    string EvidenceUrl(int id, int itemId);
}

public interface IOffboardingClientService
{
    Task<List<OffboardingListItemDto>?> GetAllAsync();
    Task<OffboardingProcessDto?> GetAsync(int id);
    Task<OffboardingProcessDto?> StartAsync(StartOffboardingDto dto);
    Task<OffboardingProcessDto?> CompleteItemAsync(int id, int itemId, CompleteChecklistItemDto dto);
    Task<OffboardingProcessDto?> SkipItemAsync(int id, int itemId);
    Task<OffboardingProcessDto?> ReopenItemAsync(int id, int itemId);
    Task<HttpResponseMessage> UpdateNoteAsync(int id, int itemId, UpdateChecklistNoteDto dto);
    Task<HttpResponseMessage> UploadEvidenceAsync(int id, int itemId, string fileName, string contentType, Stream content);
    string EvidenceUrl(int id, int itemId);
}
