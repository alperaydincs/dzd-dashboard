using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class EmergencyContactHistoryConfiguration : HistoryEntityConfigurationBase<EmergencyContactHistory>
{
    protected override string TableName => "EmergencyContactHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<EmergencyContactHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
