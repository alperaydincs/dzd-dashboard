using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DZDDashboard.Api.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPut("{id}/basic-info")]
        public async Task<IActionResult> UpdateBasicInfo(int id, [FromBody] UpdateBasicInfoDto dto)
        {
            var result = await _userService.UpdateBasicInfoAsync(id, dto);
            return result ? Ok() : BadRequest();
        }

        [HttpPut("{id}/contacts")]
        public async Task<IActionResult> UpdateContacts(int id, [FromBody] UpdateContactsDto dto)
        {
            var result = await _userService.UpdateContactsAsync(id, dto);
            return result ? Ok() : BadRequest();
        }

        [HttpPut("{id}/citizenship-info")]
        public async Task<IActionResult> UpdateCitizenshipInfo(int id, [FromBody] UpdateCitizenshipInfoDto dto)
        {
            var result = await _userService.UpdateCitizenshipInfoAsync(id, dto);
            return result ? Ok() : BadRequest();
        }

        [HttpPut("{id}/address-info")]
        public async Task<IActionResult> UpdateAddressInfo(int id, [FromBody] UpdateAddressInfoDto dto)
        {
            var result = await _userService.UpdateAddressInfoAsync(id, dto);
            return result ? Ok() : BadRequest();
        }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var users = await _userService.GetAllWithRolesAsync();
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserDto>> Get(int id)
    {
        var user = await _userService.GetByIdWithRolesAsync(id);
        return user is null ? NotFound() : Ok(user);
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
    [Authorize]
    public async Task<ActionResult<UserProfileDto>> GetMyProfile()
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue) return Unauthorized();

        var profile = await _userService.GetProfileByIdAsync(userId.Value);
        return profile is null ? NotFound() : Ok(profile);
    }

    [HttpPut("my-profile/contact-info")]
    [Authorize]
    public async Task<IActionResult> UpdateMyContactInfo([FromBody] UpdateContactInfoDto dto)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue) return Unauthorized();

        if (!string.IsNullOrWhiteSpace(dto.PersonalEmail) && !new EmailAddressAttribute().IsValid(dto.PersonalEmail))
            return BadRequest("Invalid personal email.");

        if (!string.IsNullOrWhiteSpace(dto.WorkPhoneNumber) && !Regex.IsMatch(dto.WorkPhoneNumber, "^\\+?[0-9]{6,20}$"))
            return BadRequest("Invalid work phone number.");

        if (!string.IsNullOrWhiteSpace(dto.PersonalPhoneNumber) && !Regex.IsMatch(dto.PersonalPhoneNumber, "^\\+?[0-9]{6,20}$"))
            return BadRequest("Invalid personal phone number.");

        var result = await _userService.UpdateContactInfoAsync(userId.Value, dto);
        return result ? NoContent() : NotFound();
    }

    [HttpGet("{id:int}/details")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EmployeeDetailDto>> GetEmployeeDetails(int id)
    {
        var profile = await _userService.GetEmployeeDetailsAsync(id);
        return profile is null ? NotFound() : Ok(profile);
    }

    [HttpGet("my-avatar")]
    [Authorize]
    public async Task<ActionResult<UserAvatarDto>> GetMyAvatar()
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue) return Unauthorized();

        var avatarDto = await _userService.GetAvatarByUserIdAsync(userId.Value);
        return Ok(avatarDto ?? new UserAvatarDto());
    }

    [HttpGet("{id:int}/personal-info")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PersonalInfoDto>> GetPersonalInfo(int id)
    {
        var dto = await _userService.GetPersonalInfoAsync(id);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPut("{id:int}/personal-info")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PutPersonalInfo(int id, [FromBody] PersonalInfoDto dto)
    {
        if (dto?.Id != id) return BadRequest("Invalid payload or ID mismatch.");
        // Validate emails
        if (!string.IsNullOrWhiteSpace(dto.Email) && !new EmailAddressAttribute().IsValid(dto.Email))
            return BadRequest("Invalid work email.");

        if (!string.IsNullOrWhiteSpace(dto.PersonalEmail) && !new EmailAddressAttribute().IsValid(dto.PersonalEmail))
            return BadRequest("Invalid personal email.");

        if (!string.IsNullOrWhiteSpace(dto.PhoneNumber) && !Regex.IsMatch(dto.PhoneNumber, "^\\+?[0-9]{6,20}$"))
            return BadRequest("Invalid work phone number.");

        if (!string.IsNullOrWhiteSpace(dto.PersonalPhoneNumber) && !Regex.IsMatch(dto.PersonalPhoneNumber, "^\\+?[0-9]{6,20}$"))
            return BadRequest("Invalid personal phone number.");

        var ok = await _userService.UpdatePersonalInfoAsync(id, dto);
        return ok ? NoContent() : NotFound();
    }

    [HttpPost("my-profile/avatar")]
    [Authorize]
    public async Task<IActionResult> UploadMyAvatar([FromForm] IFormFile file)
    {
        if (file?.Length == 0) return BadRequest("No file uploaded.");
        if (file == null || file.Length > 100 * 1024 * 1024) return BadRequest("File size exceeds 100MB limit.");

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

    [HttpPut("{id}/organization-position")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateOrganizationPosition(int id, [FromBody] UpdateUserOrganizationPositionDto dto)
    {
        try
        {
            await _userService.UpdateOrganizationPositionAsync(id, dto.OrganizationPositionId, dto.ReportsToId);
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
