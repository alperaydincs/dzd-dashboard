using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Services;

/// <summary>
/// Manages flat reference / lookup data: work types, jobs, grades,
/// payroll locations, and user groups.
/// </summary>
public interface IReferenceDataService
{
    Task<List<WorkTypeDto>>        GetWorkTypesAsync(CancellationToken cancellationToken = default);
    Task<WorkTypeDto>              CreateWorkTypeAsync(WorkTypeDto dto, CancellationToken cancellationToken = default);
    Task                           UpdateWorkTypeAsync(WorkTypeDto dto, CancellationToken cancellationToken = default);
    Task                           DeleteWorkTypeAsync(int id, CancellationToken cancellationToken = default);

    Task<List<JobDto>>             GetJobsAsync(CancellationToken cancellationToken = default);
    Task<JobDto>                   CreateJobAsync(JobDto dto, CancellationToken cancellationToken = default);
    Task                           UpdateJobAsync(JobDto dto, CancellationToken cancellationToken = default);
    Task                           DeleteJobAsync(int id, CancellationToken cancellationToken = default);

    Task<List<GradeDto>>           GetGradesAsync(CancellationToken cancellationToken = default);
    Task<GradeDto>                 CreateGradeAsync(GradeDto dto, CancellationToken cancellationToken = default);
    Task                           UpdateGradeAsync(GradeDto dto, CancellationToken cancellationToken = default);
    Task                           DeleteGradeAsync(int id, CancellationToken cancellationToken = default);

    Task<List<PayrollLocationDto>> GetPayrollLocationsAsync(CancellationToken cancellationToken = default);
    Task<PayrollLocationDto>       CreatePayrollLocationAsync(PayrollLocationDto dto, CancellationToken cancellationToken = default);
    Task                           UpdatePayrollLocationAsync(PayrollLocationDto dto, CancellationToken cancellationToken = default);
    Task                           DeletePayrollLocationAsync(int id, CancellationToken cancellationToken = default);

    Task<List<UserGroupDto>>       GetUserGroupsAsync(CancellationToken cancellationToken = default);
    Task<UserGroupDto>             CreateUserGroupAsync(UserGroupDto dto, CancellationToken cancellationToken = default);
    Task                           UpdateUserGroupAsync(UserGroupDto dto, CancellationToken cancellationToken = default);
    Task                           DeleteUserGroupAsync(int id, CancellationToken cancellationToken = default);
    Task<UserGroupDto>             GetUserGroupByIdAsync(int id, CancellationToken cancellationToken = default);
}
