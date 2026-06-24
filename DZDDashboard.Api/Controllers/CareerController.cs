using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DZDDashboard.Api.Controllers;

[Route("api/organization")]
public class CareerController(ICareerPathService careerPathService) : BaseController
{

    [HttpGet("careerpaths")]
    public async Task<ActionResult<List<CareerPathDto>>> GetCareerPaths(CancellationToken cancellationToken)
        => Ok(await careerPathService.GetCareerPathsAsync(cancellationToken));

    [HttpPost("careerpaths")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<ActionResult<CareerPathDto>> CreateCareerPath([FromBody] CareerPathDto dto, CancellationToken cancellationToken)
    {
        var result = await careerPathService.CreateCareerPathAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetCareerPaths), null, result);
    }

    [HttpPut("careerpaths/{id}")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<IActionResult> UpdateCareerPath(int id, [FromBody] CareerPathDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(id, dto.Id) is { } mismatch) return mismatch;
        await careerPathService.UpdateCareerPathAsync(dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("careerpaths/{id}")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<IActionResult> DeleteCareerPath(int id, CancellationToken cancellationToken)
    {
        await careerPathService.DeleteCareerPathAsync(id, cancellationToken);
        return NoContent();
    }


    [HttpPost("careermaprules")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<ActionResult<CareerMapRuleDto>> CreateCareerMapRule([FromBody] CareerMapRuleDto dto, CancellationToken cancellationToken)
    {
        var result = await careerPathService.CreateCareerMapRuleAsync(dto, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPut("careermaprules/{id}")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<IActionResult> UpdateCareerMapRule(int id, [FromBody] CareerMapRuleDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(id, dto.Id) is { } mismatch) return mismatch;
        await careerPathService.UpdateCareerMapRuleAsync(dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("careermaprules/{id}")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<IActionResult> DeleteCareerMapRule(int id, CancellationToken cancellationToken)
    {
        await careerPathService.DeleteCareerMapRuleAsync(id, cancellationToken);
        return NoContent();
    }
}
