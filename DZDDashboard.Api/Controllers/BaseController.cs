using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DZDDashboard.Api.Controllers;

/// <summary>
/// Shared base for all API controllers.
/// Provides common helpers (id-mismatch guard) so they are not duplicated across controllers.
/// </summary>
[ApiController]
[Authorize]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Returns a 400 Problem result when the route id does not match the DTO id.
    /// Returns null when they match (caller should proceed normally).
    /// </summary>
    protected IActionResult? CheckIdMismatch(int routeId, int dtoId)
        => routeId != dtoId
            ? Problem("Route id does not match the body id.", statusCode: 400, title: "Bad Request")
            : null;
}
