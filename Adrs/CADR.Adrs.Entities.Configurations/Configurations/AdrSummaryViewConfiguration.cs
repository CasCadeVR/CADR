using CADR.Adrs.Entities.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CADR.Adrs.Entities.Configurations.Configurations;

/// <summary>Конфигурация <see cref="AdrSummaryView"/></summary>
public class AdrSummaryViewConfiguration : IEntityTypeConfiguration<AdrSummaryView>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AdrSummaryView> builder)
    {
        builder.HasNoKey().ToView("adr_summary");
        builder.Property(x => x.Status).HasConversion<int>();
    }
}
