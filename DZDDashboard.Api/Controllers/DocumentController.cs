using DZDDashboard.Api.Utils;
using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace DZDDashboard.Api.Controllers;

[Route("api/users/{userId:int}/documents")]
[Authorize(Roles = Roles.AdminOrHr)]
public class UserDocumentsController(IUserDocumentService documents) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<List<UserDocumentDto>>> List(int userId, CancellationToken cancellationToken)
        => Ok(await documents.GetUserDocumentsAsync(userId, cancellationToken));

    [HttpPost]
    [EnableRateLimiting("upload")]
    [RequestSizeLimit(DocumentConstants.MaxFileSizeBytes)]
    [RequestFormLimits(MultipartBodyLengthLimit = DocumentConstants.MaxFileSizeBytes)]
    public async Task<ActionResult<UserDocumentDto>> Upload(int userId, [FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return Problem("No file uploaded.", statusCode: 400, title: "Validation Error");

        if (!DocumentConstants.AllowedMimeTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
            return Problem(
                $"Unsupported file type '{file.ContentType}'. Allowed: {string.Join(", ", DocumentConstants.AllowedMimeTypes)}.",
                statusCode: 400, title: "Validation Error");

        using var ms = new MemoryStream((int)file.Length);
        await file.CopyToAsync(ms, cancellationToken);
        var bytes = ms.ToArray();

        if (!FileMagicBytes.IsValid(bytes, file.ContentType))
            return Problem("File content does not match the declared content type.",
                statusCode: 400, title: "Validation Error");

        var fileName = Path.GetFileName(file.FileName);
        var dto = await documents.UploadAsync(userId, fileName, file.ContentType, bytes, cancellationToken);
        return Ok(dto);
    }

    [HttpGet("{documentId:int}/content")]
    public async Task<IActionResult> Download(int userId, int documentId, CancellationToken cancellationToken)
    {
        var content = await documents.GetContentAsync(userId, documentId, cancellationToken);
        if (content is null) return NotFound();
        return File(content.Value.Content, content.Value.ContentType ?? "application/octet-stream", content.Value.FileName);
    }

    [HttpDelete("{documentId:int}")]
    public async Task<IActionResult> Delete(int userId, int documentId, CancellationToken cancellationToken)
    {
        await documents.DeleteAsync(userId, documentId, cancellationToken);
        return NoContent();
    }

    [HttpPut("{documentId:int}/review")]
    public async Task<IActionResult> Review(int userId, int documentId, [FromBody] ReviewDocumentDto dto, CancellationToken cancellationToken)
    {
        await documents.ReviewAsync(userId, documentId, dto.Status, dto.Note, cancellationToken);
        return NoContent();
    }
}
