using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class BankConfiguration : IEntityTypeConfiguration<Bank>
{
    public void Configure(EntityTypeBuilder<Bank> builder)
    {
        builder.ToTable("Banks");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.BankName)
               .IsRequired()
               .HasMaxLength(150);

        builder.HasIndex(b => b.BankName).IsUnique();

        builder.HasMany(b => b.Projects)
               .WithOne(p => p.Bank)
               .HasForeignKey(p => p.BankId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(b => b.Itsms)
               .WithOne(i => i.Bank)
               .HasForeignKey(i => i.BankId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.ModifiedBy)
               .WithMany()
               .HasForeignKey(b => b.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}