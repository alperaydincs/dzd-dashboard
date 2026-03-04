using AutoMapper;
using DZDDashboard.Common.DTOs.Organization;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;

public class OrganizationPositionService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public OrganizationPositionService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<OrganizationPositionDto>> GetAllAsync()
    {
        var positions = await _context.OrganizationPositions
            .AsNoTracking()
            .Include(x => x.Parent)
            .Include(x => x.Users)
            .ThenInclude(x => x.Avatar)
            .OrderBy(x => x.Name)
            .ToListAsync();

        return positions.Select(MapPositionToDto).ToList();
    }

    public async Task<OrganizationPositionDto> CreateAsync(CreateOrganizationPositionDto dto)
    {
        if (dto.ParentId.HasValue)
        {
            var parent = await _context.OrganizationPositions.FindAsync(dto.ParentId.Value);
            if (parent == null)
                throw new InvalidOperationException("Parent position not found.");
        }

        var entity = new OrganizationPosition
        {
            Name = dto.Name,
            ParentId = dto.ParentId
        };

        _context.OrganizationPositions.Add(entity);

        await _context.SaveChangesAsync();

        return await GetByIdAsync(entity.Id);
    }

    public async Task<OrganizationPositionDto> UpdateAsync(UpdateOrganizationPositionDto dto)
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

             if (await IsDescendantAsync(dto.Id, dto.ParentId.Value))
                 throw new InvalidOperationException("Cannot set a descendant as parent (circular dependency).");
        }

        entity.Name = dto.Name;
        entity.ParentId = dto.ParentId;

        await SetPositionUserAsync(dto.Id, dto.UserId);

        await _context.SaveChangesAsync();

        return await GetByIdAsync(dto.Id);
    }

    public async Task DeleteAsync(int id)
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

    private async Task<bool> IsDescendantAsync(int ancestorId, int potentialDescendantId)
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
            return;
        }

        var userToAssign = await _context.Users.FindAsync(userId.Value);
        if (userToAssign == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        userToAssign.OrganizationPositionId = positionId;
    }

    private async Task<OrganizationPositionDto> GetByIdAsync(int id)
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

        return MapPositionToDto(position);
    }

    private OrganizationPositionDto MapPositionToDto(OrganizationPosition position)
    {
        var assignedUser = position.Users.OrderBy(x => x.Id).FirstOrDefault();

        return new OrganizationPositionDto
        {
            Id = position.Id,
            Name = position.Name,
            ParentId = position.ParentId,
            ParentName = position.Parent?.Name,
            UserCount = position.Users.Count,
            UserId = assignedUser?.Id,
            User = assignedUser != null ? _mapper.Map<Common.DTOs.Users.UserDto>(assignedUser) : null
        };
    }
}
