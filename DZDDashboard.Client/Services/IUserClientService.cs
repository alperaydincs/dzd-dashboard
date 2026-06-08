using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Client.Services;

public interface IUserClientService
{
    Task<UserProfileDto?> GetMyProfileAsync();
    Task<PagedResult<UserSummaryDto>?> GetAllUsersAsync(int page = 1, int pageSize = 50);
    Task<EmployeeCardDto?> GetEmployeeCardAsync(int userId);
    /// <summary>
    /// Fetches PII-sensitive fields (citizenship, personal contact, family, address) for an employee.
    /// Requires Admin or HR role (SensitiveDataPolicy). Used to populate PII edit sections in EmployeeCard
    /// because the card endpoint intentionally does not return PII fields.
    /// </summary>
    Task<EmployeeSensitiveInfoDto?> GetSensitiveInfoAsync(int userId);
    Task<UserAvatarDto?> GetUserAvatarAsync(int userId);
    Task<UserAvatarDto?> GetMyAvatarAsync();
    Task<HttpResponseMessage> UpdateMyProfileAvatarAsync(MultipartFormDataContent content);
    Task<HttpResponseMessage> UpdateMyContactInfoAsync(UpdateContactInfoDto dto);
    Task<HttpResponseMessage> UpdateOrganizationPositionAsync(int userId, UpdateUserOrganizationPositionDto dto);
    Task<HttpResponseMessage> UpdateEmergencyContactsAsync(int userId, UpdateEmergencyContactsDto dto);
    Task<HttpResponseMessage> UpdateFamilyInfoAsync(int userId, UpdateFamilyInfoDto dto);
    Task<HttpResponseMessage> UpdateCareerAssignmentAsync(int userId, UpdateCareerAssignmentDto dto);
    Task<HttpResponseMessage> UpdateBasicInfoAsync(int userId, UpdateBasicInfoDto dto);
    Task<HttpResponseMessage> UpdateContactsAsync(int userId, UpdateContactsDto dto);
    Task<HttpResponseMessage> UpdateCitizenshipInfoAsync(int userId, UpdateCitizenshipInfoDto dto);
    Task<HttpResponseMessage> UpdateAddressInfoAsync(int userId, UpdateAddressInfoDto dto);
    Task<HttpResponseMessage> UpdateEducationInfoAsync(int userId, UpdateEducationInfoDto dto);
}
