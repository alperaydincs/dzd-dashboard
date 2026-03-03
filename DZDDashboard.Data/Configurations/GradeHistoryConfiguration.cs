using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class GradeHistoryConfiguration : IEntityTypeConfiguration<GradeHistory>
{
    public void Configure(EntityTypeBuilder<GradeHistory> builder)
    {
        builder.ToTable("UserGradeHistories");
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Grade).IsRequired();
        builder.Property(g => g.StartDate).IsRequired();

        builder.HasOne(g => g.User)
               .WithMany(u => u.GradeHistories)
               .HasForeignKey(g => g.UserId)
               .OnDelete(DeleteBehavior.Cascade); 
    }
}