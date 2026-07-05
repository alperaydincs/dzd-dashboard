using DZDDashboard.Common.DTOs;
using DZDDashboard.Data.Entities;
using Mapster;

namespace DZDDashboard.Services.Mapping;

public class ReferenceDataMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ChildInfo, ChildInfoDto>();
        config.NewConfig<ChildInfoDto, ChildInfo>();
        config.NewConfig<RoleDuration, RoleDurationDto>();
        config.NewConfig<RoleDurationDto, RoleDuration>();

        config.NewConfig<CareerPathRule, CareerPathRuleDto>()
            .Map(dest => dest.MinRoleTime, src => src.MinRoleTime)
            .Map(dest => dest.MinExperience, src => src.MinExperience)
            .Map(dest => dest.PositionJobIds, src => src.Positions.Select(p => p.JobId).ToList())
            .Map(dest => dest.PositionJobs, src => src.Positions.Select(p => p.Job).Where(j => j != null).ToList());

        config.NewConfig<CareerPath, CareerPathDto>()
            .Map(dest => dest.Rules, src => src.Rules.OrderBy(r => r.Grade));
    }
}
