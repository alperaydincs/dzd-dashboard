using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Services;

public interface IOrganizationPositionService
{
    Task<List<OrganizationPositionDto>> GetAllPositionsAsync(CancellationToken cancellationToken = default);
    Task<OrganizationPositionDto>       CreatePositionAsync(CreateOrganizationPositionDto dto, CancellationToken cancellationToken = default);
    Task                                UpdatePositionAsync(UpdateOrganizationPositionDto dto, CancellationToken cancellationToken = default);
    Task                                DeletePositionAsync(int id, CancellationToken cancellationToken = default);
}
