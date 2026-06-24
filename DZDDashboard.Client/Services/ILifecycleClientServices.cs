using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Client.Services;

public interface IOnboardingClientService
{
    Task<List<OnboardingListItemDto>?> GetAllAsync();
    Task<OnboardingProcessDto?> GetAsync(int id);
    Task<OnboardingProcessDto?> StartAsync(StartOnboardingDto dto);
    Task<OnboardingProcessDto?> UpdateProcessAsync(int id, UpdateOnboardingProcessDto dto);
    Task<OnboardingProcessDto?> CompleteProcessAsync(int id);
    Task<HttpResponseMessage> CancelAsync(int id);
    Task<OnboardingProcessDto?> CompleteItemAsync(int id, int itemId, CompleteChecklistItemDto dto);
    Task<OnboardingProcessDto?> SkipItemAsync(int id, int itemId);
    Task<OnboardingProcessDto?> ReopenItemAsync(int id, int itemId);
    Task<HttpResponseMessage> UpdateNoteAsync(int id, int itemId, UpdateChecklistNoteDto dto);
    Task<HttpResponseMessage> UploadEvidenceAsync(int id, int itemId, string fileName, string contentType, Stream content);
    Task<OnboardingProcessDto?> DeleteEvidenceAsync(int id, int itemId);
    Task<byte[]?> DownloadEvidenceAsync(int id, int itemId);
}

public interface IMyOnboardingClientService
{
    Task<MyOnboardingStateDto?> GetStateAsync();
    Task<List<UserDocumentDto>?> GetDocumentsAsync();
    Task<HttpResponseMessage> UploadDocumentAsync(string fileName, string contentType, Stream content);
    Task<HttpResponseMessage> DeleteDocumentAsync(int docId);
    Task<byte[]?> DownloadDocumentAsync(int docId);
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
    Task<OffboardingProcessDto?> DeleteEvidenceAsync(int id, int itemId);
    Task<byte[]?> DownloadEvidenceAsync(int id, int itemId);
}
