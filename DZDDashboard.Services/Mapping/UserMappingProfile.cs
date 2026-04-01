using AutoMapper;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Data.Entities;

namespace DZDDashboard.Services.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(_ => Array.Empty<RoleDto>()))
            .ForMember(dest => dest.ReportsToName,
                opt => opt.MapFrom(src => src.ReportsTo != null ? $"{src.ReportsTo.FirstName} {src.ReportsTo.LastName}" : null));

        CreateMap<UserAvatar, UserAvatarDto>();
        CreateMap<User, UserProfileReportsToDto>();
        CreateMap<User, UserProfileDto>();
        CreateMap<EmergencyContact, EmergencyContactDto>().ReverseMap();
        CreateMap<EducationHistory, EducationHistoryDto>().ReverseMap();

        CreateMap<User, EmployeeCardDto>()
            .ForMember(dest => dest.OrganizationPositionName,
                opt => opt.MapFrom(src => src.OrganizationPosition != null ? src.OrganizationPosition.Name : null))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                string.Join(" ", new[] { src.FirstName, src.LastName }.Where(s => !string.IsNullOrWhiteSpace(s)))))
            .ForMember(dest => dest.Children, opt => opt.MapFrom(src =>
                src.Children ?? new List<ChildInfo>()))
            .ForMember(dest => dest.EducationHistories, opt => opt.MapFrom(src =>
                src.EducationHistories ?? new List<EducationHistory>()));

        CreateMap<UpdateContactInfoDto, User>()
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.WorkPhoneNumber))
            .ForMember(dest => dest.PersonalEmail, opt => opt.MapFrom(src => src.PersonalEmail))
            .ForMember(dest => dest.PersonalPhoneNumber, opt => opt.MapFrom(src => src.PersonalPhoneNumber));
    }
}
