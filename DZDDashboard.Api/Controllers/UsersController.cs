using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.DTOs.Users;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DZDDashboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(UserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
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
        if (user is null) return NotFound();
        return Ok(user);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var success = await _userService.DeleteAsync(id);
            if (!success) return NotFound(); 
            return NoContent(); 
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Delete user failed.");
            return BadRequest(new { message = ex.Message }); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error deleting user.");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Unexpected server error." }); 
        }
    }

    [HttpGet("my-profile")]
    [Authorize]
    public async Task<ActionResult<UserProfileDto>> GetMyProfile()
    {
        var userIdString = User.FindFirstValue("database_user_id");
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }

        var profile = await _userService.GetProfileByIdAsync(userId);

        if (profile == null)
        {
            return NotFound("Profile not found.");
        }

        return Ok(profile);
    }

    [HttpPut("my-profile/contact-info")]
    [Authorize]
    public async Task<IActionResult> UpdateMyContactInfo([FromBody] UpdateContactInfoDto dto)
    {
        var userIdString = User.FindFirstValue("database_user_id");
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }

        var result = await _userService.UpdateContactInfoAsync(userId, dto);
        if (!result) return NotFound("User not found.");

        return NoContent();
    }

    [HttpGet("{id:int}/details")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EmployeeDetailDto>> GetEmployeeDetails(int id)
    {
        var profile = await _userService.GetEmployeeDetailsAsync(id);
        if (profile is null)
            return NotFound("Profile not found.");
        return Ok(profile);
    }

    [HttpGet("my-avatar")]
    [Authorize]
    public async Task<ActionResult<UserAvatarDto>> GetMyAvatar()
    {
        var userIdString = User.FindFirstValue("database_user_id");
        if (!int.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }

        var avatarDto = await _userService.GetAvatarByUserIdAsync(userId);

        if (avatarDto == null)
        {
            return Ok(new UserAvatarDto());
        }

        return Ok(avatarDto);
    }

    [HttpGet("{id:int}/personal-info")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PersonalInfoDto>> GetPersonalInfo(int id)
    {
        var dto = await _userService.GetPersonalInfoAsync(id);
        if (dto is null) return NotFound();
        return Ok(dto);
    }

    [HttpPut("{id:int}/personal-info")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PutPersonalInfo(int id, [FromBody] PersonalInfoDto dto)
    {
        if (dto is null || dto.Id != id) return BadRequest("Invalid payload or ID mismatch.");
        var ok = await _userService.UpdatePersonalInfoAsync(id, dto);
        if (!ok) return NotFound();
        return NoContent(); 
    }

    [HttpPost("my-profile/avatar")]
    [Authorize]
    public async Task<IActionResult> UploadMyAvatar([FromForm] IFormFile file)
    {
        try
        {
            var userIdString = User.FindFirstValue("database_user_id");
            if (!int.TryParse(userIdString, out var userId))
            {
                return Unauthorized();
            }

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            if (file.Length > 100 * 1024 * 1024) 
                return BadRequest("File size exceeds 100MB limit.");

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var base64 = Convert.ToBase64String(ms.ToArray());

            var success = await _userService.UpdateAvatarAsync(userId, file.ContentType, base64);
            if (!success) return NotFound("User not found.");

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading avatar.");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error during upload.");
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
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
