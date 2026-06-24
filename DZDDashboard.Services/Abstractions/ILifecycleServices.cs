using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Services;

public interface IOnboardingService
{
    Task<List<OnboardingListItemDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<OnboardingProcessDto> GetAsync(int processId, CancellationToken cancellationToken = default);
    Task<OnboardingProcessDto> StartAsync(StartOnboardingDto dto, CancellationToken cancellationToken = default);

    Task<OnboardingProcessDto> CompleteItemAsync(int processId, int itemId, CompleteChecklistItemDto dto, CancellationToken cancellationToken = default);
    Task<OnboardingProcessDto> SkipItemAsync(int processId, int itemId, CancellationToken cancellationToken = default);
    Task<OnboardingProcessDto> ReopenItemAsync(int processId, int itemId, CancellationToken cancellationToken = default);
    Task<OnboardingProcessDto> UpdateItemNoteAsync(int processId, int itemId, UpdateChecklistNoteDto dto, CancellationToken cancellationToken = default);

    Task<OnboardingProcessDto> UploadEvidenceAsync(int processId, int itemId, string fileName, string contentType, byte[] content, CancellationToken cancellationToken = default);
    Task<(byte[] Content, string? ContentType, string FileName)?> GetEvidenceAsync(int processId, int itemId, CancellationToken cancellationToken = default);
}

public interface IOffboardingService
{
    Task<List<OffboardingListItemDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<OffboardingProcessDto> GetAsync(int processId, CancellationToken cancellationToken = default);
    Task<OffboardingProcessDto> StartAsync(StartOffboardingDto dto, CancellationToken cancellationToken = default);

    Task<OffboardingProcessDto> CompleteItemAsync(int processId, int itemId, CompleteChecklistItemDto dto, CancellationToken cancellationToken = default);
    Task<OffboardingProcessDto> SkipItemAsync(int processId, int itemId, CancellationToken cancellationToken = default);
    Task<OffboardingProcessDto> ReopenItemAsync(int processId, int itemId, CancellationToken cancellationToken = default);
    Task<OffboardingProcessDto> UpdateItemNoteAsync(int processId, int itemId, UpdateChecklistNoteDto dto, CancellationToken cancellationToken = default);

    Task<OffboardingProcessDto> UploadEvidenceAsync(int processId, int itemId, string fileName, string contentType, byte[] content, CancellationToken cancellationToken = default);
    Task<(byte[] Content, string? ContentType, string FileName)?> GetEvidenceAsync(int processId, int itemId, CancellationToken cancellationToken = default);
}
