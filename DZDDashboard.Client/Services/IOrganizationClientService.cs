using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Client.Services;

public interface IOrganizationClientService
{
    Task<List<OrganizationPositionDto>> GetOrganizationPositionsAsync();
    Task<HttpResponseMessage> CreateOrganizationPositionAsync(CreateOrganizationPositionDto dto);
    Task<HttpResponseMessage> UpdateOrganizationPositionAsync(int id, UpdateOrganizationPositionDto dto);
    Task<HttpResponseMessage> DeleteOrganizationPositionAsync(int id);

    Task<List<CompanyDto>> GetCompaniesAsync();
    Task<HttpResponseMessage> CreateCompanyAsync(CompanyDto dto);
    Task<HttpResponseMessage> UpdateCompanyAsync(CompanyDto dto);
    Task<HttpResponseMessage> DeleteCompanyAsync(int id);

    Task<List<DepartmentDto>> GetDepartmentsAsync();
    Task<HttpResponseMessage> CreateDepartmentAsync(DepartmentDto dto);
    Task<HttpResponseMessage> UpdateDepartmentAsync(DepartmentDto dto);
    Task<HttpResponseMessage> DeleteDepartmentAsync(int id);

    Task<List<TeamDto>> GetTeamsAsync();
    Task<HttpResponseMessage> CreateTeamAsync(TeamDto dto);
    Task<HttpResponseMessage> UpdateTeamAsync(TeamDto dto);
    Task<HttpResponseMessage> DeleteTeamAsync(int id);

    Task<List<JobDto>> GetJobsAsync();
    Task<HttpResponseMessage> CreateJobAsync(JobDto dto);
    Task<HttpResponseMessage> UpdateJobAsync(JobDto dto);
    Task<HttpResponseMessage> DeleteJobAsync(int id);

    Task<List<GradeDto>> GetGradesAsync();
    Task<HttpResponseMessage> CreateGradeAsync(GradeDto dto);
    Task<HttpResponseMessage> UpdateGradeAsync(GradeDto dto);
    Task<HttpResponseMessage> DeleteGradeAsync(int id);

    Task<List<PayrollLocationDto>> GetPayrollLocationsAsync();
    Task<HttpResponseMessage> CreatePayrollLocationAsync(PayrollLocationDto dto);
    Task<HttpResponseMessage> UpdatePayrollLocationAsync(PayrollLocationDto dto);
    Task<HttpResponseMessage> DeletePayrollLocationAsync(int id);

    Task<List<UserGroupDto>> GetUserGroupsAsync();
    Task<HttpResponseMessage> CreateUserGroupAsync(UserGroupDto dto);
    Task<HttpResponseMessage> UpdateUserGroupAsync(UserGroupDto dto);
    Task<HttpResponseMessage> DeleteUserGroupAsync(int id);

    Task<List<CareerPathDto>> GetCareerPathsAsync();
    Task<HttpResponseMessage> CreateCareerPathAsync(CareerPathDto dto);
    Task<HttpResponseMessage> UpdateCareerPathAsync(CareerPathDto dto);
    Task<HttpResponseMessage> DeleteCareerPathAsync(int id);

    Task<HttpResponseMessage> CreateCareerMapRuleAsync(CareerMapRuleDto dto);
    Task<HttpResponseMessage> UpdateCareerMapRuleAsync(CareerMapRuleDto dto);
    Task<HttpResponseMessage> DeleteCareerMapRuleAsync(int id);
}
