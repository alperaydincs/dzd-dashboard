using DZDDashboard.Common.DTOs;
using DZDDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DZDDashboard.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrganizationController : BaseController
{
    private readonly IOrganizationService _service;

    public OrganizationController(IOrganizationService service)
    {
        _service = service;
    }

    [HttpGet("companies")]
    public async Task<ActionResult<List<CompanyDto>>> GetCompanies() => await _service.GetCompaniesAsync();

    [HttpPost("companies")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CompanyDto>> CreateCompany(CompanyDto dto) => await _service.CreateCompanyAsync(dto);

    [HttpPut("companies")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> UpdateCompany(CompanyDto dto)
        => ExecuteNoContent(() => _service.UpdateCompanyAsync(dto), "Update company");

    [HttpDelete("companies/{id}")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> DeleteCompany(int id)
        => ExecuteNoContent(() => _service.DeleteCompanyAsync(id), "Delete company");

    [HttpGet("departments")]
    public async Task<ActionResult<List<DepartmentDto>>> GetDepartments() => await _service.GetDepartmentsAsync();

    [HttpPost("departments")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DepartmentDto>> CreateDepartment(DepartmentDto dto) => await _service.CreateDepartmentAsync(dto);

    [HttpPut("departments")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> UpdateDepartment(DepartmentDto dto)
        => ExecuteNoContent(() => _service.UpdateDepartmentAsync(dto), "Update department");

    [HttpDelete("departments/{id}")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> DeleteDepartment(int id)
        => ExecuteNoContent(() => _service.DeleteDepartmentAsync(id), "Delete department");

    [HttpGet("teams")]
    public async Task<ActionResult<List<TeamDto>>> GetTeams() => await _service.GetTeamsAsync();

    [HttpPost("teams")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<TeamDto>> CreateTeam(TeamDto dto) => await _service.CreateTeamAsync(dto);

    [HttpPut("teams")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> UpdateTeam(TeamDto dto)
        => ExecuteNoContent(() => _service.UpdateTeamAsync(dto), "Update team");

    [HttpDelete("teams/{id}")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> DeleteTeam(int id)
        => ExecuteNoContent(() => _service.DeleteTeamAsync(id), "Delete team");

    [HttpGet("worktypes")]
    public async Task<ActionResult<List<WorkTypeDto>>> GetWorkTypes() => await _service.GetWorkTypesAsync();

    [HttpPost("worktypes")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<WorkTypeDto>> CreateWorkType(WorkTypeDto dto) => await _service.CreateWorkTypeAsync(dto);

    [HttpPut("worktypes")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> UpdateWorkType(WorkTypeDto dto)
        => ExecuteNoContent(() => _service.UpdateWorkTypeAsync(dto), "Update work type");

    [HttpDelete("worktypes/{id}")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> DeleteWorkType(int id)
        => ExecuteNoContent(() => _service.DeleteWorkTypeAsync(id), "Delete work type");

    [HttpGet("jobs")]
    public async Task<ActionResult<List<JobDto>>> GetJobs() => await _service.GetJobsAsync();

    [HttpPost("jobs")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<JobDto>> CreateJob(JobDto dto) => await _service.CreateJobAsync(dto);

    [HttpPut("jobs")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> UpdateJob(JobDto dto)
        => ExecuteNoContent(() => _service.UpdateJobAsync(dto), "Update job");

    [HttpDelete("jobs/{id}")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> DeleteJob(int id)
        => ExecuteNoContent(() => _service.DeleteJobAsync(id), "Delete job");

    [HttpGet("grades")]
    public async Task<ActionResult<List<GradeDto>>> GetGrades() => await _service.GetGradesAsync();

    [HttpPost("grades")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GradeDto>> CreateGrade(GradeDto dto) => await _service.CreateGradeAsync(dto);

    [HttpPut("grades")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> UpdateGrade(GradeDto dto)
        => ExecuteNoContent(() => _service.UpdateGradeAsync(dto), "Update grade");

    [HttpDelete("grades/{id}")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> DeleteGrade(int id)
        => ExecuteNoContent(() => _service.DeleteGradeAsync(id), "Delete grade");

    [HttpGet("payrolllocations")]
    public async Task<ActionResult<List<PayrollLocationDto>>> GetPayrollLocations() => await _service.GetPayrollLocationsAsync();

    [HttpPost("payrolllocations")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PayrollLocationDto>> CreatePayrollLocation(PayrollLocationDto dto) => await _service.CreatePayrollLocationAsync(dto);

    [HttpPut("payrolllocations")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> UpdatePayrollLocation(PayrollLocationDto dto)
        => ExecuteNoContent(() => _service.UpdatePayrollLocationAsync(dto), "Update payroll location");

    [HttpDelete("payrolllocations/{id}")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> DeletePayrollLocation(int id)
        => ExecuteNoContent(() => _service.DeletePayrollLocationAsync(id), "Delete payroll location");

    [HttpGet("usergroups")]
    public async Task<ActionResult<List<UserGroupDto>>> GetUserGroups() => await _service.GetUserGroupsAsync();

    [HttpPost("usergroups")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserGroupDto>> CreateUserGroup(UserGroupDto dto) => await _service.CreateUserGroupAsync(dto);

    [HttpPut("usergroups")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> UpdateUserGroup(UserGroupDto dto)
        => ExecuteNoContent(() => _service.UpdateUserGroupAsync(dto), "Update user group");

    [HttpDelete("usergroups/{id}")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> DeleteUserGroup(int id)
        => ExecuteNoContent(() => _service.DeleteUserGroupAsync(id), "Delete user group");

    [HttpGet("usergroups/{id}/members")]
    public async Task<ActionResult<UserGroupDto>> GetUserGroupWithMembers(int id) => await _service.GetUserGroupWithMembersAsync(id);

    [HttpGet("positions")]
    public async Task<ActionResult<List<OrganizationPositionDto>>> GetPositions() => await _service.GetAllPositionsAsync();

    [HttpPost("positions")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreatePosition(CreateOrganizationPositionDto dto)
    {
        try
        {
            var result = await _service.CreatePositionAsync(dto);
            return CreatedAtAction(nameof(GetPositions), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "Create position");
        }
    }

    [HttpPut("positions/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdatePosition(int id, UpdateOrganizationPositionDto dto)
    {
        if (id != dto.Id) return BadRequest("Id mismatch");

        try
        {
            var result = await _service.UpdatePositionAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "Update position");
        }
    }

    [HttpDelete("positions/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePosition(int id)
    {
        try
        {
            await _service.DeletePositionAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return HandleException(ex, "Delete position");
        }
    }

    private async Task<IActionResult> ExecuteNoContent(Func<Task> action, string operationName)
    {
        try
        {
            await action();
            return NoContent();
        }
        catch (Exception ex)
        {
            return HandleException(ex, operationName);
        }
    }
}
