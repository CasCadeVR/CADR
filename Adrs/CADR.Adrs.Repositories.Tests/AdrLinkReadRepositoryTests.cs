using Ahatornn.TestGenerator;
using CADR.Adrs.Entities;
using CADR.Adrs.Entities.Enums;
using CADR.Adrs.Repositories.Contracts;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;

namespace CADR.Adrs.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="IAdrLinkReadRepository"/>
/// </summary>
public class AdrLinkReadRepositoryTests : CadrContextInMemory
{
    private readonly IAdrLinkReadRepository adrLinkReadRepository;
    private readonly TestEntityProvider entityProvider;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrLinkReadRepositoryTests"/>
    /// </summary>
    public AdrLinkReadRepositoryTests()
    {
        entityProvider = new TestEntityProviderBuilder()
            .Build();
        adrLinkReadRepository = new AdrLinkReadRepository(Context);
    }

    /// <summary>
    /// Возвращает пустой список связей ADR
    /// </summary>
    [Fact]
    public async Task GetBySourceAdrIdShouldReturnEmpty()
    {
        // Arrange
        var targetAdrId = Guid.NewGuid();

        // Act
        var result = await adrLinkReadRepository.GetBySourceAdrIdAsync(targetAdrId, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Возвращает связи ADR как источника, исключая удалённые и связи других ADR
    /// </summary>
    [Fact]
    public async Task GetBySourceAdrIdShouldReturnValue()
    {
        // Arrange
        var targetAdrId = Guid.NewGuid();
        var otherAdrId = Guid.NewGuid();
        var targetLink1 = entityProvider.Create<AdrLink>(x =>
        {
            x.SourceAdrId = targetAdrId;
            x.TargetAdrId = Guid.NewGuid();
            x.Type = AdrLinkType.RelatedTo;
        });
        var targetLink2 = entityProvider.Create<AdrLink>(x =>
        {
            x.SourceAdrId = targetAdrId;
            x.TargetAdrId = Guid.NewGuid();
            x.Type = AdrLinkType.Supersedes;
        });
        var deletedLink = entityProvider.Create<AdrLink>(x =>
        {
            x.SourceAdrId = targetAdrId;
            x.TargetAdrId = Guid.NewGuid();
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        var otherSourceLink = entityProvider.Create<AdrLink>(x =>
        {
            x.SourceAdrId = otherAdrId;
            x.TargetAdrId = targetAdrId;
            x.Type = AdrLinkType.RelatedTo;
        });
        await Context.AddRangeAsync(targetLink1, targetLink2, deletedLink, otherSourceLink);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrLinkReadRepository.GetBySourceAdrIdAsync(targetAdrId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == targetLink1.Id)
            .And.ContainSingle(x => x.Id == targetLink2.Id);
    }

    /// <summary>
    /// Возвращает связи ADR как цели, исключая удалённые и связи других ADR
    /// </summary>
    [Fact]
    public async Task GetByTargetAdrIdShouldReturnValue()
    {
        // Arrange
        var targetAdrId = Guid.NewGuid();
        var otherAdrId = Guid.NewGuid();
        var targetLink1 = entityProvider.Create<AdrLink>(x =>
        {
            x.SourceAdrId = Guid.NewGuid();
            x.TargetAdrId = targetAdrId;
            x.Type = AdrLinkType.RelatedTo;
        });
        var targetLink2 = entityProvider.Create<AdrLink>(x =>
        {
            x.SourceAdrId = Guid.NewGuid();
            x.TargetAdrId = targetAdrId;
            x.Type = AdrLinkType.DeprecatedBy;
        });
        var deletedLink = entityProvider.Create<AdrLink>(x =>
        {
            x.SourceAdrId = Guid.NewGuid();
            x.TargetAdrId = targetAdrId;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        var otherTargetLink = entityProvider.Create<AdrLink>(x =>
        {
            x.SourceAdrId = targetAdrId;
            x.TargetAdrId = otherAdrId;
            x.Type = AdrLinkType.RelatedTo;
        });
        await Context.AddRangeAsync(targetLink1, targetLink2, deletedLink, otherTargetLink);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrLinkReadRepository.GetByTargetAdrIdAsync(targetAdrId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == targetLink1.Id)
            .And.ContainSingle(x => x.Id == targetLink2.Id);
    }

    /// <summary>
    /// Проверяет, что связь между указанными ADR не существует
    /// </summary>
    [Fact]
    public async Task IsActiveLinkExistsShouldReturnFalse()
    {
        // Arrange
        var targetSourceAdrId = Guid.NewGuid();
        var targetTargetAdrId = Guid.NewGuid();

        // Act
        var result = await adrLinkReadRepository.IsActiveLinkExistsAsync(targetSourceAdrId,
            targetTargetAdrId,
            AdrLinkType.RelatedTo,
            CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Проверяет, что связь между указанными ADR не существует т.к. она удалена
    /// </summary>
    [Fact]
    public async Task IsActiveLinkExistsShouldReturnFalseIfDeleted()
    {
        // Arrange
        var targetSourceAdrId = Guid.NewGuid();
        var targetTargetAdrId = Guid.NewGuid();
        var deletedLink = entityProvider.Create<AdrLink>(x =>
        {
            x.SourceAdrId = targetSourceAdrId;
            x.TargetAdrId = targetTargetAdrId;
            x.Type = AdrLinkType.RelatedTo;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        await Context.AddAsync(deletedLink);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrLinkReadRepository.IsActiveLinkExistsAsync(targetSourceAdrId,
            targetTargetAdrId,
            AdrLinkType.RelatedTo,
            CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Проверяет, что связь между указанными ADR существует
    /// </summary>
    [Fact]
    public async Task IsActiveLinkExistsShouldReturnTrue()
    {
        // Arrange
        var targetSourceAdrId = Guid.NewGuid();
        var targetTargetAdrId = Guid.NewGuid();
        var targetLink = entityProvider.Create<AdrLink>(x =>
        {
            x.SourceAdrId = targetSourceAdrId;
            x.TargetAdrId = targetTargetAdrId;
            x.Type = AdrLinkType.Supersedes;
        });
        await Context.AddAsync(targetLink);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrLinkReadRepository.IsActiveLinkExistsAsync(targetSourceAdrId,
            targetTargetAdrId,
            AdrLinkType.Supersedes,
            CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Проверяет, что связь того же типа между другими ADR не учитывается
    /// </summary>
    [Fact]
    public async Task IsActiveLinkExistsShouldReturnFalseForOtherType()
    {
        // Arrange
        var targetSourceAdrId = Guid.NewGuid();
        var targetTargetAdrId = Guid.NewGuid();
        var targetLink = entityProvider.Create<AdrLink>(x =>
        {
            x.SourceAdrId = targetSourceAdrId;
            x.TargetAdrId = targetTargetAdrId;
            x.Type = AdrLinkType.RelatedTo;
        });
        await Context.AddAsync(targetLink);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrLinkReadRepository.IsActiveLinkExistsAsync(targetSourceAdrId,
            targetTargetAdrId,
            AdrLinkType.Supersedes,
            CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }
}
