using AutoMapper;
using DZDDashboard.Data;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Services.Generic;

public abstract class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    protected readonly AppDbContext Context;
    protected readonly IMapper Mapper;

    protected GenericService(AppDbContext context, IMapper mapper)
    {
        Context = context;
        Mapper = mapper;
    }

    public virtual async Task<List<TDto>> GetAllAsync()
    {
        var entities = await GetQueryable().ToListAsync();
        return Mapper.Map<List<TDto>>(entities);
    }

    public virtual async Task<TDto?> GetByIdAsync(int id)
    {
        var entity = await GetQueryable().FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        if (entity is null)
        {
            return default;
        }

        var dto = Mapper.Map<TDto>(entity);
        return dto;
    }

    public virtual async Task<TDto> CreateAsync(TDto dto)
    {
        var entity = Mapper.Map<TEntity>(dto);
        Context.Set<TEntity>().Add(entity);
        await Context.SaveChangesAsync();
        return Mapper.Map<TDto>(entity);
    }

    public virtual async Task UpdateAsync(TDto dto)
    {
        var entity = Mapper.Map<TEntity>(dto);
        Context.Set<TEntity>().Update(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await Context.Set<TEntity>().FindAsync(id);
        if (entity != null)
        {
            Context.Set<TEntity>().Remove(entity);
            await Context.SaveChangesAsync();
        }
    }

    protected virtual IQueryable<TEntity> GetQueryable() => Context.Set<TEntity>();
}
