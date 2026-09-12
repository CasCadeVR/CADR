using CADR.Context.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CADR.Adrs.Entities.Configurations;

/// <summary>Конфигурация <see cref="Adr"/></summary>
public class AdrConfiguration : IEntityTypeConfiguration<Adr>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Adr> builder)
    {
        builder.ToTable("Adrs");
        builder.HasIdAsKey();
        builder.PropertyAuditConfiguration();

        builder.HasIndex(x => new { x.OrganizationId, x.Number, }, $"IX_{nameof(Adr)}_{nameof(Adr.Number)}")
            .IsUnique()
            .HasFilter($@"""{nameof(Adr.DeletedAt)}"" IS NULL");

        builder.HasIndex(x => new { x.OrganizationId, x.ParentAdrFolderId }, $"IX_{nameof(Adr)}_{nameof(Adr.ParentAdrFolder)}")
            .HasFilter($@"""{nameof(Adr.DeletedAt)}"" IS NULL");

        builder.HasIndex(x => new { x.OrganizationId, x.Status }, $"IX_{nameof(Adr)}_{nameof(Adr.Status)}")
           .HasFilter($@"""{nameof(Adr.DeletedAt)}"" IS NULL");

        builder.HasMany(x => x.Comments)
            .WithOne(x => x.Adr)
            .HasForeignKey(x => x.AdrId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Votes)
            .WithOne(x => x.Adr)
            .HasForeignKey(x => x.AdrId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Sections)
            .WithOne(x => x.Adr)
            .HasForeignKey(x => x.AdrId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.TargetLinks)
            .WithOne(x => x.TargetAdr)
            .HasForeignKey(x => x.TargetAdrId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.TargetLinks)
            .WithOne(x => x.SourceAdr)
            .HasForeignKey(x => x.SourceAdrId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Organization)
            .WithMany()
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.ParentAdrFolder)
            .WithMany()
            .HasForeignKey(x => x.ParentAdrFolderId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Template)
            .WithMany()
            .HasForeignKey(x => x.TemplateId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
