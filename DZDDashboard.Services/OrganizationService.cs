using DZDDashboard.Common.DTOs;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;

public interface IOrganizationService
{
    Task<List<CompanyDto>> GetCompaniesAsync();
    Task<CompanyDto> CreateCompanyAsync(CompanyDto dto);
    Task UpdateCompanyAsync(CompanyDto dto);
    Task DeleteCompanyAsync(int id);
    Task<List<DepartmentDto>> GetDepartmentsAsync();
    Task<DepartmentDto> CreateDepartmentAsync(DepartmentDto dto);
    Task UpdateDepartmentAsync(DepartmentDto dto);
    Task DeleteDepartmentAsync(int id);
    Task<List<TeamDto>> GetTeamsAsync();
    Task<TeamDto> CreateTeamAsync(TeamDto dto);
    Task UpdateTeamAsync(TeamDto dto);
    Task DeleteTeamAsync(int id);
    Task<List<WorkTypeDto>> GetWorkTypesAsync();
    Task<WorkTypeDto> CreateWorkTypeAsync(WorkTypeDto dto);
    Task UpdateWorkTypeAsync(WorkTypeDto dto);
    Task DeleteWorkTypeAsync(int id);
    Task<List<JobDto>> GetJobsAsync();
    Task<JobDto> CreateJobAsync(JobDto dto);
    Task UpdateJobAsync(JobDto dto);
    Task DeleteJobAsync(int id);
    Task<List<GradeDto>> GetGradesAsync();
    Task<GradeDto> CreateGradeAsync(GradeDto dto);
    Task UpdateGradeAsync(GradeDto dto);
    Task DeleteGradeAsync(int id);
    Task<List<UserGroupDto>> GetUserGroupsAsync();
    Task<UserGroupDto> CreateUserGroupAsync(UserGroupDto dto);
    Task UpdateUserGroupAsync(UserGroupDto dto);
    Task DeleteUserGroupAsync(int id);
    Task<UserGroupDto> GetUserGroupWithMembersAsync(int id);
}

public class OrganizationService : IOrganizationService
{
    private readonly AppDbContext _context;

    public OrganizationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CompanyDto>> GetCompaniesAsync()
    {
        return await _context.Companies
            .Select(c => new CompanyDto { Id = c.Id, Name = c.Name, Description = c.Description })
            .ToListAsync();
    }

    public async Task<CompanyDto> CreateCompanyAsync(CompanyDto dto)
    {
        var entity = new Company { Name = dto.Name, Description = dto.Description };
        _context.Companies.Add(entity);
        await _context.SaveChangesAsync();
        dto.Id = entity.Id;
        return dto;
    }

