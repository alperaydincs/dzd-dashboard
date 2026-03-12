using AutoMapper;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Data.Entities;

namespace DZDDashboard.Services.Mapping;

public class ReferenceDataMappingProfile : Profile
{
    public ReferenceDataMappingProfile()
    {
        CreateMap<TargetEffort, TargetEffortDto>();
        CreateMap<SalaryHistory, SalaryHistoryDto>();
        CreateMap<GradeHistory, GradeHistoryDto>();
        CreateMap<ChildInfo, ChildInfoDto>().ReverseMap();
        CreateMap<ExCompanyHistory, ExCompanyHistoryDto>();
        CreateMap<UserTraining, UserTrainingDto>();
        CreateMap<UserDocument, UserDocumentDto>();
        CreateMap<UserDocumentCategory, UserDocumentCategoryDto>();
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
    }
}
