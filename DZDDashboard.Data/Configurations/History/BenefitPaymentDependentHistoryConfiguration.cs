using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class BenefitPaymentDependentHistoryConfiguration : HistoryEntityConfigurationBase<BenefitPaymentDependentHistory>
{
    protected override string TableName => "BenefitPaymentDependentHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<BenefitPaymentDependentHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
