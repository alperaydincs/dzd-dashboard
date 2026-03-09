using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DZDDashboard.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin,HR")]
public class OrganizationController : ControllerBase
{
    private readonly IOrganizationService _service;

    public OrganizationController(IOrganizationService service)
    {
        _service = service;
    }

    [HttpGet("companies")]
    public async Task<ActionResult<List<CompanyDto>>> GetCompanies() => await _service.GetCompaniesAsync();

    [HttpPost("companies")]
    public async Task<ActionResult<CompanyDto>> CreateCompany(CompanyDto dto) => await _service.CreateCompanyAsync(dto);

    [HttpPut("companies")]
    public Task<IActionResult> UpdateCompany(CompanyDto dto)
        => ExecuteNoContent(() => _service.UpdateCompanyAsync(dto));

    [HttpDelete("companies/{id}")]
    public Task<IActionResult> DeleteCompany(int id)
        => ExecuteNoContent(() => _service.DeleteCompanyAsync(id));

    [HttpGet("departments")]
    public async Task<ActionResult<List<DepartmentDto>>> GetDepartments() => await _service.GetDepartmentsAsync();

    [HttpPost("departments")]
    public async Task<ActionResult<DepartmentDto>> CreateDepartment(DepartmentDto dto) => await _service.CreateDepartmentAsync(dto);

    [HttpPut("departments")]
    public Task<IActionResult> UpdateDepartment(DepartmentDto dto)
        => ExecuteNoContent(() => _service.UpdateDepartmentAsync(dto));

    [HttpDelete("departments/{id}")]
    public Task<IActionResult> DeleteDepartment(int id)
        => ExecuteNoContent(() => _service.DeleteDepartmentAsync(id));

    [HttpGet("teams")]
    public async Task<ActionResult<List<TeamDto>>> GetTeams() => await _service.GetTeamsAsync();

    [HttpPost("teams")]
    public async Task<ActionResult<TeamDto>> CreateTeam(TeamDto dto) => await _service.CreateTeamAsync(dto);

    [HttpPut("teams")]
    public Task<IActionResult> UpdateTeam(TeamDto dto)
        => ExecuteNoContent(() => _service.UpdateTeamAsync(dto));

    [HttpDelete("teams/{id}")]
    public Task<IActionResult> DeleteTeam(int id)
        => ExecuteNoContent(() => _service.DeleteTeamAsync(id));

    [HttpGet("worktypes")]
    public async Task<ActionResult<List<WorkTypeDto>>> GetWorkTypes() => await _service.GetWorkTypesAsync();

    [HttpPost("worktypes")]
    public async Task<ActionResult<WorkTypeDto>> CreateWorkType(WorkTypeDto dto) => await _service.CreateWorkTypeAsync(dto);

    [HttpPut("worktypes")]
    public Task<IActionResult> UpdateWorkType(WorkTypeDto dto)
        => ExecuteNoContent(() => _service.UpdateWorkTypeAsync(dto));

    [HttpDelete("worktypes/{id}")]
    public Task<IActionResult> DeleteWorkType(int id)
        => ExecuteNoContent(() => _service.DeleteWorkTypeAsync(id));

    [HttpGet("jobs")]
    public async Task<ActionResult<List<JobDto>>> GetJobs() => await _service.GetJobsAsync();

    [HttpPost("jobs")]
    public async Task<ActionResult<JobDto>> CreateJob(JobDto dto) => await _service.CreateJobAsync(dto);

    [HttpPut("jobs")]
    public Task<IActionResult> UpdateJob(JobDto dto)
        => ExecuteNoContent(() => _service.UpdateJobAsync(dto));

    [HttpDelete("jobs/{id}")]
    public Task<IActionResult> DeleteJob(int id)
        => ExecuteNoContent(() => _service.DeleteJobAsync(id));

    [HttpGet("grades")]
    public async Task<ActionResult<List<GradeDto>>> GetGrades() => await _service.GetGradesAsync();

    [HttpPost("grades")]
    public async Task<ActionResult<GradeDto>> CreateGrade(GradeDto dto) => await _service.CreateGradeAsync(dto);

    [HttpPut("grades")]
    public Task<IActionResult> UpdateGrade(GradeDto dto)
        => ExecuteNoContent(() => _service.UpdateGradeAsync(dto));

    [HttpDelete("grades/{id}")]
    public Task<IActionResult> DeleteGrade(int id)
        => ExecuteNoContent(() => _service.DeleteGradeAsync(id));

    [HttpGet("payrolllocations")]
    public async Task<ActionResult<List<PayrollLocationDto>>> GetPayrollLocations() => await _service.GetPayrollLocationsAsync();

    [HttpGet("usergroups")]
    public async Task<ActionResult<List<UserGroupDto>>> GetUserGroups() => await _service.GetUserGroupsAsync();

    [HttpPost("usergroups")]
    public async Task<ActionResult<UserGroupDto>> CreateUserGroup(UserGroupDto dto) => await _service.CreateUserGroupAsync(dto);

    [HttpPut("usergroups")]
    public Task<IActionResult> UpdateUserGroup(UserGroupDto dto)
        => ExecuteNoContent(() => _service.UpdateUserGroupAsync(dto));

    [HttpDelete("usergroups/{id}")]
    public Task<IActionResult> DeleteUserGroup(int id)
        => ExecuteNoContent(() => _service.DeleteUserGroupAsync(id));

    [HttpGet("usergroups/{id}/members")]
    public async Task<ActionResult<UserGroupDto>> GetUserGroupWithMembers(int id) => await _service.GetUserGroupWithMembersAsync(id);

    [HttpGet("positions")]
    public async Task<ActionResult<List<OrganizationPositionDto>>> GetPositions() => await _service.GetAllPositionsAsync();

    [HttpPost("positions")]
    public async Task<IActionResult> CreatePosition(CreateOrganizationPositionDto dto)
    {
        try
        {
            var result = await _service.CreatePositionAsync(dto);
            return CreatedAtAction(nameof(GetPositions), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Unexpected server error." });
        }
    }

    [HttpPut("positions/{id}")]
    public async Task<IActionResult> UpdatePosition(int id, UpdateOrganizationPositionDto dto)
    {
        if (id != dto.Id) return BadRequest("Id mismatch");

        try
        {
            var result = await _service.UpdatePositionAsync(dto);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Unexpected server error." });
        }
    }

    [HttpDelete("positions/{id}")]
    public async Task<IActionResult> DeletePosition(int id)
    {
        try
        {
            await _service.DeletePositionAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Unexpected server error." });
        }
    }

    private async Task<IActionResult> ExecuteNoContent(Func<Task> action)
    {
        await action();
        return NoContent();
    }
}
