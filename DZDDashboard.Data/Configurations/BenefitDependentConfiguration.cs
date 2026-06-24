using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class BenefitDependentConfiguration : IEntityTypeConfiguration<BenefitDependent>
{
    public void Configure(EntityTypeBuilder<BenefitDependent> builder)
    {
        builder.ToTable("UserBenefitDependents");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.DependentName).HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.DependentType).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.StartDate).IsRequired();

        builder.HasOne(x => x.ModifiedBy)
            .WithMany()
            .HasForeignKey(x => x.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
