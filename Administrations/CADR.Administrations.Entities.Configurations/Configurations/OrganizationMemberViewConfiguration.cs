using CADR.Administrations.Entities.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CADR.Administrations.Entities.Configurations.Configurations;

/// <summary>Конфигурация <see cref="OrganizationMemberView"/></summary>
public class OrganizationMemberViewConfiguration : IEntityTypeConfiguration<OrganizationMemberView>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<OrganizationMemberView> builder)
    {
        builder.HasNoKey().ToView("organization_members");
        builder.Property(x => x.Role).HasConversion<int>();
    }
}
