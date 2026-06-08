using Microsoft.AspNetCore.Components;
using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Client.Services;

public class OrganizationService : ApiServiceBase, IOrganizationClientService
{
    public OrganizationService(
        IHttpClientFactory httpClientFactory,
        NavigationManager navigationManager)
        : base(httpClientFactory, navigationManager) { }

    // ── Positions ──────────────────────────────────────────────────────────────
    public async Task<List<OrganizationPositionDto>> GetOrganizationPositionsAsync()
        => await GetAsync<List<OrganizationPositionDto>>(ApiRoutes.Organization.Positions) ?? [];

    public async Task<HttpResponseMessage> CreateOrganizationPositionAsync(CreateOrganizationPositionDto dto)
        => await PostAsync(ApiRoutes.Organization.Positions, dto);

    public async Task<HttpResponseMessage> UpdateOrganizationPositionAsync(int id, UpdateOrganizationPositionDto dto)
        => await PutAsync(ApiRoutes.Organization.Position(id), dto);

    public async Task<HttpResponseMessage> DeleteOrganizationPositionAsync(int id)
        => await DeleteAsync(ApiRoutes.Organization.Position(id));

    // ── Companies ──────────────────────────────────────────────────────────────
    public async Task<List<CompanyDto>> GetCompaniesAsync()
        => await GetAsync<List<CompanyDto>>(ApiRoutes.Organization.Companies) ?? [];

    public async Task<HttpResponseMessage> CreateCompanyAsync(CompanyDto dto)
        => await PostAsync(ApiRoutes.Organization.Companies, dto);

    public async Task<HttpResponseMessage> UpdateCompanyAsync(CompanyDto dto)
        => await PutAsync(ApiRoutes.Organization.Company(dto.Id), dto);

    public async Task<HttpResponseMessage> DeleteCompanyAsync(int id)
        => await DeleteAsync(ApiRoutes.Organization.Company(id));

    // ── Departments ────────────────────────────────────────────────────────────
    public async Task<List<DepartmentDto>> GetDepartmentsAsync()
        => await GetAsync<List<DepartmentDto>>(ApiRoutes.Organization.Departments) ?? [];

    public async Task<HttpResponseMessage> CreateDepartmentAsync(DepartmentDto dto)
        => await PostAsync(ApiRoutes.Organization.Departments, dto);

    public async Task<HttpResponseMessage> UpdateDepartmentAsync(DepartmentDto dto)
        => await PutAsync(ApiRoutes.Organization.Department(dto.Id), dto);

    public async Task<HttpResponseMessage> DeleteDepartmentAsync(int id)
        => await DeleteAsync(ApiRoutes.Organization.Department(id));

    // ── Teams ──────────────────────────────────────────────────────────────────
    public async Task<List<TeamDto>> GetTeamsAsync()
        => await GetAsync<List<TeamDto>>(ApiRoutes.Organization.Teams) ?? [];

    public async Task<HttpResponseMessage> CreateTeamAsync(TeamDto dto)
        => await PostAsync(ApiRoutes.Organization.Teams, dto);

    public async Task<HttpResponseMessage> UpdateTeamAsync(TeamDto dto)
        => await PutAsync(ApiRoutes.Organization.Team(dto.Id), dto);

    public async Task<HttpResponseMessage> DeleteTeamAsync(int id)
        => await DeleteAsync(ApiRoutes.Organization.Team(id));

    // ── Work Types ─────────────────────────────────────────────────────────────
    public async Task<List<WorkTypeDto>> GetWorkTypesAsync()
        => await GetAsync<List<WorkTypeDto>>(ApiRoutes.Organization.WorkTypes) ?? [];

    public async Task<HttpResponseMessage> CreateWorkTypeAsync(WorkTypeDto dto)
        => await PostAsync(ApiRoutes.Organization.WorkTypes, dto);

    public async Task<HttpResponseMessage> UpdateWorkTypeAsync(WorkTypeDto dto)
        => await PutAsync(ApiRoutes.Organization.WorkType(dto.Id), dto);

    public async Task<HttpResponseMessage> DeleteWorkTypeAsync(int id)
        => await DeleteAsync(ApiRoutes.Organization.WorkType(id));

    // ── Jobs ───────────────────────────────────────────────────────────────────
    public async Task<List<JobDto>> GetJobsAsync()
        => await GetAsync<List<JobDto>>(ApiRoutes.Organization.Jobs) ?? [];

