using AutoMapper;
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
    Task<List<PayrollLocationDto>> GetPayrollLocationsAsync();
    Task<List<UserGroupDto>> GetUserGroupsAsync();
    Task<UserGroupDto> CreateUserGroupAsync(UserGroupDto dto);
    Task UpdateUserGroupAsync(UserGroupDto dto);
    Task DeleteUserGroupAsync(int id);
    Task<UserGroupDto> GetUserGroupWithMembersAsync(int id);
    Task<List<OrganizationPositionDto>> GetAllPositionsAsync();
    Task<OrganizationPositionDto> CreatePositionAsync(CreateOrganizationPositionDto dto);
    Task<OrganizationPositionDto> UpdatePositionAsync(UpdateOrganizationPositionDto dto);
    Task DeletePositionAsync(int id);
}

public class OrganizationService : IOrganizationService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public OrganizationService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CompanyDto>> GetCompaniesAsync()
        => _mapper.Map<List<CompanyDto>>(await _context.Companies.ToListAsync());

    public async Task<CompanyDto> CreateCompanyAsync(CompanyDto dto)
    {
        var entity = _mapper.Map<Company>(dto);
        _context.Companies.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<CompanyDto>(entity);
    }

    public async Task UpdateCompanyAsync(CompanyDto dto)
    {
        var entity = await _context.Companies.FindAsync(dto.Id);
        if (entity != null)
        {
            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteCompanyAsync(int id)
    {
        var entity = await _context.Companies.FindAsync(id);
        if (entity == null) return;

        var departments = await _context.Departments.Where(d => d.CompanyId == id).ToListAsync();
        if (departments.Any())
        {
            var departmentIds = departments.Select(d => d.Id).ToList();
            var teams = await _context.Set<Team>()
                .Where(t => t.DepartmentId.HasValue && departmentIds.Contains(t.DepartmentId.Value))
                .ToListAsync();
            
            if (teams.Any())
                _context.Set<Team>().RemoveRange(teams);
            
            _context.Departments.RemoveRange(departments);
        }

        _context.Companies.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<DepartmentDto>> GetDepartmentsAsync()
        => _mapper.Map<List<DepartmentDto>>(await _context.Departments.Include(d => d.Company).ToListAsync());

    public async Task<DepartmentDto> CreateDepartmentAsync(DepartmentDto dto)
    {
        var entity = _mapper.Map<Department>(dto);
        _context.Departments.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<DepartmentDto>(entity);
    }

    public async Task UpdateDepartmentAsync(DepartmentDto dto)
    {
        var entity = await _context.Departments.FindAsync(dto.Id);
        if (entity != null)
        {
            _mapper.Map(dto, entity);
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
        => _mapper.Map<List<TeamDto>>(await _context.Set<Team>().Include(t => t.Department).ToListAsync());

    public async Task<TeamDto> CreateTeamAsync(TeamDto dto)
    {
        var entity = _mapper.Map<Team>(dto);
        _context.Set<Team>().Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<TeamDto>(entity);
    }

    public async Task UpdateTeamAsync(TeamDto dto)
    {
        var entity = await _context.Set<Team>().FindAsync(dto.Id);
        if (entity != null)
        {
            _mapper.Map(dto, entity);
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
        => _mapper.Map<List<WorkTypeDto>>(await _context.WorkTypes.ToListAsync());

    public async Task<WorkTypeDto> CreateWorkTypeAsync(WorkTypeDto dto)
    {
        var entity = _mapper.Map<WorkType>(dto);
        _context.WorkTypes.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<WorkTypeDto>(entity);
    }

    public async Task UpdateWorkTypeAsync(WorkTypeDto dto)
    {
        var entity = await _context.WorkTypes.FindAsync(dto.Id);
        if (entity != null)
        {
            _mapper.Map(dto, entity);
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
        => _mapper.Map<List<JobDto>>(await _context.Jobs.ToListAsync());

    public async Task<JobDto> CreateJobAsync(JobDto dto)
    {
        var entity = _mapper.Map<Job>(dto);
        _context.Jobs.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<JobDto>(entity);
    }

    public async Task UpdateJobAsync(JobDto dto)
    {
        var entity = await _context.Jobs.FindAsync(dto.Id);
        if (entity != null)
        {
            _mapper.Map(dto, entity);
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
        => _mapper.Map<List<GradeDto>>(await _context.Grades.Include(g => g.NextStep).ToListAsync());

    public async Task<GradeDto> CreateGradeAsync(GradeDto dto)
    {
        var entity = _mapper.Map<Grade>(dto);
        _context.Grades.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<GradeDto>(entity);
    }

    public async Task UpdateGradeAsync(GradeDto dto)
    {
        var entity = await _context.Grades.FindAsync(dto.Id);
        if (entity != null)
        {
            _mapper.Map(dto, entity);
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

    public async Task<List<PayrollLocationDto>> GetPayrollLocationsAsync()
        => _mapper.Map<List<PayrollLocationDto>>(await _context.PayrollLocations.ToListAsync());

    public async Task<List<UserGroupDto>> GetUserGroupsAsync()
        => _mapper.Map<List<UserGroupDto>>(await _context.UserGroups.ToListAsync());

    public async Task<UserGroupDto> CreateUserGroupAsync(UserGroupDto dto)
    {
        var entity = _mapper.Map<UserGroup>(dto);
        _context.UserGroups.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<UserGroupDto>(entity);
    }

    public async Task UpdateUserGroupAsync(UserGroupDto dto)
    {
        var entity = await _context.UserGroups.FindAsync(dto.Id);
        if (entity != null)
        {
            _mapper.Map(dto, entity);
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
        var entity = await _context.UserGroups.Include(ug => ug.User).FirstOrDefaultAsync(ug => ug.Id == id);
        return entity == null ? new UserGroupDto() : _mapper.Map<UserGroupDto>(entity);
    }

    public async Task<List<OrganizationPositionDto>> GetAllPositionsAsync()
    {
        var positions = await _context.OrganizationPositions
            .AsNoTracking()
            .Include(x => x.Parent)
            .Include(x => x.Users)
            .ThenInclude(x => x.Avatar)
            .OrderBy(x => x.Name)
            .ToListAsync();

        return _mapper.Map<List<OrganizationPositionDto>>(positions);
    }

    public async Task<OrganizationPositionDto> CreatePositionAsync(CreateOrganizationPositionDto dto)
    {
        if (dto.ParentId.HasValue)
        {
            var parent = await _context.OrganizationPositions.FindAsync(dto.ParentId.Value);
            if (parent == null)
                throw new InvalidOperationException("Parent position not found.");
        }

        var entity = _mapper.Map<OrganizationPosition>(dto);

        _context.OrganizationPositions.Add(entity);

        await _context.SaveChangesAsync();

        return await GetPositionByIdAsync(entity.Id);
    }

    public async Task<OrganizationPositionDto> UpdatePositionAsync(UpdateOrganizationPositionDto dto)
    {
        var entity = await _context.OrganizationPositions.FindAsync(dto.Id);
        if (entity == null) throw new KeyNotFoundException("Position not found");
        
        if (dto.ParentId.HasValue)
        {
            if (dto.ParentId == dto.Id)
                throw new InvalidOperationException("Cannot be parent of itself.");
            
            var parent = await _context.OrganizationPositions.FindAsync(dto.ParentId.Value);
            if (parent == null)
                throw new InvalidOperationException("Parent position not found.");

            if (await IsPositionDescendantAsync(dto.Id, dto.ParentId.Value))
                throw new InvalidOperationException("Cannot set a descendant as parent (circular dependency).");
        }

        _mapper.Map(dto, entity);
        await SetPositionUserAsync(dto.Id, dto.UserId);
        await _context.SaveChangesAsync();

        return await GetPositionByIdAsync(dto.Id);
    }

    public async Task DeletePositionAsync(int id)
    {
        var entity = await _context.OrganizationPositions
            .Include(x => x.Children)
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null) throw new KeyNotFoundException("Position not found");

        if (entity.Children.Any())
            throw new InvalidOperationException("Cannot delete position with children.");

        if (entity.Users.Any())
            throw new InvalidOperationException("Cannot delete position with assigned users.");

        _context.OrganizationPositions.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private async Task<bool> IsPositionDescendantAsync(int ancestorId, int potentialDescendantId)
    {
        var current = await _context.OrganizationPositions.FindAsync(potentialDescendantId);
        
        while (current?.ParentId != null)
        {
            if (current.ParentId == ancestorId)
                return true;
            
            current = await _context.OrganizationPositions.FindAsync(current.ParentId.Value);
        }
        
        return false;
    }

    private async Task SetPositionUserAsync(int positionId, int? userId)
    {
        var currentlyAssignedUsers = await _context.Users
            .Where(x => x.OrganizationPositionId == positionId)
            .ToListAsync();

        foreach (var user in currentlyAssignedUsers)
        {
            if (!userId.HasValue || user.Id != userId.Value)
            {
                user.OrganizationPositionId = null;
            }
        }

        if (!userId.HasValue)
        {
            await RecalculateReportsToAsync();
            return;
        }

        var userToAssign = await _context.Users.FindAsync(userId.Value);
        if (userToAssign == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        userToAssign.OrganizationPositionId = positionId;
        await RecalculateReportsToAsync();
    }

    private async Task RecalculateReportsToAsync()
    {
        var positions = await _context.OrganizationPositions
            .AsNoTracking()
            .Select(p => new { p.Id, p.ParentId })
            .ToListAsync();

        var parentByPositionId = positions.ToDictionary(x => x.Id, x => x.ParentId);

        var allUsers = await _context.Users.ToListAsync();

        var positionedUsers = allUsers
            .Where(u => u.OrganizationPositionId.HasValue)
            .ToList();

        var usersByPosition = positionedUsers
            .GroupBy(u => u.OrganizationPositionId!.Value)
            .ToDictionary(g => g.Key, g => g.OrderBy(u => u.Id).ToList());

        foreach (var currentUser in positionedUsers)
        {
            var managerId = FindNearestAncestorManagerId(
                currentUser.OrganizationPositionId!.Value,
                currentUser.Id,
                parentByPositionId,
                usersByPosition);

            currentUser.ReportsToId = managerId;
        }

        var usersOutsideTree = allUsers
            .Where(u => !u.OrganizationPositionId.HasValue && u.ReportsToId != null)
            .ToList();

        foreach (var userOutsideTree in usersOutsideTree)
        {
            userOutsideTree.ReportsToId = null;
        }
    }

    private static int? FindNearestAncestorManagerId(
        int positionId,
        int currentUserId,
        IReadOnlyDictionary<int, int?> parentByPositionId,
        IReadOnlyDictionary<int, List<User>> usersByPosition)
    {
        if (!parentByPositionId.TryGetValue(positionId, out var parentId))
        {
            return null;
        }

        while (parentId.HasValue)
        {
            if (usersByPosition.TryGetValue(parentId.Value, out var managers))
            {
                var manager = managers.FirstOrDefault(u => u.Id != currentUserId);
                if (manager != null)
                {
                    return manager.Id;
                }
            }

            if (!parentByPositionId.TryGetValue(parentId.Value, out parentId))
            {
                break;
            }
        }

        return null;
    }

    private async Task<OrganizationPositionDto> GetPositionByIdAsync(int id)
    {
        var position = await _context.OrganizationPositions
            .AsNoTracking()
            .Include(x => x.Parent)
            .Include(x => x.Users)
            .ThenInclude(x => x.Avatar)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (position == null)
        {
            throw new KeyNotFoundException("Position not found");
        }

        return _mapper.Map<OrganizationPositionDto>(position);
    }
}
