using AutoMapper;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Data.Entities;

namespace DZDDashboard.Services.Mapping;

public class OrganizationMappingProfile : Profile
{
    public OrganizationMappingProfile()
    {
        CreateMap<Company, CompanyDto>().ReverseMap();

        CreateMap<Job, JobDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
            .ReverseMap()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name));

        CreateMap<Department, DepartmentDto>()
            .ReverseMap()
            .ForMember(dest => dest.Company, opt => opt.Ignore());

        CreateMap<Team, TeamDto>()
            .ReverseMap()
            .ForMember(dest => dest.Department, opt => opt.Ignore());

        CreateMap<Grade, GradeDto>();
        CreateMap<GradeDto, Grade>()
            .ForMember(dest => dest.NextStep, opt => opt.Ignore());
        CreateMap<UserGroup, UserGroupDto>().ReverseMap();

        CreateMap<PayrollLocation, PayrollLocationDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Location));
        CreateMap<PayrollLocationDto, PayrollLocation>()
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Name));

        CreateMap<User, OrgChartUserDto>()
            .ForMember(dest => dest.Job, opt => opt.MapFrom(src => src.Job));

        CreateMap<OrganizationPosition, OrganizationPositionDto>()
            .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.Users.Count))
            .ForMember(dest => dest.User,      opt => opt.MapFrom(src => src.Users.OrderBy(x => x.Id).FirstOrDefault()))
            .ForMember(dest => dest.UserId,    opt => opt.MapFrom(src => src.Users.OrderBy(x => x.Id).Select(x => (int?)x.Id).FirstOrDefault()))
            .ForMember(dest => dest.Children,  opt => opt.Ignore());

        CreateMap<CreateOrganizationPositionDto, OrganizationPosition>();
        CreateMap<UpdateOrganizationPositionDto, OrganizationPosition>()
            .ForMember(dest => dest.Users, opt => opt.Ignore())
            .ForMember(dest => dest.Parent, opt => opt.Ignore())
            .ForMember(dest => dest.Children, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore());
    }
}
