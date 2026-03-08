using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Client.Services;

public class OrganizationService : ApiServiceBase
{
    public OrganizationService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<List<OrganizationPositionDto>> GetOrganizationPositionsAsync()
        => await GetAsync<List<OrganizationPositionDto>>("api/organization/positions") ?? new();

    public async Task<List<CompanyDto>> GetCompaniesAsync()
        => await GetAsync<List<CompanyDto>>("api/organization/companies") ?? new();

    public async Task<List<DepartmentDto>> GetDepartmentsAsync()
        => await GetAsync<List<DepartmentDto>>("api/organization/departments") ?? new();

    public async Task<List<TeamDto>> GetTeamsAsync()
        => await GetAsync<List<TeamDto>>("api/organization/teams") ?? new();

    public async Task<List<WorkTypeDto>> GetWorkTypesAsync()
        => await GetAsync<List<WorkTypeDto>>("api/organization/worktypes") ?? new();

    public async Task<List<JobDto>> GetJobsAsync()
        => await GetAsync<List<JobDto>>("api/organization/jobs") ?? new();

    public async Task<List<GradeDto>> GetGradesAsync()
        => await GetAsync<List<GradeDto>>("api/organization/grades") ?? new();

    public async Task<HttpResponseMessage> CreateOrganizationPositionAsync(CreateOrganizationPositionDto dto)
        => await PostAsync("api/organization/positions", dto);

    public async Task<HttpResponseMessage> UpdateOrganizationPositionAsync(int positionId, UpdateOrganizationPositionDto dto)
        => await PutAsync($"api/organization/positions/{positionId}", dto);

    public async Task<HttpResponseMessage> DeleteOrganizationPositionAsync(int positionId)
        => await DeleteAsync($"api/organization/positions/{positionId}");

    public async Task<HttpResponseMessage> DeleteCompanyAsync(int companyId)
        => await DeleteAsync($"api/organization/companies/{companyId}");

    public async Task<HttpResponseMessage> DeleteDepartmentAsync(int departmentId)
        => await DeleteAsync($"api/organization/departments/{departmentId}");

    public async Task<HttpResponseMessage> DeleteTeamAsync(int teamId)
        => await DeleteAsync($"api/organization/teams/{teamId}");

    public async Task<HttpResponseMessage> DeleteWorkTypeAsync(int workTypeId)
        => await DeleteAsync($"api/organization/worktypes/{workTypeId}");

    public async Task<HttpResponseMessage> DeleteJobAsync(int jobId)
        => await DeleteAsync($"api/organization/jobs/{jobId}");

    public async Task<HttpResponseMessage> DeleteGradeAsync(int gradeId)
        => await DeleteAsync($"api/organization/grades/{gradeId}");

    public async Task<List<UserGroupDto>> GetUserGroupsAsync()
        => await GetAsync<List<UserGroupDto>>("api/organization/usergroups") ?? new();

    public async Task<HttpResponseMessage> CreateUserGroupAsync(UserGroupDto dto)
        => await PostAsync("api/organization/usergroups", dto);

    public async Task<HttpResponseMessage> UpdateUserGroupAsync(UserGroupDto dto)
        => await PutAsync("api/organization/usergroups", dto);

    public async Task<HttpResponseMessage> DeleteUserGroupAsync(int userGroupId)
        => await DeleteAsync($"api/organization/usergroups/{userGroupId}");

    public async Task<UserGroupDto?> GetUserGroupWithMembersAsync(int userGroupId)
        => await GetAsync<UserGroupDto>($"api/organization/usergroups/{userGroupId}/members");
}
