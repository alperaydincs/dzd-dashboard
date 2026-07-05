using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;

public class DbFileStorageService(AppDbContext context) : IFileStorageService
{
    public async Task<int> SaveAsync(byte[] content, string? contentType, CancellationToken cancellationToken = default)
    {
        var file = new StoredFile { Content = content, ContentType = contentType, SizeBytes = content.LongLength };
        context.Files.Add(file);
        await context.SaveChangesAsync(cancellationToken);
        return file.Id;
    }

    public async Task<(byte[] Content, string? ContentType)?> GetAsync(int storageId, CancellationToken cancellationToken = default)
    {
        var file = await context.Files.AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == storageId, cancellationToken);
        return file is null ? null : (file.Content, file.ContentType);
    }

    public async Task DeleteAsync(int storageId, CancellationToken cancellationToken = default)
    {
        var file = await context.Files.FirstOrDefaultAsync(f => f.Id == storageId, cancellationToken);
        if (file is null) return;
        context.Files.Remove(file);
        await context.SaveChangesAsync(cancellationToken);
    }
}
