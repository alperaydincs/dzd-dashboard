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
        CreateMap<UserTraining, UserTrainingDto>();
        CreateMap<CareerMapRule, CareerMapRuleDto>()
            .ForMember(dest => dest.PositionJobIds, opt => opt.MapFrom(src => src.Positions.Select(p => p.JobId).ToList()))
            .ForMember(dest => dest.PositionJobs, opt => opt.MapFrom(src => src.Positions.Select(p => p.Job).Where(j => j != null).ToList()));

        CreateMap<CareerPath, CareerPathDto>()
            .ForMember(dest => dest.UserGroupName, opt => opt.MapFrom(src => src.UserGroup != null ? src.UserGroup.GroupName : null))
            .ForMember(dest => dest.Rules, opt => opt.MapFrom(src => src.Rules.OrderBy(r => r.Grade)));
    }
}
