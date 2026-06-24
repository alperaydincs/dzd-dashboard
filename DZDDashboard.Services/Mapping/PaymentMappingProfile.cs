using AutoMapper;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Utils;
using DZDDashboard.Data.Entities;

namespace DZDDashboard.Services.Mapping;

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
            .ForMember(d => d.ModifiedAt, o => o.Ignore())
            .ForMember(d => d.NotesModifiedAt, o => o.Ignore());

        CreateMap<BenefitDependent, BenefitDependentDto>()
            .ForMember(d => d.DependentType, o => o.MapFrom(s => s.DependentTypeRef != null ? s.DependentTypeRef.Name : null))
            .ReverseMap()
            .ForMember(d => d.BenefitRecord, o => o.Ignore())
            .ForMember(d => d.BenefitRecordId, o => o.Ignore())
            .ForMember(d => d.DependentTypeRef, o => o.Ignore())
            .ForMember(d => d.DependentTypeId, o => o.Ignore())
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
            .ForMember(d => d.Dependents, o => o.Ignore());
        CreateMap<AdditionalPayment, AdditionalPaymentDto>()
            .ForMember(d => d.ModifiedByName, o => o.MapFrom(s => s.ModifiedBy != null ? AppFormatter.BuildFullName(s.ModifiedBy.FirstName, s.ModifiedBy.LastName) : null))
            .ForMember(d => d.PaymentType, o => o.MapFrom(s => s.PaymentTypeRef != null ? s.PaymentTypeRef.Name : null))
            .ReverseMap()
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.PaymentTypeRef, o => o.Ignore())
            .ForMember(d => d.PaymentTypeId, o => o.Ignore())
            .ForMember(d => d.ModifiedBy, o => o.Ignore())
            .ForMember(d => d.ModifiedById, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.ModifiedAt, o => o.Ignore());

        CreateMap<Deduction, DeductionDto>()
            .ForMember(d => d.ModifiedByName, o => o.MapFrom(s => s.ModifiedBy != null ? AppFormatter.BuildFullName(s.ModifiedBy.FirstName, s.ModifiedBy.LastName) : null))
            .ForMember(d => d.DeductionType, o => o.MapFrom(s => s.DeductionTypeRef != null ? s.DeductionTypeRef.Name : null))
            .ReverseMap()
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.DeductionTypeRef, o => o.Ignore())
            .ForMember(d => d.DeductionTypeId, o => o.Ignore())
            .ForMember(d => d.ModifiedBy, o => o.Ignore())
            .ForMember(d => d.ModifiedById, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.ModifiedAt, o => o.Ignore());
    }
}
