using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CADR.Adrs.Entities.Configurations.Seeding;

/// <summary>Конфигурация начальных данных для <see cref="AdrTemplate"/></summary>
public class AdrTemplateSeeder : IEntityTypeConfiguration<AdrTemplate>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AdrTemplate> builder)
        => builder.HasData(
        [
            new AdrTemplate()
            {
                Id = Guid.Parse("fe644bbb-416f-4af0-abc0-5e0144db26c0"),
                Name = "Минимальный",
                CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                UpdatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                CreatedBy = "system",
                UpdatedBy = "system",
            },
            new AdrTemplate()
            {
                Id = Guid.Parse("94b3e49d-e314-4b0e-b620-35e38759feaa"),
                Name = "Базовый",
                CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                UpdatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                CreatedBy = "system",
                UpdatedBy = "system",
            },
        ]);
}
