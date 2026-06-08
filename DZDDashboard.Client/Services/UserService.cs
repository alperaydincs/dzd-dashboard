using Microsoft.AspNetCore.Components;
using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Client.Services;

public class UserService : ApiServiceBase, IUserClientService
{
    public UserService(
        IHttpClientFactory httpClientFactory,
        NavigationManager navigationManager)
        : base(httpClientFactory, navigationManager) { }

    public async Task<UserProfileDto?> GetMyProfileAsync()
        => await GetAsync<UserProfileDto>(ApiRoutes.Users.MyProfile);

    /// <summary>
    /// Returns a paged list of lightweight user summaries (no avatar base64).
    /// Use <see cref="GetEmployeeCardAsync"/> for the full employee record.
    /// </summary>
    public async Task<PagedResult<UserSummaryDto>?> GetAllUsersAsync(int page = 1, int pageSize = 50)
        => await GetAsync<PagedResult<UserSummaryDto>>(ApiRoutes.Users.All(page, pageSize));

    public async Task<EmployeeCardDto?> GetEmployeeCardAsync(int userId)
        => await GetAsync<EmployeeCardDto>(ApiRoutes.Users.Card(userId));

    public async Task<EmployeeSensitiveInfoDto?> GetSensitiveInfoAsync(int userId)
        => await GetAsync<EmployeeSensitiveInfoDto>(ApiRoutes.Users.SensitiveInfo(userId));

    public async Task<UserAvatarDto?> GetUserAvatarAsync(int userId)
        => await GetAsync<UserAvatarDto>(ApiRoutes.Users.Avatar(userId));

    public async Task<UserAvatarDto?> GetMyAvatarAsync()
        => await GetAsync<UserAvatarDto>(ApiRoutes.Users.MyAvatar);

    public async Task<HttpResponseMessage> UpdateMyProfileAvatarAsync(MultipartFormDataContent content)
        => await PostMultipartAsync(ApiRoutes.Users.MyProfileAvatar, content);

    public async Task<HttpResponseMessage> UpdateMyContactInfoAsync(UpdateContactInfoDto dto)
        => await PutAsync(ApiRoutes.Users.MyContactInfo, dto);

    public async Task<HttpResponseMessage> UpdateOrganizationPositionAsync(int userId, UpdateUserOrganizationPositionDto dto)
        => await PutAsync(ApiRoutes.Users.OrganizationPosition(userId), dto);

    public async Task<HttpResponseMessage> UpdateEmergencyContactsAsync(int userId, UpdateEmergencyContactsDto dto)
        => await PutAsync(ApiRoutes.Users.EmergencyContacts(userId), dto);

    public async Task<HttpResponseMessage> UpdateFamilyInfoAsync(int userId, UpdateFamilyInfoDto dto)
        => await PutAsync(ApiRoutes.Users.FamilyInfo(userId), dto);

    public async Task<HttpResponseMessage> UpdateCareerAssignmentAsync(int userId, UpdateCareerAssignmentDto dto)
        => await PutAsync(ApiRoutes.Users.Career(userId), dto);

    public async Task<HttpResponseMessage> UpdateBasicInfoAsync(int userId, UpdateBasicInfoDto dto)
        => await PutAsync(ApiRoutes.Users.BasicInfo(userId), dto);

    public async Task<HttpResponseMessage> UpdateContactsAsync(int userId, UpdateContactsDto dto)
        => await PutAsync(ApiRoutes.Users.Contacts(userId), dto);

    public async Task<HttpResponseMessage> UpdateCitizenshipInfoAsync(int userId, UpdateCitizenshipInfoDto dto)
        => await PutAsync(ApiRoutes.Users.CitizenshipInfo(userId), dto);

    public async Task<HttpResponseMessage> UpdateAddressInfoAsync(int userId, UpdateAddressInfoDto dto)
        => await PutAsync(ApiRoutes.Users.AddressInfo(userId), dto);

    public async Task<HttpResponseMessage> UpdateEducationInfoAsync(int userId, UpdateEducationInfoDto dto)
        => await PutAsync(ApiRoutes.Users.EducationInfo(userId), dto);
}
