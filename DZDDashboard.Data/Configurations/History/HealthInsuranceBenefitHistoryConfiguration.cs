using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class HealthInsuranceBenefitHistoryConfiguration : HistoryEntityConfigurationBase<HealthInsuranceBenefitHistory>
{
    protected override string TableName => "HealthInsuranceBenefitHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<HealthInsuranceBenefitHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
