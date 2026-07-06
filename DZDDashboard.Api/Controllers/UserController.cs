using DZDDashboard.Api.Abstractions;
using DZDDashboard.Api.Options;
using DZDDashboard.Api.Utils;
using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DZDDashboard.Api.Controllers;

[Route("api/users")]
public class UserController(
    IUserReadService  readService,
    IUserWriteService writeService,
    IUserDocumentService documents,
    ICurrentUserAccessor currentUser) : BaseController
{

    [HttpGet]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<PagedResult<UserSummaryDto>>> GetAll([FromQuery] PaginationParams pagination, CancellationToken cancellationToken)
        => Ok(await readService.GetAllSummariesAsync(pagination.NormalizedPage, pagination.NormalizedPageSize, cancellationToken));

    [HttpGet("search")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<ActionResult<List<UserSearchResultDto>>> Search([FromQuery] string? query, [FromQuery] int take = 20, CancellationToken cancellationToken = default)
        => Ok(await readService.SearchUsersAsync(query, take, cancellationToken));

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await writeService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpGet("my-profile")]
    public async Task<ActionResult<UserProfileDto>> GetMyProfile(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();

        var profile = await readService.GetProfileByIdAsync(userId, cancellationToken);
        return profile is null ? NotFound() : Ok(profile);
    }

    [HttpPut("my-profile/contact-info")]
    public async Task<IActionResult> UpdateMyContactInfo([FromBody] UpdateContactInfoDto dto, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();

        await writeService.UpdateMyContactInfoAsync(userId, dto, cancellationToken);
        return NoContent();
    }

    [HttpGet("my-profile/card")]
    public async Task<ActionResult<EmployeeDto>> GetMyCard(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();

        var card = await readService.GetEmployeeCardAsync(userId, cancellationToken);
        return card is null ? NotFound() : Ok(card);
    }

    [HttpGet("my-profile/sensitive-info")]
    public async Task<ActionResult<EmployeeSensitiveInfoDto>> GetMySensitiveInfo(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();

        var info = await readService.GetSensitiveInfoAsync(userId, cancellationToken);
        return info is null ? NotFound() : Ok(info);
    }

    [HttpPut("my-profile/emergency-contacts")]
    public async Task<IActionResult> UpdateMyEmergencyContacts([FromBody] UpdateEmergencyContactsDto dto, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();

        await writeService.UpdateEmergencyContactsAsync(userId, dto, cancellationToken);
        return NoContent();
    }

    [HttpPut("my-profile/family-info")]
    public async Task<IActionResult> UpdateMyFamilyInfo([FromBody] UpdateFamilyInfoDto dto, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();

        await writeService.UpdateFamilyInfoAsync(userId, dto, cancellationToken);
        return NoContent();
    }

    [HttpPut("my-profile/address-info")]
    public async Task<IActionResult> UpdateMyAddressInfo([FromBody] UpdateAddressInfoDto dto, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();

        await writeService.UpdateAddressInfoAsync(userId, dto, cancellationToken);
        return NoContent();
    }

    [HttpPut("my-profile/education-info")]
    public async Task<IActionResult> UpdateMyEducationInfo([FromBody] UpdateEducationInfoDto dto, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();

        await writeService.UpdateEducationInfoAsync(userId, dto, cancellationToken);
        return NoContent();
    }

    [HttpGet("my-profile/documents")]
    public async Task<ActionResult<List<UserDocumentDto>>> GetMyDocuments(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();

        return Ok(await documents.GetUserDocumentsAsync(userId, cancellationToken));
    }

    [HttpGet("my-profile/documents/{documentId:int}/content")]
    public async Task<IActionResult> DownloadMyDocument(int documentId, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();

        var content = await documents.GetContentAsync(userId, documentId, cancellationToken);
        if (content is null) return NotFound();
        return File(content.Value.Content, content.Value.ContentType ?? "application/octet-stream", content.Value.FileName);
    }

    [HttpGet("{id:int}/card")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<ActionResult<EmployeeDto>> GetEmployeeCard(int id, CancellationToken cancellationToken)
    {
        var card = await readService.GetEmployeeCardAsync(id, cancellationToken);
        return card is null ? NotFound() : Ok(card);
    }

    [HttpGet("by-slug/{slug}/card")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<ActionResult<EmployeeDto>> GetEmployeeCardBySlug(string slug, CancellationToken cancellationToken)
    {
        var card = await readService.GetEmployeeCardBySlugAsync(slug, cancellationToken);
        return card is null ? NotFound() : Ok(card);
    }

    [HttpGet("{id:int}/sensitive-info")]
    [Authorize(Policy = Roles.SensitiveDataPolicy)]
    public async Task<ActionResult<EmployeeSensitiveInfoDto>> GetSensitiveInfo(int id, CancellationToken cancellationToken)
    {
        var info = await readService.GetSensitiveInfoAsync(id, cancellationToken);
        return info is null ? NotFound() : Ok(info);
    }

    [HttpGet("my-avatar")]
    public async Task<ActionResult<UserAvatarDto>> GetMyAvatar(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();

        var avatarDto = await readService.GetAvatarByUserIdAsync(userId, cancellationToken);
        return Ok(avatarDto ?? new UserAvatarDto());
    }

    [HttpPost("my-profile/avatar")]
    [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting("upload")]
    [Microsoft.AspNetCore.Mvc.RequestSizeLimit(AvatarConstants.MaxFileSizeBytes)]
    [Microsoft.AspNetCore.Mvc.RequestFormLimits(MultipartBodyLengthLimit = AvatarConstants.MaxFileSizeBytes)]
    public async Task<IActionResult> UploadMyAvatar([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return Problem("No file uploaded.", statusCode: 400, title: "Validation Error");

        if (!AvatarConstants.AllowedMimeTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
            return Problem(
                $"Unsupported file type '{file.ContentType}'. Allowed: {string.Join(", ", AvatarConstants.AllowedMimeTypes)}.",
                statusCode: 400, title: "Validation Error");

        if (!TryGetUserId(out var userId)) return Unauthorized();

        using var ms = new MemoryStream((int)file.Length);
        await file.CopyToAsync(ms, cancellationToken);
        var fileBytes = ms.ToArray();

        if (!AvatarMagicBytes.IsValid(fileBytes, file.ContentType))
            return Problem("File content does not match the declared content type.",
                statusCode: 400, title: "Validation Error");

        await writeService.UpdateAvatarAsync(userId, file.ContentType, fileBytes, cancellationToken);
        return NoContent();
    }

    [HttpDelete("my-profile/avatar")]
    public async Task<IActionResult> RemoveMyAvatar(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();

        await writeService.RemoveAvatarAsync(userId, cancellationToken);
        return NoContent();
    }

    [HttpPut("my-profile/avatar-color")]
    public async Task<IActionResult> UpdateMyAvatarColor([FromBody] AvatarColorUpdateDto dto, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();

        await writeService.UpdateAvatarColorAsync(userId, dto.ColorIndex, cancellationToken);
        return NoContent();
    }

    [HttpGet("{id:int}/avatar")]
    public async Task<ActionResult<UserAvatarDto>> GetUserAvatar(int id, CancellationToken cancellationToken)
        => Ok(await readService.GetAvatarByUserIdAsync(id, cancellationToken) ?? new UserAvatarDto());

    [HttpPut("{id:int}/basic-info")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateBasicInfo(int id, [FromBody] UpdateBasicInfoDto dto, CancellationToken cancellationToken)
    {
        await writeService.UpdateBasicInfoAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:int}/contacts")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateContacts(int id, [FromBody] UpdateContactsDto dto, CancellationToken cancellationToken)
    {
        await writeService.UpdateContactsAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:int}/citizenship-info")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateCitizenshipInfo(int id, [FromBody] UpdateCitizenshipInfoDto dto, CancellationToken cancellationToken)
    {
        await writeService.UpdateCitizenshipInfoAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:int}/career")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateCareerAssignment(int id, [FromBody] UpdateCareerAssignmentDto dto, CancellationToken cancellationToken)
    {
        await writeService.UpdateCareerAssignmentAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:int}/address-info")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateAddressInfo(int id, [FromBody] UpdateAddressInfoDto dto, CancellationToken cancellationToken)
    {
        await writeService.UpdateAddressInfoAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:int}/education-info")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateEducationInfo(int id, [FromBody] UpdateEducationInfoDto dto, CancellationToken cancellationToken)
    {
        await writeService.UpdateEducationInfoAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:int}/position-history/current")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateCurrentPosition(int id, [FromBody] UpdatePositionHistoryDto dto, CancellationToken cancellationToken)
    {
        await writeService.UpdatePositionHistoryAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:int}/organization-position")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateOrganizationPosition(int id, [FromBody] AssignUserOrganizationPositionDto dto, CancellationToken cancellationToken)
    {
        await writeService.UpdateOrganizationPositionAsync(id, dto.OrganizationPositionId, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:int}/emergency-contacts")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateEmergencyContacts(int id, [FromBody] UpdateEmergencyContactsDto dto, CancellationToken cancellationToken)
    {
        await writeService.UpdateEmergencyContactsAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:int}/family-info")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateFamilyInfo(int id, [FromBody] UpdateFamilyInfoDto dto, CancellationToken cancellationToken)
    {
        await writeService.UpdateFamilyInfoAsync(id, dto, cancellationToken);
        return NoContent();
    }
    private bool TryGetUserId(out int userId)
    {
        var id = currentUser.UserId;
        userId = id ?? 0;
        return id.HasValue;
    }
}
