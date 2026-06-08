using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DZDDashboard.Api.Controllers;

/// <summary>
/// Manages org-chart positions and the user-to-position assignment.
/// Route kept at <c>api/organization</c> for backward compatibility with existing API clients.
/// </summary>
[Route("api/organization")]
public class OrganizationPositionsController(IOrganizationPositionService positionService) : BaseController
{
    /// <summary>Returns all org-chart positions including computed depth level and assigned user.</summary>
    [HttpGet("positions")]
    public async Task<ActionResult<List<OrganizationPositionDto>>> GetPositions(CancellationToken cancellationToken)
        => Ok(await positionService.GetAllPositionsAsync(cancellationToken));

    [HttpPost("positions")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<OrganizationPositionDto>> CreatePosition([FromBody] CreateOrganizationPositionDto dto, CancellationToken cancellationToken)
    {
        var result = await positionService.CreatePositionAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetPositions), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates a position. Also reassigns the user occupying the position and
    /// triggers a full org-chart ReportsToId recalculation for all positioned users.
    /// </summary>
    [HttpPut("positions/{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdatePosition(int id, [FromBody] UpdateOrganizationPositionDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(id, dto.Id) is { } mismatch) return mismatch;
        await positionService.UpdatePositionAsync(dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("positions/{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeletePosition(int id, CancellationToken cancellationToken)
    {
        await positionService.DeletePositionAsync(id, cancellationToken);
        return NoContent();
    }
}
