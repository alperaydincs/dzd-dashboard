using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class DocumentTemplateConfiguration : IEntityTypeConfiguration<DocumentTemplate>
{
    private static readonly DateTime SeedTimestamp = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public void Configure(EntityTypeBuilder<DocumentTemplate> builder)
    {
        builder.ToTable("DocumentTemplates");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(ValidationConstants.MaxStandardLength);

        builder.HasIndex(x => x.ProcessTemplateId);

        builder.HasOne(x => x.ProcessTemplate)
            .WithMany(t => t.Documents)
            .HasForeignKey(x => x.ProcessTemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ModifiedBy)
            .WithMany()
            .HasForeignKey(x => x.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(BuildSeed());
    }

    private static IEnumerable<DocumentTemplate> BuildSeed()
    {
        var id = 1;
        var rows = new List<DocumentTemplate>();

        void Add(int processTemplateId, IReadOnlyList<(string Name, bool Required, int DeadlineDays)> docs)
        {
            var sequence = 1;
            foreach (var doc in docs)
                rows.Add(new DocumentTemplate
                {
                    Id                = id++,
                    ProcessTemplateId = processTemplateId,
                    Name              = doc.Name,
                    Sequence          = sequence++,
                    IsRequired        = doc.Required,
                    DeadlineDays      = doc.DeadlineDays,
                    CreatedAt         = SeedTimestamp
                });
        }

        Add(ProcessTemplateConfiguration.GeneralOnboardingId,
        [
            ("İkametgâh", true, 4),
            ("Diploma", true, 4),
            ("Nüfus Kayıt Örneği", true, 6),
            ("TC Kimlik Kartı Fotokopisi", true, 9),
            ("Adli Sicil Kaydı", false, 9),
            ("Akciğer grafisi, hemogram ve göz raporu", false, 9),
            ("Akbank Maaş Hesabı Bilgisi", true, 7)
        ]);
        Add(ProcessTemplateConfiguration.ResignationId,
        [
            ("İstifa Dilekçesi", true, 1),
            ("Zimmet İade Tutanağı", true, 7)
        ]);
        Add(ProcessTemplateConfiguration.TerminationId,
        [
            ("Fesih Bildirimi", true, 1),
            ("Zimmet İade Tutanağı", true, 7)
        ]);

        return rows;
    }
}
