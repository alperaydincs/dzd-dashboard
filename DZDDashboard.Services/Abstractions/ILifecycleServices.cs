using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Services;

public interface IOnboardingService
{
    Task<List<OnboardingListItemDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<OnboardingProcessDto> GetAsync(int processId, CancellationToken cancellationToken = default);
    Task<OnboardingProcessDto> StartAsync(StartOnboardingDto dto, CancellationToken cancellationToken = default);
    Task<OnboardingProcessDto> UpdateProcessAsync(int processId, UpdateOnboardingProcessDto dto, CancellationToken cancellationToken = default);
    Task<OnboardingProcessDto> CompleteProcessAsync(int processId, CancellationToken cancellationToken = default);
    Task CancelAsync(int processId, CancellationToken cancellationToken = default);

    Task<MyOnboardingStateDto> GetOrStartMyAsync(int userId, CancellationToken cancellationToken = default);

    Task<OnboardingProcessDto> CompleteItemAsync(int processId, int itemId, CancellationToken cancellationToken = default);
    Task<OnboardingProcessDto> ReopenItemAsync(int processId, int itemId, CancellationToken cancellationToken = default);

    Task<OnboardingProcessDto> AddDocumentAsync(int processId, AddProcessDocumentDto dto, CancellationToken cancellationToken = default);
    Task<OnboardingProcessDto> UploadDocumentAsync(int processId, int documentId, string fileName, string contentType, byte[] content, CancellationToken cancellationToken = default);
    Task<OnboardingProcessDto> ApproveDocumentAsync(int processId, int documentId, CancellationToken cancellationToken = default);
    Task<OnboardingProcessDto> RequestDocumentCorrectionAsync(int processId, int documentId, CancellationToken cancellationToken = default);
    Task<OnboardingProcessDto> ReopenDocumentAsync(int processId, int documentId, CancellationToken cancellationToken = default);
    Task<OnboardingProcessDto> DeleteDocumentAsync(int processId, int documentId, CancellationToken cancellationToken = default);
    Task<(byte[] Content, string? ContentType, string FileName)?> GetDocumentAsync(int processId, int documentId, CancellationToken cancellationToken = default);

    Task<List<DueSoonDocumentDto>> GetDueSoonDocumentsAsync(CancellationToken cancellationToken = default);
}

public interface IOffboardingService
{
    Task<List<OffboardingListItemDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<OffboardingProcessDto> GetAsync(int processId, CancellationToken cancellationToken = default);
    Task<OffboardingProcessDto> StartAsync(StartOffboardingDto dto, CancellationToken cancellationToken = default);

    Task<OffboardingProcessDto> CompleteItemAsync(int processId, int itemId, CancellationToken cancellationToken = default);
    Task<OffboardingProcessDto> ReopenItemAsync(int processId, int itemId, CancellationToken cancellationToken = default);

    Task<OffboardingProcessDto> AddDocumentAsync(int processId, AddProcessDocumentDto dto, CancellationToken cancellationToken = default);
    Task<OffboardingProcessDto> UploadDocumentAsync(int processId, int documentId, string fileName, string contentType, byte[] content, CancellationToken cancellationToken = default);
    Task<OffboardingProcessDto> ApproveDocumentAsync(int processId, int documentId, CancellationToken cancellationToken = default);
    Task<OffboardingProcessDto> RequestDocumentCorrectionAsync(int processId, int documentId, CancellationToken cancellationToken = default);
    Task<OffboardingProcessDto> ReopenDocumentAsync(int processId, int documentId, CancellationToken cancellationToken = default);
    Task<OffboardingProcessDto> DeleteDocumentAsync(int processId, int documentId, CancellationToken cancellationToken = default);
    Task<(byte[] Content, string? ContentType, string FileName)?> GetDocumentAsync(int processId, int documentId, CancellationToken cancellationToken = default);

    Task<List<DueSoonDocumentDto>> GetDueSoonDocumentsAsync(CancellationToken cancellationToken = default);
}
