using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class OtherBenefitHistoryConfiguration : HistoryEntityConfigurationBase<OtherBenefitHistory>
{
    protected override string TableName => "OtherBenefitHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<OtherBenefitHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
