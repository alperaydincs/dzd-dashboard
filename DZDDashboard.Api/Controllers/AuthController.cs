using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;

    public AuthController(AppDbContext db)
    {
        _db = db;
    }

    [AllowAnonymous]
    [HttpPost("sync-user")]
    public async Task<ActionResult<int>> SyncUser([FromBody] SyncUserRequest request)
    {
        if (string.IsNullOrEmpty(request?.ObjectId) || string.IsNullOrEmpty(request?.Email))
            return BadRequest("ObjectId and Email required");

        var user = await _db.Users.FirstOrDefaultAsync(u => u.EntraObjectId == request.ObjectId);
        
        if (user == null)
        {
            var nameParts = request.Name?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
            user = new User
            {
                EntraObjectId = request.ObjectId,
                Email = request.Email,
                NormalizedEmail = request.Email.ToUpper(),
                Username = request.Email.Split('@')[0],
                NormalizedUsername = request.Email.Split('@')[0].ToUpper(),
                FirstName = nameParts.Length > 0 ? nameParts[0] : null,
                LastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : null,
                IsActive = true
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        return Ok(user.Id);
    }
}

public class SyncUserRequest
{
    public string? ObjectId { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
}
