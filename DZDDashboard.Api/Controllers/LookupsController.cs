using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DZDDashboard.Api.Controllers;

[Route("api/lookups")]
public class LookupsController(ILookupService lookups) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<List<LookupValueDto>>> List([FromQuery] string category, CancellationToken cancellationToken)
        => Ok(await lookups.GetAsync(category, cancellationToken));

    [HttpPost]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<ActionResult<LookupValueDto>> Create([FromBody] LookupValueDto dto, CancellationToken cancellationToken)
        => Ok(await lookups.CreateAsync(dto, cancellationToken));

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<IActionResult> Update(int id, [FromBody] LookupValueDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(id, dto.Id) is { } mismatch) return mismatch;
        await lookups.UpdateAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.AdminOrHr)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await lookups.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
