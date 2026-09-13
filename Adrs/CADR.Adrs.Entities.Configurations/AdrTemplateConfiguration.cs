using CADR.Context.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CADR.Adrs.Entities.Configurations;

/// <summary>Конфигурация <see cref="AdrTemplate"/></summary>
public class AdrTemplateConfiguration : IEntityTypeConfiguration<AdrTemplate>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AdrTemplate> builder)
    {
        builder.ToTable("AdrTemplates");
        builder.HasIdAsKey();
        builder.PropertyAuditConfiguration();

        builder.HasIndex(x => new { x.OrganizationId, x.Name }, $"IX_{nameof(AdrTemplate)}_{nameof(AdrTemplate.Name)}")
            .IsUnique()
            .HasFilter($@"""{nameof(AdrTemplate.DeletedAt)}"" IS NULL AND ""{nameof(AdrTemplate.OrganizationId)}"" IS NOT NULL");

        builder.HasOne(x => x.Organization)
            .WithMany()
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
