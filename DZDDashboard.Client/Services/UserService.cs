using DZDDashboard.Common.DTOs.Users;

namespace DZDDashboard.Client.Services;

public class UserService : ApiServiceBase
{
    public UserService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

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
        => await ApiClient.PostAsync("api/users/my-profile/avatar", content);

    public async Task<HttpResponseMessage> UpdateContactInfoAsync(UpdateContactInfoDto dto)
        => await PutAsync("api/users/my-profile/contact-info", dto);

    public async Task<HttpResponseMessage> UpdatePersonalInfoAsync(int userId, PersonalInfoDto dto)
        => await PutAsync($"api/users/{userId}/personal-info", dto);

    public async Task<HttpResponseMessage> UpdateOrganizationPositionAsync(int userId, UpdateUserOrganizationPositionDto dto)
        => await PutAsync($"api/users/{userId}/organization-position", dto);
}
