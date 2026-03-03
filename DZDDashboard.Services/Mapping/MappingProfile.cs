using AutoMapper;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.DTOs.Users;
using DZDDashboard.Data.Entities;

namespace DZDDashboard.Services.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Job, JobDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title));

        CreateMap<Department, DepartmentDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DepartmentName));

        CreateMap<Team, TeamDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TeamName));

        CreateMap<PayrollLocation, PayrollLocationDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Location));

        CreateMap<Role, RoleDto>();

        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Roles,
                opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role)))
            .ForMember(dest => dest.ReportsToName, 
                opt => opt.MapFrom(src => src.ReportsTo != null ? $"{src.ReportsTo.FirstName} {src.ReportsTo.LastName}" : null));

        CreateMap<CreateUserDto, User>();

        CreateMap<UserAvatar, UserAvatarDto>();

        CreateMap<User, UserProfileReportsToDto>();

        CreateMap<User, UserProfileDto>();
        CreateMap<User, EmployeeDetailDto>()
            .ForMember(dest => dest.OrganizationPositionName, opt => opt.MapFrom(src => src.OrganizationPosition != null ? src.OrganizationPosition.Name : null));

        CreateMap<TargetEffort, TargetEffortDto>();
        CreateMap<SalaryHistory, SalaryHistoryDto>();
        CreateMap<GradeHistory, GradeHistoryDto>();
        CreateMap<ChildInfo, ChildInfoDto>();
        CreateMap<ExCompanyHistory, ExCompanyHistoryDto>();
        CreateMap<UserTraining, UserTrainingDto>();
        CreateMap<UserGroup, UserGroupDto>();
        CreateMap<UserRole, UserRoleDto>();
        CreateMap<UserDocument, UserDocumentDto>();
        CreateMap<UserDocumentCategory, UserDocumentCategoryDto>();

        CreateMap<OrganizationPosition, DZDDashboard.Common.DTOs.Organization.OrganizationPositionDto>()
            .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.Users.Count))
            .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.Parent != null ? src.Parent.Name : null));
        CreateMap<DZDDashboard.Common.DTOs.Organization.CreateOrganizationPositionDto, OrganizationPosition>();
        CreateMap<DZDDashboard.Common.DTOs.Organization.UpdateOrganizationPositionDto, OrganizationPosition>();

        CreateMap<Bank, BankDto>();
        CreateMap<Period, PeriodDto>();
        CreateMap<Training, TrainingDto>();

        CreateMap<Bid, BidDto>();
        CreateMap<Project, ProjectDto>();
        CreateMap<ProjectBonusCoefficient, ProjectBonusCoefficientDto>();
        CreateMap<ProjectDocument, ProjectDocumentDto>();
        CreateMap<ProjectInvoice, ProjectInvoiceDto>();
        CreateMap<DefaultDocument, DefaultDocumentDto>();

        CreateMap<Itsm, ItsmDto>();
        CreateMap<IssuePaymentType, IssuePaymentTypeDto>();
        CreateMap<IssuePriority, IssuePriorityDto>();
        CreateMap<IssueStatus, IssueStatusDto>();
        CreateMap<IssueType, IssueTypeDto>();
        CreateMap<Resolution, ResolutionDto>();

        CreateMap<DzdStatus, DzdStatusDto>();
        CreateMap<JiraStatus, JiraStatusDto>();
        CreateMap<CareerMapRule, CareerMapRuleDto>();
        CreateMap<HeadLeadCoefficient, HeadLeadCoefficientDto>();
        CreateMap<Salesforce, SalesforceDto>();

        CreateMap<User, PersonalInfoDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                string.Join(" ", new[] { src.FirstName, src.LastName }.Where(s => !string.IsNullOrWhiteSpace(s)))))
            .ForMember(dest => dest.ChildrenCount, opt => opt.MapFrom(src =>
                src.Children != null ? src.Children.Count : 0))
            .ForMember(dest => dest.ChildrenBirthDatesCsv, opt => opt.MapFrom(src =>
                src.Children != null
                    ? string.Join(", ", src.Children.Select(c => c.DateOfBirth.ToString()))
                    : null));

        CreateMap<PersonalInfoDto, User>()
            .ForMember(dest => dest.Username, opt => opt.Ignore()) 
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Children, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedById, opt => opt.Ignore());
    }
}
