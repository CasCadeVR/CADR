using CADR.Context.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CADR.Adrs.Entities.Configurations;

/// <summary>Конфигурация <see cref="AdrTemplateSection"/></summary>
public class AdrTemplateSectionConfiguration : IEntityTypeConfiguration<AdrTemplateSection>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AdrTemplateSection> builder)
    {
        builder.ToTable("AdrTemplateSections");
        builder.HasIdAsKey();
        builder.PropertyAuditConfiguration();

        builder.HasIndex(x => new { x.TemplateId, x.Position }, $"IX_{nameof(AdrTemplateSection)}_{nameof(AdrTemplateSection.Position)}")
            .HasFilter($@"""{nameof(AdrTemplateSection.DeletedAt)}"" IS NULL");

        builder.HasOne(x => x.Template)
            .WithMany(x => x.Sections)
            .HasForeignKey(x => x.TemplateId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
