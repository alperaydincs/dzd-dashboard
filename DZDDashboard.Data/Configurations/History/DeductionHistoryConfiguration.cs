using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class DeductionHistoryConfiguration : HistoryEntityConfigurationBase<DeductionHistory>
{
    protected override string TableName => "DeductionHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<DeductionHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
