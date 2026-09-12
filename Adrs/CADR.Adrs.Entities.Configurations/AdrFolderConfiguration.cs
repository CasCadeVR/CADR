using CADR.Context.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CADR.Adrs.Entities.Configurations;

/// <summary>Конфигурация <see cref="AdrFolder"/></summary>
public class AdrFolderConfiguration : IEntityTypeConfiguration<AdrFolder>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AdrFolder> builder)
    {
        builder.ToTable("AdrFolders");
        builder.HasIdAsKey();
        builder.PropertyAuditConfiguration();

        builder.HasIndex(x => new { x.OrganizationId, x.ParentAdrFolderId, x.Name }, $"IX_{nameof(AdrFolder)}_{nameof(AdrFolder.Name)}")
            .HasFilter($@"""{nameof(AdrFolder.DeletedAt)}"" IS NULL");

        builder.HasOne(x => x.ParentAdrFolder)
            .WithMany(x => x.ChildFolder)
            .HasForeignKey(x => x.ParentAdrFolderId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
