using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class PositionHistoryConfiguration : HistoryEntityConfigurationBase<PositionHistory>
{
    protected override string TableName => "PositionHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<PositionHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
