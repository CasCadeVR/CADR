using CADR.Context.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CADR.Adrs.Entities.Configurations;

/// <summary>Конфигурация <see cref="AdrSection"/></summary>
public class AdrSectionConfiguration : IEntityTypeConfiguration<AdrSection>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AdrSection> builder)
    {
        builder.ToTable("AdrSections");
        builder.HasIdAsKey();
        builder.PropertyAuditConfiguration();

        builder.HasIndex(x => new { x.AdrId, x.Position }, $"IX_{nameof(AdrSection)}_{nameof(AdrSection.Position)}")
            .HasFilter($@"""{nameof(AdrSection.DeletedAt)}"" IS NULL");

        builder.HasOne(x => x.Adr)
            .WithMany(x => x.Sections)
            .HasForeignKey(x => x.AdrId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