    public async Task<HttpResponseMessage> CreateJobAsync(JobDto dto)
        => await PostAsync(ApiRoutes.Organization.Jobs, dto);

    public async Task<HttpResponseMessage> UpdateJobAsync(JobDto dto)
        => await PutAsync(ApiRoutes.Organization.Job(dto.Id), dto);

    public async Task<HttpResponseMessage> DeleteJobAsync(int id)
        => await DeleteAsync(ApiRoutes.Organization.Job(id));

    // ── Grades ─────────────────────────────────────────────────────────────────
    public async Task<List<GradeDto>> GetGradesAsync()
        => await GetAsync<List<GradeDto>>(ApiRoutes.Organization.Grades) ?? [];

    public async Task<HttpResponseMessage> CreateGradeAsync(GradeDto dto)
        => await PostAsync(ApiRoutes.Organization.Grades, dto);

    public async Task<HttpResponseMessage> UpdateGradeAsync(GradeDto dto)
        => await PutAsync(ApiRoutes.Organization.Grade(dto.Id), dto);

    public async Task<HttpResponseMessage> DeleteGradeAsync(int id)
        => await DeleteAsync(ApiRoutes.Organization.Grade(id));

    // ── Payroll Locations ──────────────────────────────────────────────────────
    public async Task<List<PayrollLocationDto>> GetPayrollLocationsAsync()
        => await GetAsync<List<PayrollLocationDto>>(ApiRoutes.Organization.PayrollLocations) ?? [];

    public async Task<HttpResponseMessage> CreatePayrollLocationAsync(PayrollLocationDto dto)
        => await PostAsync(ApiRoutes.Organization.PayrollLocations, dto);

    public async Task<HttpResponseMessage> UpdatePayrollLocationAsync(PayrollLocationDto dto)
        => await PutAsync(ApiRoutes.Organization.PayrollLocation(dto.Id), dto);

    public async Task<HttpResponseMessage> DeletePayrollLocationAsync(int id)
        => await DeleteAsync(ApiRoutes.Organization.PayrollLocation(id));

    // ── User Groups ────────────────────────────────────────────────────────────
    public async Task<List<UserGroupDto>> GetUserGroupsAsync()
        => await GetAsync<List<UserGroupDto>>(ApiRoutes.Organization.UserGroups) ?? [];

    public async Task<HttpResponseMessage> CreateUserGroupAsync(UserGroupDto dto)
        => await PostAsync(ApiRoutes.Organization.UserGroups, dto);

    public async Task<HttpResponseMessage> UpdateUserGroupAsync(UserGroupDto dto)
        => await PutAsync(ApiRoutes.Organization.UserGroup(dto.Id), dto);

    public async Task<HttpResponseMessage> DeleteUserGroupAsync(int id)
        => await DeleteAsync(ApiRoutes.Organization.UserGroup(id));

    // ── Career Paths ───────────────────────────────────────────────────────────
    public async Task<List<CareerPathDto>> GetCareerPathsAsync()
        => await GetAsync<List<CareerPathDto>>(ApiRoutes.Organization.CareerPaths) ?? [];

    public async Task<HttpResponseMessage> CreateCareerPathAsync(CareerPathDto dto)
        => await PostAsync(ApiRoutes.Organization.CareerPaths, dto);

    public async Task<HttpResponseMessage> UpdateCareerPathAsync(CareerPathDto dto)
        => await PutAsync(ApiRoutes.Organization.CareerPath(dto.Id), dto);

    public async Task<HttpResponseMessage> DeleteCareerPathAsync(int id)
        => await DeleteAsync(ApiRoutes.Organization.CareerPath(id));

    // ── Career Map Rules ───────────────────────────────────────────────────────
    public async Task<HttpResponseMessage> CreateCareerMapRuleAsync(CareerMapRuleDto dto)
        => await PostAsync(ApiRoutes.Organization.CareerMapRules, dto);

    public async Task<HttpResponseMessage> UpdateCareerMapRuleAsync(CareerMapRuleDto dto)
        => await PutAsync(ApiRoutes.Organization.CareerMapRule(dto.Id), dto);

    public async Task<HttpResponseMessage> DeleteCareerMapRuleAsync(int id)
        => await DeleteAsync(ApiRoutes.Organization.CareerMapRule(id));
}
