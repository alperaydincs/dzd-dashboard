using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class TeamHistoryConfiguration : HistoryEntityConfigurationBase<TeamHistory>
{
    protected override string TableName => "TeamHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<TeamHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
