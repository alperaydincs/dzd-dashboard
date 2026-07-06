using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DZDDashboard.Api.Controllers;

[Route("api/organization/companies")]
public class CompanyController(ICompanyOrgService companyService) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<List<CompanyDto>>> GetCompanies(CancellationToken cancellationToken)
        => Ok(await companyService.GetCompaniesAsync(cancellationToken));

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<CompanyDto>> CreateCompany([FromBody] CompanyDto dto, CancellationToken cancellationToken)
    {
        var result = await companyService.CreateCompanyAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetCompanies), null, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateCompany(int id, [FromBody] CompanyDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(id, dto.Id) is { } mismatch) return mismatch;
        await companyService.UpdateCompanyAsync(dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteCompany(int id, CancellationToken cancellationToken)
    {
        await companyService.DeleteCompanyAsync(id, cancellationToken);
        return NoContent();
    }
}
