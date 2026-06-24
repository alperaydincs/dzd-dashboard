using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Services;

public interface IUserDocumentService
{
    Task<List<UserDocumentDto>> GetUserDocumentsAsync(int userId, CancellationToken cancellationToken = default);
    Task<UserDocumentDto> UploadAsync(int userId, string fileName, string contentType, byte[] content, CancellationToken cancellationToken = default);
    Task<(byte[] Content, string? ContentType, string FileName)?> GetContentAsync(int userId, int documentId, CancellationToken cancellationToken = default);
    Task DeleteAsync(int userId, int documentId, CancellationToken cancellationToken = default);
}
