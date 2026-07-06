using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DZDDashboard.Api.Controllers;

[Route("api/organization/departments")]
public class DepartmentController(ICompanyOrgService companyService) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<List<DepartmentDto>>> GetDepartments(CancellationToken cancellationToken)
        => Ok(await companyService.GetDepartmentsAsync(cancellationToken));

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<DepartmentDto>> CreateDepartment([FromBody] DepartmentDto dto, CancellationToken cancellationToken)
    {
        var result = await companyService.CreateDepartmentAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetDepartments), null, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(id, dto.Id) is { } mismatch) return mismatch;
        await companyService.UpdateDepartmentAsync(dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteDepartment(int id, CancellationToken cancellationToken)
    {
        await companyService.DeleteDepartmentAsync(id, cancellationToken);
        return NoContent();
    }
}
