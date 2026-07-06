using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DZDDashboard.Api.Controllers;

[Route("api/organization/jobs")]
public class JobController(IReferenceDataService refDataService) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<List<JobDto>>> GetJobs(CancellationToken cancellationToken)
        => Ok(await refDataService.GetJobsAsync(cancellationToken));

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<JobDto>> CreateJob([FromBody] JobDto dto, CancellationToken cancellationToken)
    {
        var result = await refDataService.CreateJobAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetJobs), null, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateJob(int id, [FromBody] JobDto dto, CancellationToken cancellationToken)
    {
        if (CheckIdMismatch(id, dto.Id) is { } mismatch) return mismatch;
        await refDataService.UpdateJobAsync(dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteJob(int id, CancellationToken cancellationToken)
    {
        await refDataService.DeleteJobAsync(id, cancellationToken);
        return NoContent();
    }
}
