using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace DZDDashboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : BaseController
{
    private static readonly Regex PhoneRegex = new(@"^\+?[0-9]{6,20}$", RegexOptions.Compiled);

    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var users = await _userService.GetAllWithRolesAsync();
        return Ok(users);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var success = await _userService.DeleteAsync(id);
            if (!success) throw new KeyNotFoundException();
            return NoContent();
        }
        catch (Exception ex)
        {
            return HandleException(ex, "Delete user");
        }
    }

    [HttpGet("my-profile")]
    public async Task<ActionResult<UserProfileDto>> GetMyProfile()
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue) return Unauthorized();

        var profile = await _userService.GetProfileByIdAsync(userId.Value);
        return profile is null ? NotFound() : Ok(profile);
    }

    [HttpPut("my-profile/contact-info")]
    public async Task<IActionResult> UpdateMyContactInfo([FromBody] UpdateContactInfoDto dto)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue) return Unauthorized();

        if (!string.IsNullOrWhiteSpace(dto.WorkPhoneNumber) && !PhoneRegex.IsMatch(dto.WorkPhoneNumber))
            return BadRequest("Invalid work phone number.");

        if (!string.IsNullOrWhiteSpace(dto.PersonalPhoneNumber) && !PhoneRegex.IsMatch(dto.PersonalPhoneNumber))
            return BadRequest("Invalid personal phone number.");

        try
        {
            var result = await _userService.UpdateContactInfoAsync(userId.Value, dto);
            return result ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            return HandleException(ex, "Update my contact info");
        }
    }

    [HttpGet("{id:int}/card")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EmployeeCardDto>> GetEmployeeCard(int id)
    {
        var card = await _userService.GetEmployeeCardAsync(id);
        return card is null ? NotFound() : Ok(card);
    }

    [HttpGet("my-avatar")]
    public async Task<ActionResult<UserAvatarDto>> GetMyAvatar()
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue) return Unauthorized();

        var avatarDto = await _userService.GetAvatarByUserIdAsync(userId.Value);
        return Ok(avatarDto ?? new UserAvatarDto());
    }

    [HttpPost("my-profile/avatar")]
    public async Task<IActionResult> UploadMyAvatar([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("No file uploaded.");
        if (file.Length > 100 * 1024 * 1024) return BadRequest("File size exceeds 100MB limit.");

        var userId = GetCurrentUserId();
        if (!userId.HasValue) return Unauthorized();

        try
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var base64 = Convert.ToBase64String(ms.ToArray());

            var success = await _userService.UpdateAvatarAsync(userId.Value, file.ContentType, base64);
            if (!success) throw new KeyNotFoundException("User not found");
            return Ok();
        }
        catch (Exception ex)
        {
            return HandleException(ex, "Upload avatar");
        }
    }

    [HttpGet("{id:int}/avatar")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserAvatarDto>> GetUserAvatar(int id)
    {
        var avatarDto = await _userService.GetAvatarByUserIdAsync(id);
        return Ok(avatarDto ?? new UserAvatarDto());
    }

    [HttpPut("{id}/basic-info")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateBasicInfo(int id, [FromBody] UpdateBasicInfoDto dto)
    {
        var result = await _userService.UpdateBasicInfoAsync(id, dto);
        return result ? Ok() : BadRequest();
    }

    [HttpPut("{id}/contacts")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateContacts(int id, [FromBody] UpdateContactsDto dto)
    {
        try
        {
            var result = await _userService.UpdateContactsAsync(id, dto);
            return result ? Ok() : BadRequest();
        }
        catch (Exception ex)
        {
            return HandleException(ex, "Update contacts");
        }
    }

    [HttpPut("{id}/citizenship-info")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCitizenshipInfo(int id, [FromBody] UpdateCitizenshipInfoDto dto)
    {
        var result = await _userService.UpdateCitizenshipInfoAsync(id, dto);
        return result ? Ok() : BadRequest();
    }

    [HttpPut("{id}/address-info")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateAddressInfo(int id, [FromBody] UpdateAddressInfoDto dto)
    {
        var result = await _userService.UpdateAddressInfoAsync(id, dto);
        return result ? Ok() : BadRequest();
    }

    [HttpPut("{id}/education-info")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateEducationInfo(int id, [FromBody] UpdateEducationInfoDto dto)
    {
        var result = await _userService.UpdateEducationInfoAsync(id, dto);
        return result ? Ok() : BadRequest();
    }

    [HttpPut("{id}/organization-position")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateOrganizationPosition(int id, [FromBody] UpdateUserOrganizationPositionDto dto)
    {
        try
        {
            await _userService.UpdateOrganizationPositionAsync(id, dto.OrganizationPositionId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return HandleException(ex, "Update organization position");
        }
    }

    [HttpPut("{id:int}/emergency-contacts")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateEmergencyContacts(int id, [FromBody] UpdateEmergencyContactsDto dto)
    {
        dto.UserId = id;
        var ok = await _userService.UpdateEmergencyContactsAsync(dto);
        return ok ? Ok() : BadRequest();
    }

    [HttpPut("{id:int}/family-info")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateFamilyInfo(int id, [FromBody] UpdateFamilyInfoDto dto)
    {
        dto.UserId = id;
        var ok = await _userService.UpdateFamilyInfoAsync(dto);
        return ok ? Ok() : BadRequest();
    }
}
