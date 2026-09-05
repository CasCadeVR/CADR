using CADR.Context.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CADR.Administrations.Entities.Configurations;

/// <summary>Конфигурация <see cref="UserInvite"/></summary>
public class UserInviteConfiguration : IEntityTypeConfiguration<UserInvite>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<UserInvite> builder)
    {
        builder.ToTable("UserInvites");
        builder.HasIdAsKey();
        builder.PropertyAuditConfiguration();
    }
}
