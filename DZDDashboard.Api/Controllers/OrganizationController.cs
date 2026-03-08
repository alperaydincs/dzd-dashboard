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
    public async Task<IActionResult> UpdateCompany(CompanyDto dto)
    {
        await _service.UpdateCompanyAsync(dto);
        return NoContent();
    }

    [HttpDelete("companies/{id}")]
    public async Task<IActionResult> DeleteCompany(int id)
    {
        await _service.DeleteCompanyAsync(id);
        return NoContent();
    }

    [HttpGet("departments")]
    public async Task<ActionResult<List<DepartmentDto>>> GetDepartments() => await _service.GetDepartmentsAsync();

    [HttpPost("departments")]
    public async Task<ActionResult<DepartmentDto>> CreateDepartment(DepartmentDto dto) => await _service.CreateDepartmentAsync(dto);

    [HttpPut("departments")]
    public async Task<IActionResult> UpdateDepartment(DepartmentDto dto)
    {
        await _service.UpdateDepartmentAsync(dto);
        return NoContent();
    }

    [HttpDelete("departments/{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        await _service.DeleteDepartmentAsync(id);
        return NoContent();
    }

    [HttpGet("teams")]
    public async Task<ActionResult<List<TeamDto>>> GetTeams() => await _service.GetTeamsAsync();

    [HttpPost("teams")]
    public async Task<ActionResult<TeamDto>> CreateTeam(TeamDto dto) => await _service.CreateTeamAsync(dto);

    [HttpPut("teams")]
    public async Task<IActionResult> UpdateTeam(TeamDto dto)
    {
        await _service.UpdateTeamAsync(dto);
        return NoContent();
    }

    [HttpDelete("teams/{id}")]
    public async Task<IActionResult> DeleteTeam(int id)
    {
        await _service.DeleteTeamAsync(id);
        return NoContent();
    }

    [HttpGet("worktypes")]
    public async Task<ActionResult<List<WorkTypeDto>>> GetWorkTypes() => await _service.GetWorkTypesAsync();

    [HttpPost("worktypes")]
    public async Task<ActionResult<WorkTypeDto>> CreateWorkType(WorkTypeDto dto) => await _service.CreateWorkTypeAsync(dto);

    [HttpPut("worktypes")]
    public async Task<IActionResult> UpdateWorkType(WorkTypeDto dto)
    {
        await _service.UpdateWorkTypeAsync(dto);
        return NoContent();
    }

    [HttpDelete("worktypes/{id}")]
    public async Task<IActionResult> DeleteWorkType(int id)
    {
        await _service.DeleteWorkTypeAsync(id);
        return NoContent();
    }

    [HttpGet("jobs")]
    public async Task<ActionResult<List<JobDto>>> GetJobs() => await _service.GetJobsAsync();

    [HttpPost("jobs")]
    public async Task<ActionResult<JobDto>> CreateJob(JobDto dto) => await _service.CreateJobAsync(dto);

    [HttpPut("jobs")]
    public async Task<IActionResult> UpdateJob(JobDto dto)
    {
        await _service.UpdateJobAsync(dto);
        return NoContent();
    }

    [HttpDelete("jobs/{id}")]
    public async Task<IActionResult> DeleteJob(int id)
    {
        await _service.DeleteJobAsync(id);
        return NoContent();
    }

    [HttpGet("grades")]
    public async Task<ActionResult<List<GradeDto>>> GetGrades() => await _service.GetGradesAsync();

    [HttpPost("grades")]
    public async Task<ActionResult<GradeDto>> CreateGrade(GradeDto dto) => await _service.CreateGradeAsync(dto);

    [HttpPut("grades")]
    public async Task<IActionResult> UpdateGrade(GradeDto dto)
    {
        await _service.UpdateGradeAsync(dto);
        return NoContent();
    }

    [HttpDelete("grades/{id}")]
    public async Task<IActionResult> DeleteGrade(int id)
    {
        await _service.DeleteGradeAsync(id);
        return NoContent();
    }

    [HttpGet("usergroups")]
    public async Task<ActionResult<List<UserGroupDto>>> GetUserGroups() => await _service.GetUserGroupsAsync();

    [HttpPost("usergroups")]
    public async Task<ActionResult<UserGroupDto>> CreateUserGroup(UserGroupDto dto) => await _service.CreateUserGroupAsync(dto);

    [HttpPut("usergroups")]
    public async Task<IActionResult> UpdateUserGroup(UserGroupDto dto)
    {
        await _service.UpdateUserGroupAsync(dto);
        return NoContent();
    }

    [HttpDelete("usergroups/{id}")]
    public async Task<IActionResult> DeleteUserGroup(int id)
    {
        await _service.DeleteUserGroupAsync(id);
        return NoContent();
    }

    [HttpGet("usergroups/{id}/members")]
    public async Task<ActionResult<UserGroupDto>> GetUserGroupWithMembers(int id) => await _service.GetUserGroupWithMembersAsync(id);
}
