using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Services;

/// <summary>
/// CRUD for the richer reference types that carry more than a single name
/// (Job, PayrollLocation). Simple fixed value lists are
/// embedded in code (see <c>DomainOptionCatalog</c>) instead of the database.
/// </summary>
public interface IReferenceDataService
{
    Task<List<JobDto>>             GetJobsAsync(CancellationToken cancellationToken = default);
    Task<JobDto>                   CreateJobAsync(JobDto dto, CancellationToken cancellationToken = default);
    Task                           UpdateJobAsync(JobDto dto, CancellationToken cancellationToken = default);
    Task                           DeleteJobAsync(int id, CancellationToken cancellationToken = default);

    Task<List<PayrollLocationDto>> GetPayrollLocationsAsync(CancellationToken cancellationToken = default);
    Task<PayrollLocationDto>       CreatePayrollLocationAsync(PayrollLocationDto dto, CancellationToken cancellationToken = default);
    Task                           UpdatePayrollLocationAsync(PayrollLocationDto dto, CancellationToken cancellationToken = default);
    Task                           DeletePayrollLocationAsync(int id, CancellationToken cancellationToken = default);
}
