using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Client.Services;

public interface IUserClientService
{
    Task<UserProfileDto?> GetMyProfileAsync();
    Task<EmployeeDto?> GetMyCardAsync();
    Task<EmployeeSensitiveInfoDto?> GetMySensitiveInfoAsync();
    Task<HttpResponseMessage> UpdateMyEmergencyContactsAsync(UpdateEmergencyContactsDto dto);
    Task<HttpResponseMessage> UpdateMyFamilyInfoAsync(UpdateFamilyInfoDto dto);
    Task<HttpResponseMessage> UpdateMyAddressInfoAsync(UpdateAddressInfoDto dto);
    Task<HttpResponseMessage> UpdateMyEducationInfoAsync(UpdateEducationInfoDto dto);
    Task<PagedResult<UserSummaryDto>?> GetAllUsersAsync(int page = 1, int pageSize = 50);
    Task<EmployeeDto?> GetEmployeeCardAsync(int userId);
    Task<EmployeeDto?> GetEmployeeCardBySlugAsync(string slug);
    Task<List<UserSearchResultDto>> SearchUsersAsync(string? query);
    Task<EmployeeSensitiveInfoDto?> GetSensitiveInfoAsync(int userId);
    Task<UserAvatarDto?> GetUserAvatarAsync(int userId);
    Task<UserAvatarDto?> GetMyAvatarAsync();
    Task<HttpResponseMessage> UpdateMyProfileAvatarAsync(MultipartFormDataContent content);
    Task<HttpResponseMessage> RemoveMyAvatarAsync();
    Task<HttpResponseMessage> UpdateMyAvatarColorAsync(int? colorIndex);
    Task<HttpResponseMessage> UpdateMyContactInfoAsync(UpdateContactInfoDto dto);
    Task<HttpResponseMessage> UpdateOrganizationPositionAsync(int userId, AssignUserOrganizationPositionDto dto);
    Task<HttpResponseMessage> UpdateEmergencyContactsAsync(int userId, UpdateEmergencyContactsDto dto);
    Task<HttpResponseMessage> UpdateFamilyInfoAsync(int userId, UpdateFamilyInfoDto dto);
    Task<HttpResponseMessage> UpdateCareerAssignmentAsync(int userId, UpdateCareerAssignmentDto dto);
    Task<HttpResponseMessage> UpdateBasicInfoAsync(int userId, UpdateBasicInfoDto dto);
    Task<HttpResponseMessage> UpdateContactsAsync(int userId, UpdateContactsDto dto);
    Task<HttpResponseMessage> UpdateCitizenshipInfoAsync(int userId, UpdateCitizenshipInfoDto dto);
    Task<HttpResponseMessage> UpdateAddressInfoAsync(int userId, UpdateAddressInfoDto dto);
    Task<HttpResponseMessage> UpdateEducationInfoAsync(int userId, UpdateEducationInfoDto dto);
    Task<HttpResponseMessage> UpdateCurrentPositionAsync(int userId, UpdatePositionHistoryDto dto);

    Task<List<UserDocumentDto>> GetUserDocumentsAsync(int userId);
    Task<List<UserDocumentDto>> GetMyDocumentsAsync();
    Task<byte[]?> DownloadMyDocumentAsync(int documentId);
    Task<HttpResponseMessage> UploadUserDocumentAsync(int userId, MultipartFormDataContent content);
    Task<byte[]?> DownloadUserDocumentAsync(int userId, int documentId);
    Task<HttpResponseMessage> DeleteUserDocumentAsync(int userId, int documentId);
    Task<HttpResponseMessage> ReviewUserDocumentAsync(int userId, int documentId, ReviewDocumentDto dto);
}
