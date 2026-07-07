using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class CareerPathRuleHistoryConfiguration : HistoryEntityConfigurationBase<CareerPathRuleHistory>
{
    protected override string TableName => "CareerPathRuleHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<CareerPathRuleHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
