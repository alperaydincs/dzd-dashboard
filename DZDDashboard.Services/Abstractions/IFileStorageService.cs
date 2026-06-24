namespace DZDDashboard.Services;

public interface IFileStorageService
{
    Task<int> SaveAsync(byte[] content, string? contentType, CancellationToken cancellationToken = default);
    Task<(byte[] Content, string? ContentType)?> GetAsync(int storageId, CancellationToken cancellationToken = default);
    Task DeleteAsync(int storageId, CancellationToken cancellationToken = default);
}
