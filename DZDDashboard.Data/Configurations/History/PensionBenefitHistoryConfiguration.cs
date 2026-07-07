using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class PensionBenefitHistoryConfiguration : HistoryEntityConfigurationBase<PensionBenefitHistory>
{
    protected override string TableName => "PensionBenefitHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<PensionBenefitHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
