namespace DZDDashboard.Services;

public interface IReportsToCalculator
{
    Task RecalculateAsync(CancellationToken cancellationToken = default);
}
