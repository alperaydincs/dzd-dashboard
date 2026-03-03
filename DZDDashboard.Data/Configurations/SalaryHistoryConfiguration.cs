using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class SalaryHistoryConfiguration : IEntityTypeConfiguration<SalaryHistory>
{
    public void Configure(EntityTypeBuilder<SalaryHistory> builder)
    {
        builder.ToTable("UserSalaryHistories");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Amount).HasPrecision(18, 2);

        builder.Property(s => s.StartDate).IsRequired();

        builder.HasOne(s => s.User)
               .WithMany(u => u.SalaryHistories) 
               .HasForeignKey(s => s.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}