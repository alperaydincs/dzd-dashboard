using AutoMapper;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Utils;
using DZDDashboard.Data.Entities;

namespace DZDDashboard.Services.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.ReportsToName,
                opt => opt.MapFrom(src => src.ReportsTo != null
                    ? AppFormatter.BuildFullName(src.ReportsTo.FirstName, src.ReportsTo.LastName)
                    : null))
            .ForMember(dest => dest.HasAvatar,       opt => opt.MapFrom(src => src.Avatar != null))
            .ForMember(dest => dest.AvatarUpdatedAt, opt => opt.MapFrom(src =>
                src.Avatar != null ? (DateTime?)(src.Avatar.ModifiedAt ?? src.Avatar.CreatedAt) : null));

        CreateMap<User, UserSummaryDto>()
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src =>
                src.Avatar == null ? null : new UserAvatarSummaryDto
                {
                    Id          = src.Avatar.Id,
                    ContentType = src.Avatar.ContentType,
                    UpdatedAt   = src.Avatar.ModifiedAt ?? src.Avatar.CreatedAt
                }))
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Team,       opt => opt.Ignore())
            .ForMember(dest => dest.Job,        opt => opt.Ignore());

        CreateMap<User, UserProfileReportsToDto>()
            .ForMember(dest => dest.HasAvatar,       opt => opt.MapFrom(src => src.Avatar != null))
            .ForMember(dest => dest.AvatarUpdatedAt, opt => opt.MapFrom(src =>
                src.Avatar != null ? (DateTime?)(src.Avatar.ModifiedAt ?? src.Avatar.CreatedAt) : null));
        CreateMap<User, UserProfileDto>()
            .ForMember(dest => dest.HasAvatar,       opt => opt.MapFrom(src => src.Avatar != null))
            .ForMember(dest => dest.AvatarUpdatedAt, opt => opt.MapFrom(src =>
                src.Avatar != null ? (DateTime?)(src.Avatar.ModifiedAt ?? src.Avatar.CreatedAt) : null));
        CreateMap<EmergencyContact, EmergencyContactDto>()
            .ReverseMap()
            .ForMember(dest => dest.UserId, opt => opt.Ignore());
        CreateMap<EducationHistory, EducationHistoryDto>().ReverseMap();

        CreateMap<PositionHistory, PositionHistoryDto>()
            .ForMember(dest => dest.DepartmentName,
                opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null))
            .ForMember(dest => dest.TeamName,
                opt => opt.MapFrom(src => src.Team != null ? src.Team.Name : null));

        CreateMap<User, EmployeeCardDto>()
            .ForMember(dest => dest.OrganizationPositionName,
                opt => opt.MapFrom(src => src.OrganizationPosition != null ? src.OrganizationPosition.Name : null))
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => (src.FirstName + " " + src.LastName).Trim()))
            .ForMember(dest => dest.CareerPathName,
                opt => opt.MapFrom(src => src.CareerPath != null ? src.CareerPath.Name : null))
            .ForMember(dest => dest.TargetEfforts,
                opt => opt.MapFrom(src => src.TargetEfforts!.Where(t => t.IsActive)))
            .ForMember(dest => dest.DateOfBirth,           opt => opt.Ignore())
            .ForMember(dest => dest.Gender,                opt => opt.Ignore())
            .ForMember(dest => dest.Nationality,           opt => opt.Ignore())
            .ForMember(dest => dest.CitizenshipNumber,     opt => opt.Ignore())
            .ForMember(dest => dest.DisabilityStatus,      opt => opt.Ignore())
            .ForMember(dest => dest.DisabilityDegree,      opt => opt.Ignore())
            .ForMember(dest => dest.MaritalStatus,         opt => opt.Ignore())
            .ForMember(dest => dest.SpouseFullName,        opt => opt.Ignore())
            .ForMember(dest => dest.PersonalEmail,         opt => opt.Ignore())
            .ForMember(dest => dest.PersonalPhoneNumber,   opt => opt.Ignore())
            .ForMember(dest => dest.LegalAddress,          opt => opt.Ignore())
            .ForMember(dest => dest.CurrentAddress,        opt => opt.Ignore())
            .ForMember(dest => dest.CurrentAddressChangedAt, opt => opt.Ignore())
            .ForMember(dest => dest.City,                  opt => opt.Ignore())
            .ForMember(dest => dest.Country,               opt => opt.Ignore())
            .ForMember(dest => dest.Children,              opt => opt.Ignore());

    }
}
