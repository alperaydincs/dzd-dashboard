using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DZDDashboard.Api.Controllers;

[Route("api/organization/payrolllocations")]
public class PayrollLocationController(IReferenceDataService refDataService) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<List<PayrollLocationDto>>> GetPayrollLocations(CancellationToken cancellationToken)
        => Ok(await refDataService.GetPayrollLocationsAsync(cancellationToken));

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<PayrollLocationDto>> CreatePayrollLocation([FromBody] PayrollLocationDto dto, CancellationToken cancellationToken)
    {
        var result = await refDataService.CreatePayrollLocationAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetPayrollLocations), null, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdatePayrollLocation(int id, [FromBody] PayrollLocationDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(id, dto.Id) is { } mismatch) return mismatch;
        await refDataService.UpdatePayrollLocationAsync(dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeletePayrollLocation(int id, CancellationToken cancellationToken)
    {
        await refDataService.DeletePayrollLocationAsync(id, cancellationToken);
        return NoContent();
    }
}
