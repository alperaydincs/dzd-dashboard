using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Services;

/// <summary>Mutating user operations — profile updates, avatar, deletion.</summary>
public interface IUserWriteService
{
    Task UpdateBasicInfoAsync(int userId, UpdateBasicInfoDto dto, CancellationToken cancellationToken = default);
    Task UpdateContactsAsync(int userId, UpdateContactsDto dto, CancellationToken cancellationToken = default);
    Task UpdateCitizenshipInfoAsync(int userId, UpdateCitizenshipInfoDto dto, CancellationToken cancellationToken = default);
    Task UpdateAddressInfoAsync(int userId, UpdateAddressInfoDto dto, CancellationToken cancellationToken = default);
    Task UpdateEducationInfoAsync(int userId, UpdateEducationInfoDto dto, CancellationToken cancellationToken = default);
    Task UpdateMyContactInfoAsync(int userId, UpdateContactInfoDto dto, CancellationToken cancellationToken = default);
    Task UpdateEmergencyContactsAsync(int userId, UpdateEmergencyContactsDto dto, CancellationToken cancellationToken = default);
    Task UpdateFamilyInfoAsync(int userId, UpdateFamilyInfoDto dto, CancellationToken cancellationToken = default);
    Task UpdateAvatarAsync(int userId, string contentType, string base64Content, CancellationToken cancellationToken = default);
    Task UpdateOrganizationPositionAsync(int userId, int? organizationPositionId, CancellationToken cancellationToken = default);
    Task UpdateCareerAssignmentAsync(int userId, UpdateCareerAssignmentDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
