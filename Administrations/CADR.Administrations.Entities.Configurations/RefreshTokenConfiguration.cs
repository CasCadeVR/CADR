using CADR.Context.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CADR.Administrations.Entities.Configurations;

/// <summary>
/// Конфигурация <see cref="RefreshToken"/>
/// </summary>
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");
        builder.HasIdAsKey();
        builder.Property(x => x.SecurityStamp).IsRequired();
        builder.Property(x => x.AccessPayload).IsRequired();

        builder.HasIndex(x => x.UserId, $"IX_{nameof(RefreshToken)}_{nameof(RefreshToken.UserId)}")
            .IsUnique()
            .HasFilter($@"""{nameof(User.DeletedAt)}"" IS NULL");
    }
}
