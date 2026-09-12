using CADR.Context.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CADR.Adrs.Entities.Configurations;

/// <summary>Конфигурация <see cref="AdrComment"/></summary>
public class AdrCommentConfiguration : IEntityTypeConfiguration<AdrComment>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AdrComment> builder)
    {
        builder.ToTable("AdrComments");
        builder.HasIdAsKey();
        builder.PropertyAuditConfiguration();

        builder.HasIndex(x => new { x.AdrId, x.CreatedAt }, $"IX_{nameof(AdrComment)}_{nameof(AdrComment.CreatedAt)}")
            .HasFilter($@"""{nameof(AdrComment.DeletedAt)}"" IS NULL");

        builder.HasOne(x => x.Adr)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.AdrId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
