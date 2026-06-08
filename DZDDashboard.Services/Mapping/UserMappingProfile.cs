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
                    : null));

        // UserSummaryDto is projected manually in UserService.GetAllSummariesAsync via Select()
        // to control exactly which columns EF fetches and to inline nested DTO construction.
        // This mapping is intentionally unused by the service — kept only for AutoMapper
        // configuration validation (automapper.AssertConfigurationIsValid checks all maps).
        CreateMap<User, UserSummaryDto>()
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src =>
                src.Avatar == null ? null : new UserAvatarSummaryDto { Id = src.Avatar.Id, ContentType = src.Avatar.ContentType }))
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Team,       opt => opt.Ignore())
            .ForMember(dest => dest.Job,        opt => opt.Ignore());

        CreateMap<UserAvatar, UserAvatarDto>();
        CreateMap<User, UserProfileReportsToDto>();
        CreateMap<User, UserProfileDto>();
        CreateMap<EmergencyContact, EmergencyContactDto>()
            .ReverseMap()
            .ForMember(dest => dest.UserId, opt => opt.Ignore()); // UserId set by service, not from DTO
        CreateMap<EducationHistory, EducationHistoryDto>().ReverseMap();

        CreateMap<User, EmployeeCardDto>()
            .ForMember(dest => dest.OrganizationPositionName,
                opt => opt.MapFrom(src => src.OrganizationPosition != null ? src.OrganizationPosition.Name : null))
            // EF Core-translatable string concat (replaces AppFormatter.BuildFullName which uses LINQ and
            // is not translatable to SQL). Trim() is supported by the SQL Server EF provider.
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => (src.FirstName + " " + src.LastName).Trim()))
            .ForMember(dest => dest.CareerPathName,
                opt => opt.MapFrom(src => src.CareerPath != null ? src.CareerPath.Name : null))
            // Active-only TargetEfforts — EF Core translates this filter in the ProjectTo SELECT
            .ForMember(dest => dest.TargetEfforts,
                opt => opt.MapFrom(src => src.TargetEfforts!.Where(t => t.IsActive)))
            // ── PII fields — explicitly ignored. Fetched via GET /api/users/{id}/sensitive-info
            // (SensitiveDataPolicy) rather than the card endpoint. Fields remain on the DTO
            // so Blazor edit-section components can POST them; they are never auto-populated here.
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
            .ForMember(dest => dest.City,                  opt => opt.Ignore())
            .ForMember(dest => dest.Country,               opt => opt.Ignore())
            .ForMember(dest => dest.Children,              opt => opt.Ignore());
            // BankName and Iban are not in EmployeeCardDto — never exposed through this mapping.
            // TODO: Implement GET /api/users/{id}/bank-info gated by a Finance role policy.

        // UpdateContactInfoDto → User mapping removed: properties are assigned explicitly in UserService
    }
}
