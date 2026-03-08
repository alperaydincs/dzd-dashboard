namespace DZDDashboard.Services.Generic;

public interface IGenericService<TEntity, TDto> where TEntity : class where TDto : class
{
    Task<List<TDto>> GetAllAsync();
    Task<TDto?> GetByIdAsync(int id);
    Task<TDto> CreateAsync(TDto dto);
    Task UpdateAsync(TDto dto);
    Task DeleteAsync(int id);
}
