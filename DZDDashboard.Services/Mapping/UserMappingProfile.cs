using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Utils;
using DZDDashboard.Data.Entities;
using Mapster;

namespace DZDDashboard.Services.Mapping;

public class UserMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserSummaryDto>()
            .Map(dest => dest.Avatar, src =>
                src.Avatar == null ? null : new UserAvatarSummaryDto
                {
                    Id = src.Avatar.Id,
                    ContentType = src.Avatar.ContentType,
                    UpdatedAt = src.Avatar.ModifiedAt ?? src.Avatar.CreatedAt
                })
            .Ignore("Department")
            .Ignore("Job");

        config.NewConfig<User, UserProfileReportsToDto>()
            .Map(dest => dest.HasAvatar, src => src.Avatar != null)
            .Map(dest => dest.AvatarUpdatedAt, src =>
                src.Avatar != null ? (DateTime?)(src.Avatar.ModifiedAt ?? src.Avatar.CreatedAt) : null);

        config.NewConfig<User, UserProfileDto>()
            .Map(dest => dest.HasAvatar, src => src.Avatar != null)
            .Map(dest => dest.AvatarUpdatedAt, src =>
                src.Avatar != null ? (DateTime?)(src.Avatar.ModifiedAt ?? src.Avatar.CreatedAt) : null);

        config.NewConfig<EmergencyContact, EmergencyContactDto>();
        config.NewConfig<EmergencyContactDto, EmergencyContact>()
            .Ignore("UserId");

        config.NewConfig<EducationHistory, EducationHistoryDto>();
        config.NewConfig<EducationHistoryDto, EducationHistory>();

        config.NewConfig<PositionHistory, PositionHistoryDto>();

        config.NewConfig<User, EmployeeDto>()
            .Map(dest => dest.FullName, src => (src.FirstName + " " + src.LastName).Trim())
            .Ignore("DateOfBirth")
            .Ignore("Gender")
            .Ignore("Nationality")
            .Ignore("CitizenshipNumber")
            .Ignore("DisabilityStatus")
            .Ignore("DisabilityDegree")
            .Ignore("MaritalStatus")
            .Ignore("SpouseFullName")
            .Ignore("PersonalEmail")
            .Ignore("PersonalPhoneNumber")
            .Ignore("LegalAddress")
            .Ignore("CurrentAddress")
            .Ignore("CurrentAddressChangedAt")
            .Ignore("City")
            .Ignore("Country")
            .Ignore("Children");
    }
}
