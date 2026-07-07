using DZDDashboard.Data;

namespace DZDDashboard.Tools;

/// <summary>
/// A one-off manual data fix. Implementations go through AppDbContext/SaveChangesAsync so
/// CreatedAt/ModifiedAt/ModifiedBy and the history tables stay accurate - never write raw SQL.
/// Each fix is responsible for calling SaveChangesAsync itself.
/// </summary>
public interface IDataFix
{
    string Name { get; }
    string Description { get; }
    Task RunAsync(AppDbContext db, CancellationToken cancellationToken);
}
