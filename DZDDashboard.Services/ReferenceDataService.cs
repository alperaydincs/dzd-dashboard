using AutoMapper;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Exceptions;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using DZDDashboard.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;

// Interface is in Abstractions/IReferenceDataService.cs

/// <summary>
/// Manages flat reference / lookup data: work types, jobs, grades,
/// payroll locations, and user groups.
/// <para>
/// Previously inherited <c>GenericCrudService&lt;WorkType, WorkTypeDto&gt;</c>, which gave
/// DRY CRUD only for <c>WorkType</c> while the other four entities duplicated the same
/// four-method pattern (C# single-inheritance limitation). Refactored to a flat class with
/// private generic helpers so all five entity groups share the same implementation.
/// </para>
/// </summary>
public class ReferenceDataService(AppDbContext context, IMapper mapper) : IReferenceDataService
{
    // ── Generic CRUD helpers ──────────────────────────────────────────────────

    private async Task<List<TDto>> GetAllRefAsync<TEntity, TDto>(CancellationToken ct) where TEntity : class
        => mapper.Map<List<TDto>>(await context.Set<TEntity>().AsNoTracking().ToListAsync(ct));

    private async Task<TDto> CreateRefAsync<TEntity, TDto>(TDto dto, CancellationToken ct) where TEntity : class
    {
        var entity = mapper.Map<TEntity>(dto);
        context.Set<TEntity>().Add(entity);
        await context.SaveChangesAsync(ct);
        return mapper.Map<TDto>(entity);
    }

    private async Task UpdateRefAsync<TEntity, TDto>(int id, TDto dto, string entityName, CancellationToken ct)
        where TEntity : class
    {
        var entity = await context.Set<TEntity>().FindRequiredAsync(id, entityName, ct);
        mapper.Map(dto, entity);
        await context.SaveChangesAsync(ct);
    }

    private async Task DeleteRefAsync<TEntity>(int id, string entityName, CancellationToken ct) where TEntity : class
    {
        var entity = await context.Set<TEntity>().FindRequiredAsync(id, entityName, ct);
        context.Set<TEntity>().Remove(entity);
        await context.SaveChangesAsync(ct);
    }

    // ── Work Types ────────────────────────────────────────────────────────────

    public Task<List<WorkTypeDto>> GetWorkTypesAsync(CancellationToken cancellationToken = default)
        => GetAllRefAsync<WorkType, WorkTypeDto>(cancellationToken);

    public Task<WorkTypeDto> CreateWorkTypeAsync(WorkTypeDto dto, CancellationToken cancellationToken = default)
        => CreateRefAsync<WorkType, WorkTypeDto>(dto, cancellationToken);

    public Task UpdateWorkTypeAsync(WorkTypeDto dto, CancellationToken cancellationToken = default)
        => UpdateRefAsync<WorkType, WorkTypeDto>(dto.Id, dto, nameof(WorkType), cancellationToken);

    public Task DeleteWorkTypeAsync(int id, CancellationToken cancellationToken = default)
        => DeleteRefAsync<WorkType>(id, nameof(WorkType), cancellationToken);

    // ── Jobs ──────────────────────────────────────────────────────────────────

    public Task<List<JobDto>> GetJobsAsync(CancellationToken cancellationToken = default)
        => GetAllRefAsync<Job, JobDto>(cancellationToken);

    public Task<JobDto> CreateJobAsync(JobDto dto, CancellationToken cancellationToken = default)
        => CreateRefAsync<Job, JobDto>(dto, cancellationToken);

    public Task UpdateJobAsync(JobDto dto, CancellationToken cancellationToken = default)
        => UpdateRefAsync<Job, JobDto>(dto.Id, dto, nameof(Job), cancellationToken);

    public Task DeleteJobAsync(int id, CancellationToken cancellationToken = default)
        => DeleteRefAsync<Job>(id, nameof(Job), cancellationToken);

    // ── Grades ────────────────────────────────────────────────────────────────

    public Task<List<GradeDto>> GetGradesAsync(CancellationToken cancellationToken = default)
        => GetAllRefAsync<Grade, GradeDto>(cancellationToken);

    public Task<GradeDto> CreateGradeAsync(GradeDto dto, CancellationToken cancellationToken = default)
        => CreateRefAsync<Grade, GradeDto>(dto, cancellationToken);

    public Task UpdateGradeAsync(GradeDto dto, CancellationToken cancellationToken = default)
        => UpdateRefAsync<Grade, GradeDto>(dto.Id, dto, nameof(Grade), cancellationToken);

    public Task DeleteGradeAsync(int id, CancellationToken cancellationToken = default)
        => DeleteRefAsync<Grade>(id, nameof(Grade), cancellationToken);

    // ── Payroll Locations ─────────────────────────────────────────────────────
    // Kept via generic helpers — AutoMapper handles Name↔Location field renaming transparently.

    public Task<List<PayrollLocationDto>> GetPayrollLocationsAsync(CancellationToken cancellationToken = default)
        => GetAllRefAsync<PayrollLocation, PayrollLocationDto>(cancellationToken);

    public Task<PayrollLocationDto> CreatePayrollLocationAsync(PayrollLocationDto dto, CancellationToken cancellationToken = default)
        => CreateRefAsync<PayrollLocation, PayrollLocationDto>(dto, cancellationToken);

    public Task UpdatePayrollLocationAsync(PayrollLocationDto dto, CancellationToken cancellationToken = default)
        => UpdateRefAsync<PayrollLocation, PayrollLocationDto>(dto.Id, dto, nameof(PayrollLocation), cancellationToken);

    public Task DeletePayrollLocationAsync(int id, CancellationToken cancellationToken = default)
        => DeleteRefAsync<PayrollLocation>(id, nameof(PayrollLocation), cancellationToken);

    // ── User Groups ───────────────────────────────────────────────────────────

    public Task<List<UserGroupDto>> GetUserGroupsAsync(CancellationToken cancellationToken = default)
        => GetAllRefAsync<UserGroup, UserGroupDto>(cancellationToken);

    public Task<UserGroupDto> CreateUserGroupAsync(UserGroupDto dto, CancellationToken cancellationToken = default)
        => CreateRefAsync<UserGroup, UserGroupDto>(dto, cancellationToken);

    public Task UpdateUserGroupAsync(UserGroupDto dto, CancellationToken cancellationToken = default)
        => UpdateRefAsync<UserGroup, UserGroupDto>(dto.Id, dto, nameof(UserGroup), cancellationToken);

    public Task DeleteUserGroupAsync(int id, CancellationToken cancellationToken = default)
        => DeleteRefAsync<UserGroup>(id, nameof(UserGroup), cancellationToken);

    public async Task<UserGroupDto> GetUserGroupByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await context.Set<UserGroup>()
            .AsNoTracking()
            .FirstOrDefaultAsync(ug => ug.Id == id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(UserGroup), id);
        return mapper.Map<UserGroupDto>(entity);
    }
}
