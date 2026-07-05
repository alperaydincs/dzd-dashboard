using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class StoredFileConfiguration : IEntityTypeConfiguration<StoredFile>
{
    public void Configure(EntityTypeBuilder<StoredFile> builder)
    {
        builder.ToTable("Files");
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Content).HasColumnType("varbinary(max)").IsRequired();
        builder.Property(f => f.ContentType).HasMaxLength(200);
    }
}
