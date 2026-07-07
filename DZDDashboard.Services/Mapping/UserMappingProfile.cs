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
                src.AvatarId == null ? null : new UserAvatarSummaryDto
                {
                    Id = src.AvatarId!.Value,
                    ContentType = src.Avatar!.ContentType
                })
            .Ignore("Department")
            .Ignore("Job");

        config.NewConfig<User, UserProfileReportsToDto>()
            .Map(dest => dest.HasAvatar, src => src.AvatarId != null);

        config.NewConfig<User, UserProfileDto>()
            .Map(dest => dest.HasAvatar, src => src.AvatarId != null);

        config.NewConfig<EmergencyContact, EmergencyContactDto>();
        config.NewConfig<EmergencyContactDto, EmergencyContact>()
            .Ignore("UserId");

        config.NewConfig<Education, EducationHistoryDto>();
        config.NewConfig<EducationHistoryDto, Education>();

        config.NewConfig<Position, PositionHistoryDto>();

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
            .Ignore("Children")
            .Ignore(dest => dest.PositionHistories);
    }
}
