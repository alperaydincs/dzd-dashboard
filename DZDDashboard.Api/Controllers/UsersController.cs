using DZDDashboard.Api.Abstractions;
using DZDDashboard.Api.Options;
using DZDDashboard.Api.Utils;
using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DZDDashboard.Api.Controllers;

/// <summary>
/// Manages employee read/write operations.
/// Depends on <see cref="IUserReadService"/> and <see cref="IUserWriteService"/> rather than the
/// combined <see cref="IUserService"/> to honour the Interface Segregation Principle.
/// <see cref="IUserSyncService"/> is consumed exclusively by <c>EntraUserSyncMiddleware</c>.
/// </summary>
[Route("api/[controller]")]
public class UsersController(
    IUserReadService  readService,
    IUserWriteService writeService,
    ICurrentUserAccessor currentUser) : BaseController
{

    /// <summary>
    /// Returns a paged list of user summaries (no avatar base64 — optimised for list views).
    /// Use GET /api/users/{id}/card for the full employee record.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<PagedResult<UserSummaryDto>>> GetAll([FromQuery] PaginationParams pagination, CancellationToken cancellationToken)
        => Ok(await readService.GetAllSummariesAsync(pagination.NormalizedPage, pagination.NormalizedPageSize, cancellationToken));

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
        var userId = currentUser.UserId;
        if (!userId.HasValue) return Unauthorized();

        var profile = await readService.GetProfileByIdAsync(userId.Value, cancellationToken);
        return profile is null ? NotFound() : Ok(profile);
    }

    [HttpPut("my-profile/contact-info")]
    public async Task<IActionResult> UpdateMyContactInfo([FromBody] UpdateContactInfoDto dto, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        if (!userId.HasValue) return Unauthorized();

        await writeService.UpdateMyContactInfoAsync(userId.Value, dto, cancellationToken);
        return NoContent();
    }

    [HttpGet("{id:int}/card")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<EmployeeCardDto>> GetEmployeeCard(int id, CancellationToken cancellationToken)
    {
        var card = await readService.GetEmployeeCardAsync(id, cancellationToken);
        return card is null ? NotFound() : Ok(card);
    }

    /// <summary>
    /// Returns PII-sensitive employee data (citizenship, personal contact, disability, family).
    /// Access controlled by <see cref="Roles.SensitiveDataPolicy"/>:
    /// currently Admin + HR; extend the policy in ServiceCollectionExtensions when new roles are added.
    /// </summary>
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
        var userId = currentUser.UserId;
        if (!userId.HasValue) return Unauthorized();

        var avatarDto = await readService.GetAvatarByUserIdAsync(userId.Value, cancellationToken);
        return Ok(avatarDto ?? new UserAvatarDto());
    }

    [HttpPost("my-profile/avatar")]
    [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting("upload")]
    // Reject oversized requests at the Kestrel/IIS level before the body is read into memory.
    // AvatarConstants.MaxFileSizeBytes is a long; cast to long for the attribute.
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

        var userId = currentUser.UserId;
        if (!userId.HasValue) return Unauthorized();

        using var ms = new MemoryStream((int)file.Length);
        await file.CopyToAsync(ms, cancellationToken);
        var fileBytes = ms.ToArray();

        // Validate magic bytes — Content-Type header can be spoofed by the client.
        if (!AvatarMagicBytes.IsValid(fileBytes, file.ContentType))
            return Problem("File content does not match the declared content type.",
                statusCode: 400, title: "Validation Error");

        var base64 = Convert.ToBase64String(fileBytes);

        await writeService.UpdateAvatarAsync(userId.Value, file.ContentType, base64, cancellationToken);
        return NoContent();
    }

    [HttpGet("{id:int}/avatar")]
    [Authorize(Roles = Roles.Admin)]
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

    [HttpPut("{id:int}/organization-position")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateOrganizationPosition(int id, [FromBody] UpdateUserOrganizationPositionDto dto, CancellationToken cancellationToken)
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
}
