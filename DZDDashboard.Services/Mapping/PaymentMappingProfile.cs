using DZDDashboard.Common.DTOs;
using DZDDashboard.Data.Entities;
using Mapster;

namespace DZDDashboard.Services.Mapping;

public class PaymentMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // ModifiedAt/ModifiedByName are populated separately from Salary's history table
        // (see PaymentService.PopulateSalaryLastModifiedAsync) - Salary itself no
        // longer carries them (see EntityWithHistory).
        config.NewConfig<Salary, SalaryRecordDto>()
            .Ignore(d => d.ModifiedAt)
            .Ignore(d => d.ModifiedByName);
        config.NewConfig<SalaryRecordDto, Salary>()
            .Ignore("User")
            .Ignore("CreatedAt")
            .Ignore("NotesModifiedAt");

        config.NewConfig<BenefitPaymentDependent, BenefitDependentDto>();
        config.NewConfig<BenefitDependentDto, BenefitPaymentDependent>()
            .Ignore("BenefitPayment")
            .Ignore("BenefitPaymentId")
            .Ignore("CreatedAt");

        // BenefitPayment is a TPH base (see HealthInsuranceBenefit/PensionBenefit/OtherBenefit) -
        // Include<> registers the derived-type fields so mapping a BenefitPayment-typed
        // reference at runtime still picks up e.g. PolicyNumber for an actual PensionBenefit.
        // DTO -> entity isn't mapped via Mapster: BenefitPayment is abstract, entities are
        // constructed explicitly per type in PaymentService (see CreateBenefitEntity).
        config.NewConfig<BenefitPayment, BenefitRecordDto>()
            .Include<HealthInsuranceBenefit, BenefitRecordDto>()
            .Include<PensionBenefit, BenefitRecordDto>()
            .Include<OtherBenefit, BenefitRecordDto>();
        config.NewConfig<HealthInsuranceBenefit, BenefitRecordDto>();
        config.NewConfig<PensionBenefit, BenefitRecordDto>();
        config.NewConfig<OtherBenefit, BenefitRecordDto>();

        config.NewConfig<AdditionalPayment, AdditionalPaymentDto>();
        config.NewConfig<AdditionalPaymentDto, AdditionalPayment>()
            .Ignore("User");

        config.NewConfig<Deduction, DeductionDto>();
        config.NewConfig<DeductionDto, Deduction>()
            .Ignore("User");
    }
}
