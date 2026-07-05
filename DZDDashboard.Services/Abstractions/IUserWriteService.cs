using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Services;

public interface IUserWriteService
{
    Task UpdateBasicInfoAsync(int userId, UpdateBasicInfoDto dto, CancellationToken cancellationToken = default);
    Task UpdateContactsAsync(int userId, UpdateContactsDto dto, CancellationToken cancellationToken = default);
    Task UpdateCitizenshipInfoAsync(int userId, UpdateCitizenshipInfoDto dto, CancellationToken cancellationToken = default);
    Task UpdateAddressInfoAsync(int userId, UpdateAddressInfoDto dto, CancellationToken cancellationToken = default);
    Task UpdateEducationInfoAsync(int userId, UpdateEducationInfoDto dto, CancellationToken cancellationToken = default);
    Task UpdatePositionHistoryAsync(int userId, UpdatePositionHistoryDto dto, CancellationToken cancellationToken = default);
    Task UpdateMyContactInfoAsync(int userId, UpdateContactInfoDto dto, CancellationToken cancellationToken = default);
    Task UpdateEmergencyContactsAsync(int userId, UpdateEmergencyContactsDto dto, CancellationToken cancellationToken = default);
    Task UpdateFamilyInfoAsync(int userId, UpdateFamilyInfoDto dto, CancellationToken cancellationToken = default);
    Task UpdateAvatarAsync(int userId, string contentType, byte[] content, CancellationToken cancellationToken = default);
    Task RemoveAvatarAsync(int userId, CancellationToken cancellationToken = default);
    Task UpdateAvatarColorAsync(int userId, int? colorIndex, CancellationToken cancellationToken = default);
    Task UpdateOrganizationPositionAsync(int userId, int? organizationPositionId, CancellationToken cancellationToken = default);
    Task UpdateCareerAssignmentAsync(int userId, UpdateCareerAssignmentDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
