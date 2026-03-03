using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class ProjetDocumentConfiguration : IEntityTypeConfiguration<ProjectDocument>
{
    public void Configure(EntityTypeBuilder<ProjectDocument> builder)
    {
        builder.ToTable("ProjectDocuments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.DocumentName)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(d => d.ContentType)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasOne(d => d.Project)
               .WithMany()
               .HasForeignKey(d => d.ProjectId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.ModifiedBy)
               .WithMany()
               .HasForeignKey(d => d.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}