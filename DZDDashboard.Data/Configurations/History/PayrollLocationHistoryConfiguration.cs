using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class PayrollLocationHistoryConfiguration : HistoryEntityConfigurationBase<PayrollLocationHistory>
{
    protected override string TableName => "PayrollLocationHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<PayrollLocationHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
