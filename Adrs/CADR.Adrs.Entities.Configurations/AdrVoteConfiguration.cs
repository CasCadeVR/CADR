using CADR.Context.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CADR.Adrs.Entities.Configurations;

/// <summary>Конфигурация <see cref="AdrVote"/></summary>
public class AdrVoteConfiguration : IEntityTypeConfiguration<AdrVote>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AdrVote> builder)
    {
        builder.ToTable("AdrVotes");
        builder.HasIdAsKey();
        builder.PropertyAuditConfiguration();

        builder.HasIndex(x => new { x.AdrId, x.UserId }, $"IX_{nameof(AdrVote)}_{nameof(AdrVote.User)}")
            .IsUnique()
            .HasFilter($@"""{nameof(AdrVote.DeletedAt)}"" IS NULL");

        builder.HasOne(x => x.Adr)
            .WithMany(x => x.Votes)
            .HasForeignKey(x => x.AdrId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
