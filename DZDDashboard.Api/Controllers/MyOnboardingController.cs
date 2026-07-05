using DZDDashboard.Api.Abstractions;
using DZDDashboard.Api.Utils;
using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace DZDDashboard.Api.Controllers;

[Route("api/my-onboarding")]
public class MyOnboardingController(
    IOnboardingService onboarding,
    IUserDocumentService documents,
    ICurrentUserAccessor currentUser) : BaseController
{
    [HttpGet("state")]
    public async Task<ActionResult<MyOnboardingStateDto>> GetState(CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId) return Unauthorized();
        return Ok(await onboarding.GetOrStartMyAsync(userId, cancellationToken));
    }

    [HttpGet("documents")]
    public async Task<ActionResult<List<UserDocumentDto>>> ListDocuments(CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId) return Unauthorized();
        return Ok(await documents.GetUserDocumentsAsync(userId, cancellationToken));
    }

    [HttpPost("documents")]
    [EnableRateLimiting("upload")]
    [RequestSizeLimit(DocumentConstants.MaxFileSizeBytes)]
    [RequestFormLimits(MultipartBodyLengthLimit = DocumentConstants.MaxFileSizeBytes)]
    public async Task<ActionResult<UserDocumentDto>> Upload([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId) return Unauthorized();

        if (file == null || file.Length == 0)
            return Problem("No file uploaded.", statusCode: 400, title: "Validation Error");

        if (!DocumentConstants.AllowedMimeTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
            return Problem($"Unsupported file type '{file.ContentType}'.", statusCode: 400, title: "Validation Error");

        using var ms = new MemoryStream((int)file.Length);
        await file.CopyToAsync(ms, cancellationToken);
        var bytes = ms.ToArray();

        if (!FileMagicBytes.IsValid(bytes, file.ContentType))
            return Problem("File content does not match the declared content type.", statusCode: 400, title: "Validation Error");

        var fileName = Path.GetFileName(file.FileName);
        return Ok(await documents.UploadAsync(userId, fileName, file.ContentType, bytes, cancellationToken));
    }

    [HttpGet("documents/{documentId:int}/content")]
    public async Task<IActionResult> Download(int documentId, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId) return Unauthorized();
        var content = await documents.GetContentAsync(userId, documentId, cancellationToken);
        if (content is null) return NotFound();
        return File(content.Value.Content, content.Value.ContentType ?? "application/octet-stream", content.Value.FileName);
    }

    [HttpDelete("documents/{documentId:int}")]
    public async Task<IActionResult> Delete(int documentId, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId) return Unauthorized();
        await documents.DeleteAsync(userId, documentId, cancellationToken);
        return NoContent();
    }
}
