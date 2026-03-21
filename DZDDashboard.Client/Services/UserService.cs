
using Microsoft.AspNetCore.Components;

using DZDDashboard.Common.DTOs;

namespace DZDDashboard.Client.Services;

public class UserService : ApiServiceBase
{
    public UserService(IHttpClientFactory httpClientFactory, NavigationManager navigationManager)
        : base(httpClientFactory, navigationManager) { }

    public async Task<UserProfileDto?> GetMyProfileAsync()
        => await GetAsync<UserProfileDto>("api/users/my-profile");

    public async Task<List<UserDto>> GetAllUsersAsync()
        => await GetAsync<List<UserDto>>("api/users/") ?? new();

    public async Task<EmployeeDetailDto?> GetEmployeeDetailsAsync(int userId)
        => await GetAsync<EmployeeDetailDto>($"api/users/{userId}/details");

    public async Task<UserAvatarDto?> GetUserAvatarAsync(int userId)
        => await GetAsync<UserAvatarDto>($"api/users/{userId}/avatar");

    public async Task<UserAvatarDto?> GetMyAvatarAsync()
        => await GetAsync<UserAvatarDto>("api/users/my-avatar");

    public async Task<PersonalInfoDto?> GetPersonalInfoAsync(int userId)
        => await GetAsync<PersonalInfoDto>($"api/users/{userId}/personal-info");

    public async Task<HttpResponseMessage> UpdateMyProfileAvatarAsync(MultipartFormDataContent content)
        => await PostMultipartAsync("api/users/my-profile/avatar", content);

    public async Task<HttpResponseMessage> UpdateContactInfoAsync(UpdateContactInfoDto dto)
        => await PutAsync("api/users/my-profile/contact-info", dto);

    public async Task<HttpResponseMessage> UpdatePersonalInfoAsync(int userId, PersonalInfoDto dto)
        => await PutAsync($"api/users/{userId}/personal-info", dto);

    public async Task<HttpResponseMessage> UpdateOrganizationPositionAsync(int userId, UpdateUserOrganizationPositionDto dto)
        => await PutAsync($"api/users/{userId}/organization-position", dto);

    public async Task<HttpResponseMessage> UpdateEmergencyContactsAsync(int userId, UpdateEmergencyContactsDto dto)
        => await PutAsync($"api/users/{userId}/emergency-contacts", dto);

    public async Task<HttpResponseMessage> UpdateFamilyInfoAsync(int userId, UpdateFamilyInfoDto dto)
        => await PutAsync($"api/users/{userId}/family-info", dto);

    public async Task<HttpResponseMessage> UpdateBasicInfoAsync(int userId, UpdateBasicInfoDto dto)
    {
        return await PutAsync($"api/users/{userId}/basic-info", dto);
    }

    public async Task<HttpResponseMessage> UpdateContactsAsync(int userId, UpdateContactsDto dto)
    {
        return await PutAsync($"api/users/{userId}/contacts", dto);
    }

    public async Task<HttpResponseMessage> UpdateCitizenshipInfoAsync(int userId, UpdateCitizenshipInfoDto dto)
    {
        return await PutAsync($"api/users/{userId}/citizenship-info", dto);
    }

    public async Task<HttpResponseMessage> UpdateAddressInfoAsync(int userId, UpdateAddressInfoDto dto)
    {
        return await PutAsync($"api/users/{userId}/address-info", dto);
    }
}
