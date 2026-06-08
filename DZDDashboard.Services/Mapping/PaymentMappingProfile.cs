using AutoMapper;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Utils;
using DZDDashboard.Data.Entities;

namespace DZDDashboard.Services.Mapping;

/// <summary>
/// Maps Payment-screen entities (Salary/Benefits/Additional Payments) to/from their DTOs.
/// Kept separate from <see cref="UserMappingProfile"/> — single responsibility, mirrors
/// how <c>OrganizationMappingProfile</c>/<c>ReferenceDataMappingProfile</c> are split out.
/// </summary>
public class PaymentMappingProfile : Profile
{
    public PaymentMappingProfile()
    {
        CreateMap<SalaryHistory, SalaryRecordDto>()
            .ForMember(d => d.ModifiedByName, o => o.MapFrom(s => s.ModifiedBy != null ? AppFormatter.BuildFullName(s.ModifiedBy.FirstName, s.ModifiedBy.LastName) : null))
            .ReverseMap()
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.ModifiedBy, o => o.Ignore())
            .ForMember(d => d.ModifiedById, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.ModifiedAt, o => o.Ignore());

        CreateMap<BenefitDependent, BenefitDependentDto>().ReverseMap()
            .ForMember(d => d.BenefitRecord, o => o.Ignore())
            .ForMember(d => d.BenefitRecordId, o => o.Ignore())
            .ForMember(d => d.ModifiedBy, o => o.Ignore())
            .ForMember(d => d.ModifiedById, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.ModifiedAt, o => o.Ignore());

        CreateMap<BenefitRecord, BenefitRecordDto>()
            .ForMember(d => d.ModifiedByName, o => o.MapFrom(s => s.ModifiedBy != null ? AppFormatter.BuildFullName(s.ModifiedBy.FirstName, s.ModifiedBy.LastName) : null))
            .ReverseMap()
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.ModifiedBy, o => o.Ignore())
            .ForMember(d => d.ModifiedById, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.ModifiedAt, o => o.Ignore())
            .ForMember(d => d.Dependents, o => o.Ignore()); // service manages dependents explicitly (ordering/cap rules)

        CreateMap<AdditionalPayment, AdditionalPaymentDto>()
            .ForMember(d => d.ModifiedByName, o => o.MapFrom(s => s.ModifiedBy != null ? AppFormatter.BuildFullName(s.ModifiedBy.FirstName, s.ModifiedBy.LastName) : null))
            .ReverseMap()
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.ModifiedBy, o => o.Ignore())
            .ForMember(d => d.ModifiedById, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.ModifiedAt, o => o.Ignore());
    }
}
