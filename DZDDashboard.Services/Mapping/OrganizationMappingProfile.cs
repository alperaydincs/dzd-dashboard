using DZDDashboard.Common.DTOs;
using DZDDashboard.Data.Entities;
using Mapster;

namespace DZDDashboard.Services.Mapping;

public class OrganizationMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Company, CompanyDto>();
        config.NewConfig<CompanyDto, Company>();

        config.NewConfig<Job, JobDto>()
            .Map(dest => dest.Name, src => src.Title);
        config.NewConfig<JobDto, Job>()
            .Map(dest => dest.Title, src => src.Name);

        config.NewConfig<Department, DepartmentDto>();
        config.NewConfig<DepartmentDto, Department>()
            .Ignore("Company");

        config.NewConfig<Team, TeamDto>();
        config.NewConfig<TeamDto, Team>()
            .Ignore("Department");

        config.NewConfig<PayrollLocation, PayrollLocationDto>()
            .Map(dest => dest.Name, src => src.Location);
        config.NewConfig<PayrollLocationDto, PayrollLocation>()
            .Map(dest => dest.Location, src => src.Name);

        config.NewConfig<User, OrgChartUserDto>()
            .Map(dest => dest.Department, src => src.Department)
            .Map(dest => dest.HasAvatar, src => src.AvatarId != null);

        config.NewConfig<OrganizationPosition, OrganizationPositionDto>()
            .Map(dest => dest.User, src => src.Users.OrderBy(x => x.Id).FirstOrDefault());

        config.NewConfig<CreateOrganizationPositionDto, OrganizationPosition>();
        config.NewConfig<UpdateOrganizationPositionDto, OrganizationPosition>()
            .Ignore("Users")
            .Ignore("Parent")
            .Ignore("Children");
    }
}
