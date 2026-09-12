using CADR.Context.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CADR.Adrs.Entities.Configurations;

/// <summary>Конфигурация <see cref="AdrLink"/></summary>
public class AdrLinkConfiguration : IEntityTypeConfiguration<AdrLink>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AdrLink> builder)
    {
        builder.ToTable("AdrLinks");
        builder.HasIdAsKey();
        builder.PropertyAuditConfiguration();

        builder.HasIndex(x => new { x.SourceAdrId, x.TargetAdrId, x.Type }, $"IX_{nameof(AdrLink)}_{nameof(AdrLink.Type)}")
            .IsUnique()
            .HasFilter($@"""{nameof(AdrLink.DeletedAt)}"" IS NULL");
    }
}
