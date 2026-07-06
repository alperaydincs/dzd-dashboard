using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DZDDashboard.Api.Controllers;

[Route("api/organization")]
public class OrganizationPositionController(IOrganizationPositionService positionService) : BaseController
{
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