    public async Task UpdateCompanyAsync(CompanyDto dto)
    {
        var entity = await _context.Companies.FindAsync(dto.Id);
        if (entity != null)
        {
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteCompanyAsync(int id)
    {
        var entity = await _context.Companies.FindAsync(id);
        if (entity == null)
        {
            return;
        }

        var departments = await _context.Departments
            .Where(d => d.CompanyId == id)
            .ToListAsync();

        if (departments.Count > 0)
        {
            var departmentIds = departments.Select(d => d.Id).ToList();

            var teams = await _context.Set<Team>()
                .Where(t => t.DepartmentId.HasValue && departmentIds.Contains(t.DepartmentId.Value))
                .ToListAsync();

            if (teams.Count > 0)
            {
                _context.Set<Team>().RemoveRange(teams);
            }

            _context.Departments.RemoveRange(departments);
        }

        _context.Companies.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<DepartmentDto>> GetDepartmentsAsync()
    {
        return await _context.Departments
            .Include(d => d.Company)
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.DepartmentName,
                CompanyId = d.CompanyId,
                CompanyName = d.Company != null ? d.Company.Name : null
            })
            .ToListAsync();
    }

    public async Task<DepartmentDto> CreateDepartmentAsync(DepartmentDto dto)
    {
        var entity = new Department { DepartmentName = dto.Name, CompanyId = dto.CompanyId };
        _context.Departments.Add(entity);
        await _context.SaveChangesAsync();
        dto.Id = entity.Id;
        return dto;
    }

    public async Task UpdateDepartmentAsync(DepartmentDto dto)
    {
        var entity = await _context.Departments.FindAsync(dto.Id);
        if (entity != null)
        {
            entity.DepartmentName = dto.Name;
            entity.CompanyId = dto.CompanyId;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteDepartmentAsync(int id)
    {
        var entity = await _context.Departments.FindAsync(id);
        if (entity != null)
        {
            _context.Departments.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<TeamDto>> GetTeamsAsync()
    {
        return await _context.Set<Team>()
            .Include(t => t.Department)
            .Select(t => new TeamDto
            {
                Id = t.Id,
                Name = t.TeamName,
                DepartmentId = t.DepartmentId,
                DepartmentName = t.Department != null ? t.Department.DepartmentName : null
            })
            .ToListAsync();
    }

    public async Task<TeamDto> CreateTeamAsync(TeamDto dto)
    {
        var entity = new Team { TeamName = dto.Name, DepartmentId = dto.DepartmentId };
        _context.Set<Team>().Add(entity);
        await _context.SaveChangesAsync();
        dto.Id = entity.Id;
        return dto;
    }

    public async Task UpdateTeamAsync(TeamDto dto)
    {
        var entity = await _context.Set<Team>().FindAsync(dto.Id);
        if (entity != null)
        {
            entity.TeamName = dto.Name;
            entity.DepartmentId = dto.DepartmentId;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteTeamAsync(int id)
    {
        var entity = await _context.Set<Team>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<Team>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<WorkTypeDto>> GetWorkTypesAsync()
    {
        return await _context.WorkTypes
            .Select(w => new WorkTypeDto { Id = w.Id, Name = w.Name, Description = w.Description })
            .ToListAsync();
    }

    public async Task<WorkTypeDto> CreateWorkTypeAsync(WorkTypeDto dto)
    {
        var entity = new WorkType { Name = dto.Name, Description = dto.Description };
        _context.WorkTypes.Add(entity);
        await _context.SaveChangesAsync();
        dto.Id = entity.Id;
        return dto;
    }

    public async Task UpdateWorkTypeAsync(WorkTypeDto dto)
    {
        var entity = await _context.WorkTypes.FindAsync(dto.Id);
        if (entity != null)
        {
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteWorkTypeAsync(int id)
    {
        var entity = await _context.WorkTypes.FindAsync(id);
        if (entity != null)
        {
            _context.WorkTypes.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<JobDto>> GetJobsAsync()
    {
        return await _context.Jobs
            .Select(j => new JobDto { Id = j.Id, Name = j.Title, Level = j.Level })
            .ToListAsync();
    }

    public async Task<JobDto> CreateJobAsync(JobDto dto)
    {
        var entity = new Job { Title = dto.Name, Level = dto.Level };
        _context.Jobs.Add(entity);
        await _context.SaveChangesAsync();
        dto.Id = entity.Id;
        return dto;
    }

    public async Task UpdateJobAsync(JobDto dto)
    {
        var entity = await _context.Jobs.FindAsync(dto.Id);
        if (entity != null)
        {
            entity.Title = dto.Name;
            entity.Level = dto.Level;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteJobAsync(int id)
    {
        var entity = await _context.Jobs.FindAsync(id);
        if (entity != null)
        {
            _context.Jobs.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<GradeDto>> GetGradesAsync()
    {
        return await _context.Grades
            .Include(g => g.NextStep)
            .Select(g => new GradeDto
            {
                Id = g.Id,
                Level = g.Level,
                MinSalary = g.MinSalary,
                MaxSalary = g.MaxSalary,
                Currency = g.Currency,
                NextStepId = g.NextStepId,
                NextStepLevel = g.NextStep != null ? g.NextStep.Level : null
            })
            .ToListAsync();
    }

    public async Task<GradeDto> CreateGradeAsync(GradeDto dto)
    {
        var entity = new Grade
        {
            Level = dto.Level,
            MinSalary = dto.MinSalary,
            MaxSalary = dto.MaxSalary,
            Currency = dto.Currency,
            NextStepId = dto.NextStepId
        };
        _context.Grades.Add(entity);
        await _context.SaveChangesAsync();
        dto.Id = entity.Id;
        return dto;
    }

    public async Task UpdateGradeAsync(GradeDto dto)
    {
        var entity = await _context.Grades.FindAsync(dto.Id);
        if (entity != null)
        {
            entity.Level = dto.Level;
            entity.MinSalary = dto.MinSalary;
            entity.MaxSalary = dto.MaxSalary;
            entity.Currency = dto.Currency;
            entity.NextStepId = dto.NextStepId;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteGradeAsync(int id)
    {
        var entity = await _context.Grades.FindAsync(id);
        if (entity != null)
        {
            _context.Grades.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<UserGroupDto>> GetUserGroupsAsync()
    {
        return await _context.UserGroups
            .Select(ug => new UserGroupDto { Id = ug.Id, GroupName = ug.GroupName })
            .ToListAsync();
    }

    public async Task<UserGroupDto> CreateUserGroupAsync(UserGroupDto dto)
    {
        var entity = new UserGroup { GroupName = dto.GroupName };
        _context.UserGroups.Add(entity);
        await _context.SaveChangesAsync();
        dto.Id = entity.Id;
        return dto;
    }

    public async Task UpdateUserGroupAsync(UserGroupDto dto)
    {
        var entity = await _context.UserGroups.FindAsync(dto.Id);
        if (entity != null)
        {
            entity.GroupName = dto.GroupName;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteUserGroupAsync(int id)
    {
        var entity = await _context.UserGroups.FindAsync(id);
        if (entity != null)
        {
            _context.UserGroups.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<UserGroupDto> GetUserGroupWithMembersAsync(int id)
    {
        var entity = await _context.UserGroups
            .Include(ug => ug.User)
            .FirstOrDefaultAsync(ug => ug.Id == id);

        if (entity == null)
            return new UserGroupDto();

        return new UserGroupDto 
        { 
            Id = entity.Id, 
            GroupName = entity.GroupName
        };
    }
}
