using Ahatornn.TestGenerator;
using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;

namespace CADR.Adrs.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="IAdrOrganizationSettingsReadRepository"/>
/// </summary>
public class AdrOrganizationSettingsReadRepositoryTests : CadrContextInMemory
{
    private readonly IAdrOrganizationSettingsReadRepository adrOrganizationSettingsReadRepository;
    private readonly TestEntityProvider entityProvider;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrOrganizationSettingsReadRepositoryTests"/>
    /// </summary>
    public AdrOrganizationSettingsReadRepositoryTests()
    {
        entityProvider = new TestEntityProviderBuilder()
            .Build();
        adrOrganizationSettingsReadRepository = new AdrOrganizationSettingsReadRepository(Context);
    }

    /// <summary>
    /// Возвращает null если настройки ADR организации не найдены
    /// </summary>
    [Fact]
    public async Task GetByOrganizationIdShouldReturnNull()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();

        // Act
        var result = await adrOrganizationSettingsReadRepository.GetByOrganizationIdAsync(targetOrganizationId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает null если настройки ADR организации удалены
    /// </summary>
    [Fact]
    public async Task GetByOrganizationIdShouldReturnNullIfDeleted()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();
        var targetSettings = entityProvider.Create<AdrOrganizationSettings>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        await Context.AddAsync(targetSettings);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrOrganizationSettingsReadRepository.GetByOrganizationIdAsync(targetOrganizationId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает настройки ADR организации
    /// </summary>
    [Fact]
    public async Task GetByOrganizationIdShouldReturnValue()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();
        var targetSettings = entityProvider.Create<AdrOrganizationSettings>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.LikesRequiredForApproval = 3;
        });
        await Context.AddAsync(targetSettings);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrOrganizationSettingsReadRepository.GetByOrganizationIdAsync(targetOrganizationId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(targetSettings, options => options
                .Excluding(o => o.Organization));
    }
}
