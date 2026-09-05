using CADR.Context.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CADR.Administrations.Entities.Configurations;

/// <summary>Конфигурация <see cref="User"/></summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasIdAsKey();
        builder.PropertyAuditConfiguration();

        builder.HasIndex(x => x.LoginLowerCase, $"IX_{nameof(User)}_{nameof(User.LoginLowerCase)}")
            .IsUnique()
            .HasFilter($@"""{nameof(User.DeletedAt)}"" IS NULL");

        builder.HasIndex(x => x.EmailLowerCase, $"IX_{nameof(User)}_{nameof(User.EmailLowerCase)}")
            .IsUnique()
            .HasFilter($@"""{nameof(User.EmailConfirmed)}"" is TRUE AND ""{nameof(User.DeletedAt)}"" IS NULL");

        builder.HasMany(x => x.Invites)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Organizations)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
