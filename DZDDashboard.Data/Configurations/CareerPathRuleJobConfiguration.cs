using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class CareerPathRuleJobConfiguration : IEntityTypeConfiguration<CareerPathRuleJob>
{
    public void Configure(EntityTypeBuilder<CareerPathRuleJob> builder)
    {
        builder.ToTable("CareerPathRuleJobs");

        builder.HasKey(x => new { x.CareerPathRuleId, x.JobId });

        builder.HasOne(x => x.CareerPathRule)
               .WithMany(x => x.Positions)
               .HasForeignKey(x => x.CareerPathRuleId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Job)
               .WithMany()
               .HasForeignKey(x => x.JobId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
