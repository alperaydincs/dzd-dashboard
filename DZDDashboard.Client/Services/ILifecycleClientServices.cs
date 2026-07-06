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
    Task<OnboardingProcessDto?> CompleteItemAsync(int id, int itemId);
    Task<OnboardingProcessDto?> ReopenItemAsync(int id, int itemId);
    Task<OnboardingProcessDto?> AddDocumentAsync(int id, AddProcessDocumentDto dto);
    Task<HttpResponseMessage> UploadDocumentAsync(int id, int documentId, string fileName, string contentType, Stream content);
    Task<OnboardingProcessDto?> ApproveDocumentAsync(int id, int documentId);
    Task<OnboardingProcessDto?> RequestDocumentCorrectionAsync(int id, int documentId);
    Task<OnboardingProcessDto?> ReopenDocumentAsync(int id, int documentId);
    Task<OnboardingProcessDto?> DeleteDocumentAsync(int id, int documentId);
    Task<byte[]?> DownloadDocumentAsync(int id, int documentId);
    Task<List<DueSoonDocumentDto>?> GetDueSoonDocumentsAsync();
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
    Task<OffboardingProcessDto?> CompleteItemAsync(int id, int itemId);
    Task<OffboardingProcessDto?> ReopenItemAsync(int id, int itemId);
    Task<OffboardingProcessDto?> AddDocumentAsync(int id, AddProcessDocumentDto dto);
    Task<HttpResponseMessage> UploadDocumentAsync(int id, int documentId, string fileName, string contentType, Stream content);
    Task<OffboardingProcessDto?> ApproveDocumentAsync(int id, int documentId);
    Task<OffboardingProcessDto?> RequestDocumentCorrectionAsync(int id, int documentId);
    Task<OffboardingProcessDto?> ReopenDocumentAsync(int id, int documentId);
    Task<OffboardingProcessDto?> DeleteDocumentAsync(int id, int documentId);
    Task<byte[]?> DownloadDocumentAsync(int id, int documentId);
    Task<List<DueSoonDocumentDto>?> GetDueSoonDocumentsAsync();
}
