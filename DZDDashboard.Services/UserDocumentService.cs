using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Exceptions;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;

public class UserDocumentService(AppDbContext context, IFileStorageService storage) : IUserDocumentService
{
    public async Task<List<UserDocumentDto>> GetUserDocumentsAsync(int userId, CancellationToken cancellationToken = default)
        => await context.UserDocuments.AsNoTracking()
            .Where(d => d.UserId == userId && d.IsActive)
            .OrderByDescending(d => d.CreatedAt)
            .Select(d => new UserDocumentDto
            {
                Id          = d.Id,
                FileName    = d.FileName,
                ContentType = d.ContentType,
                SizeBytes   = d.SizeBytes,
                UploadedAt  = d.CreatedAt
            })
            .ToListAsync(cancellationToken);

    public async Task<UserDocumentDto> UploadAsync(int userId, string fileName, string contentType, byte[] content, CancellationToken cancellationToken = default)
    {
        if (!await context.Users.AnyAsync(u => u.Id == userId, cancellationToken))
            throw new EntityNotFoundException("User", userId);

        var uniqueName = await EnsureUniqueFileNameAsync(userId, fileName, cancellationToken);
        var storageId  = await storage.SaveAsync(content, contentType, cancellationToken);

        var doc = new UserDocument
        {
            UserId       = userId,
            FileName     = uniqueName,
            ContentType  = contentType,
            SizeBytes    = content.LongLength,
            IsActive     = true,
            StoredFileId = storageId
        };
        context.UserDocuments.Add(doc);
        await context.SaveChangesAsync(cancellationToken);

        return new UserDocumentDto
        {
            Id = doc.Id, FileName = doc.FileName, ContentType = doc.ContentType,
            SizeBytes = doc.SizeBytes, UploadedAt = doc.CreatedAt
        };
    }

    public async Task<(byte[] Content, string? ContentType, string FileName)?> GetContentAsync(int userId, int documentId, CancellationToken cancellationToken = default)
    {
        var doc = await context.UserDocuments.AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == documentId && d.UserId == userId && d.IsActive, cancellationToken);
        if (doc?.StoredFileId is not int storageId) return null;

        var blob = await storage.GetAsync(storageId, cancellationToken);
        return blob is null ? null : (blob.Value.Content, blob.Value.ContentType, doc.FileName ?? "document");
    }

    public async Task DeleteAsync(int userId, int documentId, CancellationToken cancellationToken = default)
    {
        var doc = await context.UserDocuments
            .FirstOrDefaultAsync(d => d.Id == documentId && d.UserId == userId, cancellationToken)
            ?? throw new EntityNotFoundException("UserDocument", documentId);

        var storageId = doc.StoredFileId;
        context.UserDocuments.Remove(doc);
        await context.SaveChangesAsync(cancellationToken);

        if (storageId is int id) await storage.DeleteAsync(id, cancellationToken);
    }

    private async Task<string> EnsureUniqueFileNameAsync(int userId, string fileName, CancellationToken cancellationToken)
    {
        var taken = await context.UserDocuments
            .Where(d => d.UserId == userId)
            .Select(d => d.FileName)
            .ToListAsync(cancellationToken);

        if (!taken.Contains(fileName)) return fileName;

        var ext  = Path.GetExtension(fileName);
        var stem = Path.GetFileNameWithoutExtension(fileName);
        for (var n = 2; ; n++)
        {
            var candidate = $"{stem} ({n}){ext}";
            if (!taken.Contains(candidate)) return candidate;
        }
    }
}
