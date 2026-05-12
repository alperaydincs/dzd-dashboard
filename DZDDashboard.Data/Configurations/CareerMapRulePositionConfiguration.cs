using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class CareerMapRulePositionConfiguration : IEntityTypeConfiguration<CareerMapRulePosition>
{
    public void Configure(EntityTypeBuilder<CareerMapRulePosition> builder)
    {
        builder.ToTable("CareerMapRulePositions");

        builder.HasKey(x => new { x.CareerMapRuleId, x.JobId });

        builder.HasOne(x => x.CareerMapRule)
               .WithMany(x => x.Positions)
               .HasForeignKey(x => x.CareerMapRuleId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Job)
               .WithMany()
               .HasForeignKey(x => x.JobId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
