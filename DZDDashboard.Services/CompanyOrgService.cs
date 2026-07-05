using MapsterMapper;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Exceptions;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using DZDDashboard.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;


public class CompanyOrgService(AppDbContext context, IMapper mapper) : ICompanyOrgService
{

    public async Task<List<CompanyDto>> GetCompaniesAsync(CancellationToken cancellationToken = default)
        => mapper.Map<List<CompanyDto>>(await context.Companies.AsNoTracking().ToListAsync(cancellationToken));

    public async Task<CompanyDto> CreateCompanyAsync(CompanyDto dto, CancellationToken cancellationToken = default)
    {
        var entity = mapper.Map<Company>(dto);
        context.Companies.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<CompanyDto>(entity);
    }

    public async Task UpdateCompanyAsync(CompanyDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await context.Companies.FindRequiredAsync(dto.Id, nameof(Company), cancellationToken);
        mapper.Map(dto, entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCompanyAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await context.Companies
            .Include(c => c.Departments)
                .ThenInclude(d => d.Teams)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Company), id);
        context.Companies.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }


    public async Task<List<DepartmentDto>> GetDepartmentsAsync(CancellationToken cancellationToken = default)
        => mapper.Map<List<DepartmentDto>>(
            await context.Departments.AsNoTracking().ToListAsync(cancellationToken));

    public async Task<DepartmentDto> CreateDepartmentAsync(DepartmentDto dto, CancellationToken cancellationToken = default)
    {
        var entity = mapper.Map<Department>(dto);
        context.Departments.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<DepartmentDto>(entity);
    }

    public async Task UpdateDepartmentAsync(DepartmentDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await context.Departments.FindRequiredAsync(dto.Id, nameof(Department), cancellationToken);
        mapper.Map(dto, entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteDepartmentAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await context.Departments
            .Include(d => d.Teams)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Department), id);
        context.Departments.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }


    public async Task<List<TeamDto>> GetTeamsAsync(CancellationToken cancellationToken = default)
        => mapper.Map<List<TeamDto>>(
            await context.Teams.AsNoTracking().ToListAsync(cancellationToken));

    public async Task<TeamDto> CreateTeamAsync(TeamDto dto, CancellationToken cancellationToken = default)
    {
        var entity = mapper.Map<Team>(dto);
        context.Teams.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<TeamDto>(entity);
    }

    public async Task UpdateTeamAsync(TeamDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await context.Teams.FindRequiredAsync(dto.Id, nameof(Team), cancellationToken);
        mapper.Map(dto, entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteTeamAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await context.Teams.FindRequiredAsync(id, nameof(Team), cancellationToken);
        context.Teams.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }
}
