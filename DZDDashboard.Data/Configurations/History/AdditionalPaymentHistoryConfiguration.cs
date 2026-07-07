using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class AdditionalPaymentHistoryConfiguration : HistoryEntityConfigurationBase<AdditionalPaymentHistory>
{
    protected override string TableName => "AdditionalPaymentHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<AdditionalPaymentHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
