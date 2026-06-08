using AutoMapper;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Data.Entities;

namespace DZDDashboard.Services.Mapping;

public class ReferenceDataMappingProfile : Profile
{
    public ReferenceDataMappingProfile()
    {
        CreateMap<TargetEffort, TargetEffortDto>();
        CreateMap<Period, PeriodDto>();
        CreateMap<ChildInfo, ChildInfoDto>().ReverseMap();
        CreateMap<UserTraining, UserTrainingDto>()
            .ForMember(dest => dest.TrainingName, opt => opt.MapFrom(src => src.Training != null ? src.Training.Name : null));
        CreateMap<RoleDuration, RoleDurationDto>().ReverseMap();

        CreateMap<CareerMapRule, CareerMapRuleDto>()
            .ForMember(dest => dest.MinRoleTime,    opt => opt.MapFrom(src => src.MinRoleTime))
            .ForMember(dest => dest.MinExperience,  opt => opt.MapFrom(src => src.MinExperience))
            .ForMember(dest => dest.PositionJobIds, opt => opt.MapFrom(src => src.Positions.Select(p => p.JobId).ToList()))
            .ForMember(dest => dest.PositionJobs,   opt => opt.MapFrom(src => src.Positions.Select(p => p.Job).Where(j => j != null).ToList()));

        CreateMap<CareerPath, CareerPathDto>()
            .ForMember(dest => dest.UserGroupName, opt => opt.MapFrom(src => src.UserGroup != null ? src.UserGroup.GroupName : null))
            .ForMember(dest => dest.Rules, opt => opt.MapFrom(src => src.Rules.OrderBy(r => r.Grade)));
    }
}
