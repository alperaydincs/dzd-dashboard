using DZDDashboard.Api.Utils;
using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace DZDDashboard.Api.Controllers;

[Route("api/offboarding")]
[Authorize(Roles = Roles.AdminOrHr)]
public class OffboardingController(IOffboardingService offboarding) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<List<OffboardingListItemDto>>> List(CancellationToken cancellationToken)
        => Ok(await offboarding.GetAllAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OffboardingProcessDto>> Get(int id, CancellationToken cancellationToken)
        => Ok(await offboarding.GetAsync(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<OffboardingProcessDto>> Start([FromBody] StartOffboardingDto dto, CancellationToken cancellationToken)
    {
        var result = await offboarding.StartAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPost("{id:int}/items/{itemId:int}/complete")]
    public async Task<ActionResult<OffboardingProcessDto>> Complete(int id, int itemId, [FromBody] CompleteChecklistItemDto dto, CancellationToken cancellationToken)
        => Ok(await offboarding.CompleteItemAsync(id, itemId, dto, cancellationToken));

    [HttpPost("{id:int}/items/{itemId:int}/skip")]
    public async Task<ActionResult<OffboardingProcessDto>> Skip(int id, int itemId, CancellationToken cancellationToken)
        => Ok(await offboarding.SkipItemAsync(id, itemId, cancellationToken));

    [HttpPost("{id:int}/items/{itemId:int}/reopen")]
    public async Task<ActionResult<OffboardingProcessDto>> Reopen(int id, int itemId, CancellationToken cancellationToken)
        => Ok(await offboarding.ReopenItemAsync(id, itemId, cancellationToken));

    [HttpPut("{id:int}/items/{itemId:int}/note")]
    public async Task<ActionResult<OffboardingProcessDto>> UpdateNote(int id, int itemId, [FromBody] UpdateChecklistNoteDto dto, CancellationToken cancellationToken)
        => Ok(await offboarding.UpdateItemNoteAsync(id, itemId, dto, cancellationToken));

    [HttpPost("{id:int}/items/{itemId:int}/evidence")]
    [EnableRateLimiting("upload")]
    [RequestSizeLimit(DocumentConstants.MaxFileSizeBytes)]
    [RequestFormLimits(MultipartBodyLengthLimit = DocumentConstants.MaxFileSizeBytes)]
    public async Task<ActionResult<OffboardingProcessDto>> UploadEvidence(int id, int itemId, [FromForm] IFormFile file, CancellationToken cancellationToken)
    {
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
        return Ok(await offboarding.UploadEvidenceAsync(id, itemId, fileName, file.ContentType, bytes, cancellationToken));
    }

    [HttpDelete("{id:int}/items/{itemId:int}/evidence")]
    public async Task<ActionResult<OffboardingProcessDto>> DeleteEvidence(int id, int itemId, CancellationToken cancellationToken)
        => Ok(await offboarding.DeleteEvidenceAsync(id, itemId, cancellationToken));

    [HttpGet("{id:int}/items/{itemId:int}/evidence")]
    public async Task<IActionResult> DownloadEvidence(int id, int itemId, CancellationToken cancellationToken)
    {
        var content = await offboarding.GetEvidenceAsync(id, itemId, cancellationToken);
        if (content is null) return NotFound();
        return File(content.Value.Content, content.Value.ContentType ?? "application/octet-stream", content.Value.FileName);
    }
}
