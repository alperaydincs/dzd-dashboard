using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class CareerPathHistoryConfiguration : HistoryEntityConfigurationBase<CareerPathHistory>
{
    protected override string TableName => "CareerPathHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<CareerPathHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
