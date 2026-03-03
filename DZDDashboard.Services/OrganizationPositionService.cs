using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        return await _context.OrganizationPositions
            .Include(x => x.Parent)
            .Include(x => x.Users)
            .OrderBy(x => x.Name)
            .ProjectTo<OrganizationPositionDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<OrganizationPositionDto> CreateAsync(CreateOrganizationPositionDto dto)
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
        return _mapper.Map<OrganizationPositionDto>(entity);
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

        _mapper.Map(dto, entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<OrganizationPositionDto>(entity);
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
}
