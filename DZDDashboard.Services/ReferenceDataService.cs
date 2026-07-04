using AutoMapper;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Exceptions;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using DZDDashboard.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;


public class ReferenceDataService(AppDbContext context, IMapper mapper) : IReferenceDataService
{

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



    public Task<List<JobDto>> GetJobsAsync(CancellationToken cancellationToken = default)
        => GetAllRefAsync<Job, JobDto>(cancellationToken);

    public Task<JobDto> CreateJobAsync(JobDto dto, CancellationToken cancellationToken = default)
        => CreateRefAsync<Job, JobDto>(dto, cancellationToken);

    public Task UpdateJobAsync(JobDto dto, CancellationToken cancellationToken = default)
        => UpdateRefAsync<Job, JobDto>(dto.Id, dto, nameof(Job), cancellationToken);

    public Task DeleteJobAsync(int id, CancellationToken cancellationToken = default)
        => DeleteRefAsync<Job>(id, nameof(Job), cancellationToken);


    public Task<List<GradeDto>> GetGradesAsync(CancellationToken cancellationToken = default)
        => GetAllRefAsync<Grade, GradeDto>(cancellationToken);

    public Task<GradeDto> CreateGradeAsync(GradeDto dto, CancellationToken cancellationToken = default)
        => CreateRefAsync<Grade, GradeDto>(dto, cancellationToken);

    public Task UpdateGradeAsync(GradeDto dto, CancellationToken cancellationToken = default)
        => UpdateRefAsync<Grade, GradeDto>(dto.Id, dto, nameof(Grade), cancellationToken);

    public Task DeleteGradeAsync(int id, CancellationToken cancellationToken = default)
        => DeleteRefAsync<Grade>(id, nameof(Grade), cancellationToken);


    public Task<List<PayrollLocationDto>> GetPayrollLocationsAsync(CancellationToken cancellationToken = default)
        => GetAllRefAsync<PayrollLocation, PayrollLocationDto>(cancellationToken);

    public Task<PayrollLocationDto> CreatePayrollLocationAsync(PayrollLocationDto dto, CancellationToken cancellationToken = default)
        => CreateRefAsync<PayrollLocation, PayrollLocationDto>(dto, cancellationToken);

    public Task UpdatePayrollLocationAsync(PayrollLocationDto dto, CancellationToken cancellationToken = default)
        => UpdateRefAsync<PayrollLocation, PayrollLocationDto>(dto.Id, dto, nameof(PayrollLocation), cancellationToken);

    public Task DeletePayrollLocationAsync(int id, CancellationToken cancellationToken = default)
        => DeleteRefAsync<PayrollLocation>(id, nameof(PayrollLocation), cancellationToken);
}
