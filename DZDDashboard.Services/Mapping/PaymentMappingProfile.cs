using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Utils;
using DZDDashboard.Data.Entities;
using Mapster;

namespace DZDDashboard.Services.Mapping;

public class PaymentMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<SalaryHistory, SalaryRecordDto>()
            .Map(d => d.ModifiedByName, s => s.ModifiedBy != null ? AppFormatter.BuildFullName(s.ModifiedBy.FirstName, s.ModifiedBy.LastName) : null);
        config.NewConfig<SalaryRecordDto, SalaryHistory>()
            .Ignore("User")
            .Ignore("ModifiedBy")
            .Ignore("ModifiedById")
            .Ignore("CreatedAt")
            .Ignore("ModifiedAt")
            .Ignore("NotesModifiedAt");

        config.NewConfig<BenefitDependent, BenefitDependentDto>();
        config.NewConfig<BenefitDependentDto, BenefitDependent>()
            .Ignore("BenefitRecord")
            .Ignore("BenefitRecordId")
            .Ignore("ModifiedBy")
            .Ignore("ModifiedById")
            .Ignore("CreatedAt")
            .Ignore("ModifiedAt");

        config.NewConfig<BenefitRecord, BenefitRecordDto>();
        config.NewConfig<BenefitRecordDto, BenefitRecord>()
            .Ignore("User")
            .Ignore("ModifiedBy")
            .Ignore("ModifiedById")
            .Ignore("Dependents");

        config.NewConfig<AdditionalPayment, AdditionalPaymentDto>();
        config.NewConfig<AdditionalPaymentDto, AdditionalPayment>()
            .Ignore("User")
            .Ignore("ModifiedBy")
            .Ignore("ModifiedById");

        config.NewConfig<Deduction, DeductionDto>();
        config.NewConfig<DeductionDto, Deduction>()
            .Ignore("User")
            .Ignore("ModifiedBy")
            .Ignore("ModifiedById");
    }
}
