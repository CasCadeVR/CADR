using CADR.Context.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CADR.Adrs.Entities.Configurations;

/// <summary>Конфигурация <see cref="AdrOrganizationSettings"/></summary>
public class AdrOrganizationSettingsConfiguration : IEntityTypeConfiguration<AdrOrganizationSettings>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AdrOrganizationSettings> builder)
    {
        builder.ToTable("AdrOrganizationSettings");
        builder.HasIdAsKey();
        builder.PropertyAuditConfiguration();

        builder.HasOne(x => x.Organization)
            .WithMany()
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
