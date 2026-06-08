using DZDDashboard.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Data.Extensions;

public static class DbSetExtensions
{
    /// <summary>
    /// Finds an entity by primary key and throws <see cref="EntityNotFoundException"/>
    /// if it does not exist — eliminates the repetitive <c>?? throw</c> pattern.
    /// </summary>
    public static async Task<T> FindRequiredAsync<T>(
        this DbSet<T> dbSet,
        int id,
        string? entityName = null,
        CancellationToken cancellationToken = default)
        where T : class
    {
        var entity = await dbSet.FindAsync([id], cancellationToken);
        if (entity is null)
            throw new EntityNotFoundException(entityName ?? typeof(T).Name, id);
        return entity;
    }
}
