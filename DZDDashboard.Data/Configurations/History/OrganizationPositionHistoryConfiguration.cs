using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class OrganizationPositionHistoryConfiguration : HistoryEntityConfigurationBase<OrganizationPositionHistory>
{
    protected override string TableName => "OrganizationPositionHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<OrganizationPositionHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
