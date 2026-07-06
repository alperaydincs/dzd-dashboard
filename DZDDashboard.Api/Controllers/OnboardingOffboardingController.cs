using DZDDashboard.Api.Utils;
using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace DZDDashboard.Api.Controllers;

[Authorize(Roles = Roles.AdminOrHr)]
public class OnboardingOffboardingController(
    IProcessTemplateService processTemplates,
    IChecklistTemplateService checklistTemplates,
    IDocumentTemplateService documentTemplates,
    IOnboardingService onboarding,
    IOffboardingService offboarding) : BaseController
{
    [HttpGet("api/process-templates")]
    public async Task<ActionResult<List<ProcessTemplateDto>>> ListProcessTemplates([FromQuery] string kind, CancellationToken cancellationToken)
        => Ok(await processTemplates.GetAsync(kind, cancellationToken));

    [HttpPost("api/process-templates")]
    public async Task<ActionResult<ProcessTemplateDto>> CreateProcessTemplate([FromBody] ProcessTemplateDto dto, CancellationToken cancellationToken)
        => Ok(await processTemplates.CreateAsync(dto, cancellationToken));

    [HttpPut("api/process-templates/{id:int}")]
    public async Task<IActionResult> UpdateProcessTemplate(int id, [FromBody] ProcessTemplateDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(id, dto.Id) is { } mismatch) return mismatch;
        await processTemplates.UpdateAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("api/process-templates/{id:int}")]
    public async Task<IActionResult> DeleteProcessTemplate(int id, CancellationToken cancellationToken)
    {
        await processTemplates.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpGet("api/checklist-templates")]
    public async Task<ActionResult<List<ChecklistItemTemplateDto>>> ListChecklistTemplates([FromQuery] int processTemplateId, CancellationToken cancellationToken)
        => Ok(await checklistTemplates.GetAsync(processTemplateId, cancellationToken));

    [HttpPost("api/checklist-templates")]
    public async Task<ActionResult<ChecklistItemTemplateDto>> CreateChecklistTemplate([FromBody] ChecklistItemTemplateDto dto, CancellationToken cancellationToken)
        => Ok(await checklistTemplates.CreateAsync(dto, cancellationToken));

    [HttpPut("api/checklist-templates/{id:int}")]
    public async Task<IActionResult> UpdateChecklistTemplate(int id, [FromBody] ChecklistItemTemplateDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(id, dto.Id) is { } mismatch) return mismatch;
        await checklistTemplates.UpdateAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("api/checklist-templates/{id:int}")]
    public async Task<IActionResult> DeleteChecklistTemplate(int id, CancellationToken cancellationToken)
    {
        await checklistTemplates.DeleteAsync(id, cancellationToken);
        return NoContent();
    }


    [HttpGet("api/document-templates")]
    public async Task<ActionResult<List<DocumentTemplateDto>>> ListDocumentTemplates([FromQuery] int processTemplateId, CancellationToken cancellationToken)
        => Ok(await documentTemplates.GetAsync(processTemplateId, cancellationToken));

    [HttpPost("api/document-templates")]
    public async Task<ActionResult<DocumentTemplateDto>> CreateDocumentTemplate([FromBody] DocumentTemplateDto dto, CancellationToken cancellationToken)
        => Ok(await documentTemplates.CreateAsync(dto, cancellationToken));

    [HttpPut("api/document-templates/{id:int}")]
    public async Task<IActionResult> UpdateDocumentTemplate(int id, [FromBody] DocumentTemplateDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(id, dto.Id) is { } mismatch) return mismatch;
        await documentTemplates.UpdateAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("api/document-templates/{id:int}")]
    public async Task<IActionResult> DeleteDocumentTemplate(int id, CancellationToken cancellationToken)
    {
        await documentTemplates.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpGet("api/onboarding")]
    public async Task<ActionResult<List<OnboardingListItemDto>>> ListOnboarding(CancellationToken cancellationToken)
        => Ok(await onboarding.GetAllAsync(cancellationToken));

    [HttpGet("api/onboarding/due-soon-documents")]
    public async Task<ActionResult<List<DueSoonDocumentDto>>> GetOnboardingDueSoonDocuments(CancellationToken cancellationToken)
        => Ok(await onboarding.GetDueSoonDocumentsAsync(cancellationToken));

    [HttpGet("api/onboarding/{id:int}")]
    public async Task<ActionResult<OnboardingProcessDto>> GetOnboarding(int id, CancellationToken cancellationToken)
        => Ok(await onboarding.GetAsync(id, cancellationToken));

    [HttpPost("api/onboarding")]
    public async Task<ActionResult<OnboardingProcessDto>> StartOnboarding([FromBody] StartOnboardingDto dto, CancellationToken cancellationToken)
    {
        var result = await onboarding.StartAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetOnboarding), new { id = result.Id }, result);
    }

    [HttpPut("api/onboarding/{id:int}")]
    public async Task<ActionResult<OnboardingProcessDto>> UpdateOnboardingProcess(int id, [FromBody] UpdateOnboardingProcessDto dto, CancellationToken cancellationToken)
        => Ok(await onboarding.UpdateProcessAsync(id, dto, cancellationToken));

    [HttpPost("api/onboarding/{id:int}/complete")]
    public async Task<ActionResult<OnboardingProcessDto>> CompleteOnboardingProcess(int id, CancellationToken cancellationToken)
        => Ok(await onboarding.CompleteProcessAsync(id, cancellationToken));

    [HttpPost("api/onboarding/{id:int}/cancel")]
    public async Task<IActionResult> CancelOnboarding(int id, CancellationToken cancellationToken)
    {
        await onboarding.CancelAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPost("api/onboarding/{id:int}/items/{itemId:int}/complete")]
    public async Task<ActionResult<OnboardingProcessDto>> CompleteOnboardingItem(int id, int itemId, CancellationToken cancellationToken)
        => Ok(await onboarding.CompleteItemAsync(id, itemId, cancellationToken));

    [HttpPost("api/onboarding/{id:int}/items/{itemId:int}/reopen")]
    public async Task<ActionResult<OnboardingProcessDto>> ReopenOnboardingItem(int id, int itemId, CancellationToken cancellationToken)
        => Ok(await onboarding.ReopenItemAsync(id, itemId, cancellationToken));

    [HttpPost("api/onboarding/{id:int}/documents")]
    public async Task<ActionResult<OnboardingProcessDto>> AddOnboardingDocument(int id, [FromBody] AddProcessDocumentDto dto, CancellationToken cancellationToken)
        => Ok(await onboarding.AddDocumentAsync(id, dto, cancellationToken));

    [HttpPost("api/onboarding/{id:int}/documents/{documentId:int}/upload")]
    [EnableRateLimiting("upload")]
    [RequestSizeLimit(DocumentConstants.MaxFileSizeBytes)]
    [RequestFormLimits(MultipartBodyLengthLimit = DocumentConstants.MaxFileSizeBytes)]
    public async Task<ActionResult<OnboardingProcessDto>> UploadOnboardingDocument(int id, int documentId, [FromForm] IFormFile file, CancellationToken cancellationToken)
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
        return Ok(await onboarding.UploadDocumentAsync(id, documentId, fileName, file.ContentType, bytes, cancellationToken));
    }

    [HttpGet("api/onboarding/{id:int}/documents/{documentId:int}")]
    public async Task<IActionResult> DownloadOnboardingDocument(int id, int documentId, CancellationToken cancellationToken)
    {
        var content = await onboarding.GetDocumentAsync(id, documentId, cancellationToken);
        if (content is null) return NotFound();
        return File(content.Value.Content, content.Value.ContentType ?? "application/octet-stream", content.Value.FileName);
    }

    [HttpPost("api/onboarding/{id:int}/documents/{documentId:int}/approve")]
    public async Task<ActionResult<OnboardingProcessDto>> ApproveOnboardingDocument(int id, int documentId, CancellationToken cancellationToken)
        => Ok(await onboarding.ApproveDocumentAsync(id, documentId, cancellationToken));

    [HttpPost("api/onboarding/{id:int}/documents/{documentId:int}/request-correction")]
    public async Task<ActionResult<OnboardingProcessDto>> RequestOnboardingDocumentCorrection(int id, int documentId, CancellationToken cancellationToken)
        => Ok(await onboarding.RequestDocumentCorrectionAsync(id, documentId, cancellationToken));

    [HttpPost("api/onboarding/{id:int}/documents/{documentId:int}/reopen")]
    public async Task<ActionResult<OnboardingProcessDto>> ReopenOnboardingDocument(int id, int documentId, CancellationToken cancellationToken)
        => Ok(await onboarding.ReopenDocumentAsync(id, documentId, cancellationToken));

    [HttpDelete("api/onboarding/{id:int}/documents/{documentId:int}")]
    public async Task<ActionResult<OnboardingProcessDto>> DeleteOnboardingDocument(int id, int documentId, CancellationToken cancellationToken)
        => Ok(await onboarding.DeleteDocumentAsync(id, documentId, cancellationToken));


    [HttpGet("api/offboarding")]
    public async Task<ActionResult<List<OffboardingListItemDto>>> ListOffboarding(CancellationToken cancellationToken)
        => Ok(await offboarding.GetAllAsync(cancellationToken));

    [HttpGet("api/offboarding/due-soon-documents")]
    public async Task<ActionResult<List<DueSoonDocumentDto>>> GetOffboardingDueSoonDocuments(CancellationToken cancellationToken)
        => Ok(await offboarding.GetDueSoonDocumentsAsync(cancellationToken));

    [HttpGet("api/offboarding/{id:int}")]
    public async Task<ActionResult<OffboardingProcessDto>> GetOffboarding(int id, CancellationToken cancellationToken)
        => Ok(await offboarding.GetAsync(id, cancellationToken));

    [HttpPost("api/offboarding")]
    public async Task<ActionResult<OffboardingProcessDto>> StartOffboarding([FromBody] StartOffboardingDto dto, CancellationToken cancellationToken)
    {
        var result = await offboarding.StartAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetOffboarding), new { id = result.Id }, result);
    }

    [HttpPost("api/offboarding/{id:int}/items/{itemId:int}/complete")]
    public async Task<ActionResult<OffboardingProcessDto>> CompleteOffboardingItem(int id, int itemId, CancellationToken cancellationToken)
        => Ok(await offboarding.CompleteItemAsync(id, itemId, cancellationToken));

    [HttpPost("api/offboarding/{id:int}/items/{itemId:int}/reopen")]
    public async Task<ActionResult<OffboardingProcessDto>> ReopenOffboardingItem(int id, int itemId, CancellationToken cancellationToken)
        => Ok(await offboarding.ReopenItemAsync(id, itemId, cancellationToken));

    [HttpPost("api/offboarding/{id:int}/documents")]
    public async Task<ActionResult<OffboardingProcessDto>> AddOffboardingDocument(int id, [FromBody] AddProcessDocumentDto dto, CancellationToken cancellationToken)
        => Ok(await offboarding.AddDocumentAsync(id, dto, cancellationToken));

    [HttpPost("api/offboarding/{id:int}/documents/{documentId:int}/upload")]
    [EnableRateLimiting("upload")]
    [RequestSizeLimit(DocumentConstants.MaxFileSizeBytes)]
    [RequestFormLimits(MultipartBodyLengthLimit = DocumentConstants.MaxFileSizeBytes)]
    public async Task<ActionResult<OffboardingProcessDto>> UploadOffboardingDocument(int id, int documentId, [FromForm] IFormFile file, CancellationToken cancellationToken)
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
        return Ok(await offboarding.UploadDocumentAsync(id, documentId, fileName, file.ContentType, bytes, cancellationToken));
    }

    [HttpGet("api/offboarding/{id:int}/documents/{documentId:int}")]
    public async Task<IActionResult> DownloadOffboardingDocument(int id, int documentId, CancellationToken cancellationToken)
    {
        var content = await offboarding.GetDocumentAsync(id, documentId, cancellationToken);
        if (content is null) return NotFound();
        return File(content.Value.Content, content.Value.ContentType ?? "application/octet-stream", content.Value.FileName);
    }

    [HttpPost("api/offboarding/{id:int}/documents/{documentId:int}/approve")]
    public async Task<ActionResult<OffboardingProcessDto>> ApproveOffboardingDocument(int id, int documentId, CancellationToken cancellationToken)
        => Ok(await offboarding.ApproveDocumentAsync(id, documentId, cancellationToken));

    [HttpPost("api/offboarding/{id:int}/documents/{documentId:int}/request-correction")]
    public async Task<ActionResult<OffboardingProcessDto>> RequestOffboardingDocumentCorrection(int id, int documentId, CancellationToken cancellationToken)
        => Ok(await offboarding.RequestDocumentCorrectionAsync(id, documentId, cancellationToken));

    [HttpPost("api/offboarding/{id:int}/documents/{documentId:int}/reopen")]
    public async Task<ActionResult<OffboardingProcessDto>> ReopenOffboardingDocument(int id, int documentId, CancellationToken cancellationToken)
        => Ok(await offboarding.ReopenDocumentAsync(id, documentId, cancellationToken));

    [HttpDelete("api/offboarding/{id:int}/documents/{documentId:int}")]
    public async Task<ActionResult<OffboardingProcessDto>> DeleteOffboardingDocument(int id, int documentId, CancellationToken cancellationToken)
        => Ok(await offboarding.DeleteDocumentAsync(id, documentId, cancellationToken));
}
