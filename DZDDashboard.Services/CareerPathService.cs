using AutoMapper;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Exceptions;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using DZDDashboard.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;


public class CareerPathService(AppDbContext context, IMapper mapper) : ICareerPathService
{
    public async Task<List<CareerPathDto>> GetCareerPathsAsync(CancellationToken cancellationToken = default)
    {
        var paths = await context.CareerPaths
            .AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.Rules)
                .ThenInclude(r => r.Positions)
                    .ThenInclude(pos => pos.Job)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<CareerPathDto>>(paths);
    }

    public async Task<CareerPathDto> CreateCareerPathAsync(CareerPathDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new CareerPath { Name = dto.Name };
        context.CareerPaths.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<CareerPathDto>(entity);
    }

    public async Task UpdateCareerPathAsync(CareerPathDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await context.CareerPaths.FindRequiredAsync(dto.Id, nameof(CareerPath), cancellationToken);

        entity.Name = dto.Name;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCareerPathAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await context.CareerPaths.FindRequiredAsync(id, nameof(CareerPath), cancellationToken);
        context.CareerPaths.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<CareerMapRuleDto> CreateCareerMapRuleAsync(CareerMapRuleDto dto, CancellationToken cancellationToken = default)
    {
        await ValidateJobIdsExistAsync(dto.PositionJobIds, cancellationToken);

        var entity = ApplyDto(dto, new CareerMapRule());
        entity.Positions = [.. dto.PositionJobIds.Select(jobId => new CareerMapRulePosition { JobId = jobId })];

        context.CareerMapRules.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        await context.Entry(entity).Collection(e => e.Positions).Query()
            .Include(p => p.Job)
            .LoadAsync(cancellationToken);

        return mapper.Map<CareerMapRuleDto>(entity);
    }

    public async Task UpdateCareerMapRuleAsync(CareerMapRuleDto dto, CancellationToken cancellationToken = default)
    {
        await ValidateJobIdsExistAsync(dto.PositionJobIds, cancellationToken);

        var entity = await context.CareerMapRules
            .Include(r => r.Positions)
            .FirstOrDefaultAsync(r => r.Id == dto.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(CareerMapRule), dto.Id);

        ApplyDto(dto, entity);

        var existingJobIds = entity.Positions.Select(p => p.JobId).ToHashSet();
        var incomingJobIds = dto.PositionJobIds.ToHashSet();

        var toRemove = entity.Positions.Where(p => !incomingJobIds.Contains(p.JobId)).ToList();
        context.CareerMapRulePositions.RemoveRange(toRemove);

        foreach (var jobId in incomingJobIds.Except(existingJobIds))
        {
            context.CareerMapRulePositions.Add(new CareerMapRulePosition
            {
                CareerMapRuleId = entity.Id,
                JobId           = jobId
            });
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCareerMapRuleAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await context.CareerMapRules.FindRequiredAsync(id, nameof(CareerMapRule), cancellationToken);
        context.CareerMapRules.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }


    private async Task ValidateJobIdsExistAsync(IEnumerable<int> jobIds, CancellationToken cancellationToken)
    {
        var ids       = jobIds.Distinct().ToList();
        var foundIds  = await context.Jobs
            .Where(j => ids.Contains(j.Id))
            .Select(j => j.Id)
            .ToListAsync(cancellationToken);
        var missingIds = ids.Except(foundIds).ToList();
        if (missingIds.Count > 0)
            throw new EntityNotFoundException($"Job ids not found: {string.Join(", ", missingIds)}");
    }

    private static CareerMapRule ApplyDto(CareerMapRuleDto dto, CareerMapRule entity)
    {
        entity.CareerPathId                 = dto.CareerPathId;
        entity.Grade                        = dto.Grade;
        entity.MinRoleTime.Months           = dto.MinRoleTime.Months;
        entity.MinRoleTime.Years            = dto.MinRoleTime.Years;
        entity.MinExperience.Months         = dto.MinExperience.Months;
        entity.MinExperience.Years          = dto.MinExperience.Years;
        entity.ManagerPerformanceEvaluation = dto.ManagerPerformanceEvaluation;
        entity.AssessmentCenterApplication  = dto.AssessmentCenterApplication;
        entity.TechnicalInterview           = dto.TechnicalInterview;
        entity.CaseStudy                    = dto.CaseStudy;
        entity.EnglishProficiency           = dto.EnglishProficiency;
        entity.ProjectObjective             = dto.ProjectObjective;
        entity.CommitteeApproval            = dto.CommitteeApproval;
        return entity;
    }
}
