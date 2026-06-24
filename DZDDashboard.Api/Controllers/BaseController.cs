using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DZDDashboard.Api.Controllers;

[ApiController]
[Authorize]
public abstract class BaseController : ControllerBase
{
    protected IActionResult? CheckIdMismatch(int routeId, int dtoId)
        => routeId != dtoId
            ? Problem("Route id does not match the body id.", statusCode: 400, title: "Bad Request")
            : null;
}
