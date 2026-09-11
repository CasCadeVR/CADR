using CADR.Context.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CADR.Administrations.Entities.Configurations;

/// <summary>Конфигурация <see cref="Organization"/></summary>
public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("Organizations");
        builder.HasIdAsKey();
        builder.PropertyAuditConfiguration();

        builder.HasIndex(x => x.NameLowerCase, $"IX_{nameof(Organization)}_{nameof(Organization.NameLowerCase)}")
            .IsUnique()
            .HasFilter($@"""{nameof(Organization.DeletedAt)}"" IS NULL");

        builder.HasMany(x => x.Users)
            .WithOne(x => x.Organization)
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
