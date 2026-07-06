using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DZDDashboard.Api.Controllers;

[Route("api/organization/teams")]
public class TeamController(ICompanyOrgService companyService) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<List<TeamDto>>> GetTeams(CancellationToken cancellationToken)
        => Ok(await companyService.GetTeamsAsync(cancellationToken));

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<TeamDto>> CreateTeam([FromBody] TeamDto dto, CancellationToken cancellationToken)
    {
        var result = await companyService.CreateTeamAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetTeams), null, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateTeam(int id, [FromBody] TeamDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(id, dto.Id) is { } mismatch) return mismatch;
        await companyService.UpdateTeamAsync(dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteTeam(int id, CancellationToken cancellationToken)
    {
        await companyService.DeleteTeamAsync(id, cancellationToken);
        return NoContent();
    }
}
