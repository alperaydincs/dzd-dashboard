using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DZDDashboard.Api.Controllers;

[Route("api/organization")]
public class ReferenceDataController(IReferenceDataService refDataService) : BaseController
{

    [HttpGet("jobs")]
    public async Task<ActionResult<List<JobDto>>> GetJobs(CancellationToken cancellationToken)
        => Ok(await refDataService.GetJobsAsync(cancellationToken));

    [HttpPost("jobs")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<JobDto>> CreateJob([FromBody] JobDto dto, CancellationToken cancellationToken)
    {
        var result = await refDataService.CreateJobAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetJobs), null, result);
    }

    [HttpPut("jobs/{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateJob(int id, [FromBody] JobDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(id, dto.Id) is { } mismatch) return mismatch;
        await refDataService.UpdateJobAsync(dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("jobs/{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteJob(int id, CancellationToken cancellationToken)
    {
        await refDataService.DeleteJobAsync(id, cancellationToken);
        return NoContent();
    }


    [HttpGet("grades")]
    public async Task<ActionResult<List<GradeDto>>> GetGrades(CancellationToken cancellationToken)
        => Ok(await refDataService.GetGradesAsync(cancellationToken));

    [HttpPost("grades")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<GradeDto>> CreateGrade([FromBody] GradeDto dto, CancellationToken cancellationToken)
    {
        var result = await refDataService.CreateGradeAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetGrades), null, result);
    }

    [HttpPut("grades/{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateGrade(int id, [FromBody] GradeDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(id, dto.Id) is { } mismatch) return mismatch;
        await refDataService.UpdateGradeAsync(dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("grades/{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteGrade(int id, CancellationToken cancellationToken)
    {
        await refDataService.DeleteGradeAsync(id, cancellationToken);
        return NoContent();
    }


    [HttpGet("payrolllocations")]
    public async Task<ActionResult<List<PayrollLocationDto>>> GetPayrollLocations(CancellationToken cancellationToken)
        => Ok(await refDataService.GetPayrollLocationsAsync(cancellationToken));

    [HttpPost("payrolllocations")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<PayrollLocationDto>> CreatePayrollLocation([FromBody] PayrollLocationDto dto, CancellationToken cancellationToken)
    {
        var result = await refDataService.CreatePayrollLocationAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetPayrollLocations), null, result);
    }

    [HttpPut("payrolllocations/{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdatePayrollLocation(int id, [FromBody] PayrollLocationDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(id, dto.Id) is { } mismatch) return mismatch;
        await refDataService.UpdatePayrollLocationAsync(dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("payrolllocations/{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeletePayrollLocation(int id, CancellationToken cancellationToken)
    {
        await refDataService.DeletePayrollLocationAsync(id, cancellationToken);
        return NoContent();
    }


    [HttpGet("usergroups")]
    public async Task<ActionResult<List<UserGroupDto>>> GetUserGroups(CancellationToken cancellationToken)
        => Ok(await refDataService.GetUserGroupsAsync(cancellationToken));

    [HttpPost("usergroups")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<UserGroupDto>> CreateUserGroup([FromBody] UserGroupDto dto, CancellationToken cancellationToken)
    {
        var result = await refDataService.CreateUserGroupAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetUserGroups), null, result);
    }

    [HttpPut("usergroups/{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateUserGroup(int id, [FromBody] UserGroupDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(id, dto.Id) is { } mismatch) return mismatch;
        await refDataService.UpdateUserGroupAsync(dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("usergroups/{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteUserGroup(int id, CancellationToken cancellationToken)
    {
        await refDataService.DeleteUserGroupAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpGet("usergroups/{id}")]
    public async Task<ActionResult<UserGroupDto>> GetUserGroupById(int id, CancellationToken cancellationToken)
        => Ok(await refDataService.GetUserGroupByIdAsync(id, cancellationToken));

}
