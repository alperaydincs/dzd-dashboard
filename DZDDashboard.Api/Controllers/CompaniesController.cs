using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DZDDashboard.Api.Controllers;

/// <summary>
/// Manages the company organisational hierarchy: companies, departments, and teams.
/// Route kept at <c>api/organization</c> for backward compatibility with existing API clients.
/// </summary>
[Route("api/organization")]
public class CompaniesController(ICompanyOrgService companyService) : BaseController
{
    // ── Companies ──────────────────────────────────────────────────────────────

    [HttpGet("companies")]
    public async Task<ActionResult<List<CompanyDto>>> GetCompanies(CancellationToken cancellationToken)
        => Ok(await companyService.GetCompaniesAsync(cancellationToken));

    [HttpPost("companies")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<CompanyDto>> CreateCompany([FromBody] CompanyDto dto, CancellationToken cancellationToken)
    {
        var result = await companyService.CreateCompanyAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetCompanies), null, result);
    }

    [HttpPut("companies/{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateCompany(int id, [FromBody] CompanyDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(id, dto.Id) is { } mismatch) return mismatch;
        await companyService.UpdateCompanyAsync(dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("companies/{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteCompany(int id, CancellationToken cancellationToken)
    {
        await companyService.DeleteCompanyAsync(id, cancellationToken);
        return NoContent();
    }

    // ── Departments ────────────────────────────────────────────────────────────

    [HttpGet("departments")]
    public async Task<ActionResult<List<DepartmentDto>>> GetDepartments(CancellationToken cancellationToken)
        => Ok(await companyService.GetDepartmentsAsync(cancellationToken));

    [HttpPost("departments")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<DepartmentDto>> CreateDepartment([FromBody] DepartmentDto dto, CancellationToken cancellationToken)
    {
        var result = await companyService.CreateDepartmentAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetDepartments), null, result);
    }

    [HttpPut("departments/{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(id, dto.Id) is { } mismatch) return mismatch;
        await companyService.UpdateDepartmentAsync(dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("departments/{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteDepartment(int id, CancellationToken cancellationToken)
    {
        await companyService.DeleteDepartmentAsync(id, cancellationToken);
        return NoContent();
    }

    // ── Teams ──────────────────────────────────────────────────────────────────

    [HttpGet("teams")]
    public async Task<ActionResult<List<TeamDto>>> GetTeams(CancellationToken cancellationToken)
        => Ok(await companyService.GetTeamsAsync(cancellationToken));

    [HttpPost("teams")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<TeamDto>> CreateTeam([FromBody] TeamDto dto, CancellationToken cancellationToken)
    {
        var result = await companyService.CreateTeamAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetTeams), null, result);
    }

    [HttpPut("teams/{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateTeam(int id, [FromBody] TeamDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(id, dto.Id) is { } mismatch) return mismatch;
        await companyService.UpdateTeamAsync(dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("teams/{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteTeam(int id, CancellationToken cancellationToken)
    {
        await companyService.DeleteTeamAsync(id, cancellationToken);
        return NoContent();
    }
}
