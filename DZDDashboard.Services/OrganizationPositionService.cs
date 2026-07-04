using AutoMapper;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Exceptions;
using DZDDashboard.Data;
using DZDDashboard.Data.Entities;
using DZDDashboard.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services;


public class OrganizationPositionService(
    AppDbContext context,
    IMapper mapper,
    IReportsToCalculator reportsToCalculator) : IOrganizationPositionService
{
    public async Task<List<OrganizationPositionDto>> GetAllPositionsAsync(CancellationToken cancellationToken = default)
    {
        var positions = await PositionsWithDetails()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        var dtos = mapper.Map<List<OrganizationPositionDto>>(positions);
        ComputeLevels(dtos);
        return dtos;
    }

    public async Task<OrganizationPositionDto> CreatePositionAsync(CreateOrganizationPositionDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.ParentId.HasValue)
        {
            var parentExists = await context.OrganizationPositions.AnyAsync(p => p.Id == dto.ParentId.Value, cancellationToken);
            if (!parentExists) throw new EntityNotFoundException("OrganizationPosition (parent)", dto.ParentId.Value);
        }

        var entity = mapper.Map<OrganizationPosition>(dto);
        context.OrganizationPositions.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(entity.Id, cancellationToken);
    }

    public async Task UpdatePositionAsync(UpdateOrganizationPositionDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await context.OrganizationPositions
            .FindRequiredAsync(dto.Id, nameof(OrganizationPosition), cancellationToken);

        if (dto.ParentId.HasValue)
        {
            if (dto.ParentId == dto.Id)
                throw new DomainValidationException("A position cannot be its own parent.");

            var parentExists = await context.OrganizationPositions.AnyAsync(p => p.Id == dto.ParentId.Value, cancellationToken);
            if (!parentExists)
                throw new EntityNotFoundException("OrganizationPosition (parent)", dto.ParentId.Value);

            if (await IsDescendantAsync(dto.Id, dto.ParentId.Value))
                throw new DomainConflictException("Cannot set a descendant as parent — circular dependency detected.");
        }

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        mapper.Map(dto, entity);
        await AssignPositionUserAsync(dto.Id, dto.UserId, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        await reportsToCalculator.RecalculateAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    public async Task DeletePositionAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await context.OrganizationPositions
            .Include(x => x.Children)
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new EntityNotFoundException("OrganizationPosition", id);

        if (entity.Children.Count > 0)
            throw new DomainConflictException("Cannot delete a position that has child positions.");

        if (entity.Users.Count > 0)
            throw new DomainConflictException("Cannot delete a position that has assigned users.");

        context.OrganizationPositions.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }


    private async Task<bool> IsDescendantAsync(int ancestorId, int potentialDescendantId)
    {
        var parentMap = await context.OrganizationPositions
            .AsNoTracking()
            .Select(p => new { p.Id, p.ParentId })
            .ToDictionaryAsync(p => p.Id, p => p.ParentId);

        var currentId = (int?)potentialDescendantId;
        while (currentId.HasValue && parentMap.TryGetValue(currentId.Value, out var parentId))
        {
            if (parentId == ancestorId) return true;
            currentId = parentId;
        }
        return false;
    }

    private async Task AssignPositionUserAsync(int positionId, int? userId, CancellationToken cancellationToken = default)
    {
        var currentOccupant = await context.Users
            .FirstOrDefaultAsync(u => u.OrganizationPositionId == positionId, cancellationToken);

        if (currentOccupant is not null && currentOccupant.Id != userId)
        {
            currentOccupant.OrganizationPositionId = null;
            currentOccupant.ReportsToId            = null;
        }

        if (userId.HasValue)
        {
            var toAssign = await context.Users.FindRequiredAsync(userId.Value, nameof(User), cancellationToken);
            toAssign.OrganizationPositionId = positionId;
        }
    }

    private async Task<OrganizationPositionDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var position = await PositionsWithDetails()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(OrganizationPosition), id);

        return mapper.Map<OrganizationPositionDto>(position);
    }

    private IQueryable<OrganizationPosition> PositionsWithDetails()
        => context.OrganizationPositions
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Users).ThenInclude(x => x.Job)
            .Include(x => x.Users).ThenInclude(x => x.Avatar);
    private static void ComputeLevels(IList<OrganizationPositionDto> dtos)
    {
        var byId   = dtos.ToDictionary(p => p.Id);
        var depths = new Dictionary<int, int>(dtos.Count);

        int GetDepth(OrganizationPositionDto node, int guard = 0)
        {
            if (guard > dtos.Count) return 0;            if (depths.TryGetValue(node.Id, out var cached)) return cached;

            var depth = node.ParentId.HasValue && byId.TryGetValue(node.ParentId.Value, out var parent)
                ? 1 + GetDepth(parent, guard + 1)
                : 0;

            depths[node.Id] = depth;
            return depth;
        }

        foreach (var dto in dtos)
            dto.Level = GetDepth(dto);
    }
}
