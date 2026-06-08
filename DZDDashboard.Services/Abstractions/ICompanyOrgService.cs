using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Services;

/// <summary>
/// Manages the company organisational hierarchy: companies, departments, and teams.
/// </summary>
public interface ICompanyOrgService
{
    Task<List<CompanyDto>>    GetCompaniesAsync(CancellationToken cancellationToken = default);
    Task<CompanyDto>          CreateCompanyAsync(CompanyDto dto, CancellationToken cancellationToken = default);
    Task                      UpdateCompanyAsync(CompanyDto dto, CancellationToken cancellationToken = default);
    Task                      DeleteCompanyAsync(int id, CancellationToken cancellationToken = default);

    Task<List<DepartmentDto>> GetDepartmentsAsync(CancellationToken cancellationToken = default);
    Task<DepartmentDto>       CreateDepartmentAsync(DepartmentDto dto, CancellationToken cancellationToken = default);
    Task                      UpdateDepartmentAsync(DepartmentDto dto, CancellationToken cancellationToken = default);
    Task                      DeleteDepartmentAsync(int id, CancellationToken cancellationToken = default);

    Task<List<TeamDto>>       GetTeamsAsync(CancellationToken cancellationToken = default);
    Task<TeamDto>             CreateTeamAsync(TeamDto dto, CancellationToken cancellationToken = default);
    Task                      UpdateTeamAsync(TeamDto dto, CancellationToken cancellationToken = default);
    Task                      DeleteTeamAsync(int id, CancellationToken cancellationToken = default);
}
