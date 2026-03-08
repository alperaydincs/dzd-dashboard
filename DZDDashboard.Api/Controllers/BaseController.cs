using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DZDDashboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected ILogger Logger { get; set; }

    protected BaseController(ILogger logger)
    {
        Logger = logger;
    }

    protected int? GetCurrentUserId()
    {
        var userIdString = User.FindFirstValue("database_user_id");
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            return null;

        return userId;
    }

    protected IActionResult HandleException(Exception ex, string operationName)
    {
        if (ex is KeyNotFoundException)
        {
            Logger.LogWarning(ex, "{Operation} failed: resource not found.", operationName);
            return NotFound();
        }

        if (ex is InvalidOperationException)
        {
            Logger.LogWarning(ex, "{Operation} failed: invalid operation.", operationName);
            return BadRequest(new { message = ex.Message });
        }

        Logger.LogError(ex, "Unexpected error during {Operation}.", operationName);
        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Unexpected server error." });
    }

    protected async Task<IActionResult> ExecuteAsync(Func<Task> action, string operationName)
    {
        try
        {
            await action();
            return Ok();
        }
        catch (Exception ex)
        {
            return HandleException(ex, operationName);
        }
    }
}
