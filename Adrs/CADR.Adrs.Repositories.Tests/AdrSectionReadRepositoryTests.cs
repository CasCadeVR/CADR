using Ahatornn.TestGenerator;
using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;

namespace CADR.Adrs.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="IAdrSectionReadRepository"/>
/// </summary>
public class AdrSectionReadRepositoryTests : CadrContextInMemory
{
    private readonly IAdrSectionReadRepository adrSectionReadRepository;
    private readonly TestEntityProvider entityProvider;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrSectionReadRepositoryTests"/>
    /// </summary>
    public AdrSectionReadRepositoryTests()
    {
        entityProvider = new TestEntityProviderBuilder()
            .AddPreset<AdrSection>(x =>
                x.Title = $"Section{Guid.NewGuid():N}")
            .Build();
        adrSectionReadRepository = new AdrSectionReadRepository(Context);
    }

    /// <summary>
    /// Возвращает пустой список секций ADR
    /// </summary>
    [Fact]
    public async Task GetByAdrIdShouldReturnEmpty()
    {
        // Arrange
        var targetAdrId = Guid.NewGuid();

        // Act
        var result = await adrSectionReadRepository.GetByAdrIdAsync(targetAdrId, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Возвращает секции ADR, исключая удалённые и секции других ADR
    /// </summary>
    [Fact]
    public async Task GetByAdrIdShouldReturnValue()
    {
        // Arrange
        var targetAdrId = Guid.NewGuid();
        var otherAdrId = Guid.NewGuid();
        var targetSection1 = entityProvider.Create<AdrSection>(x =>
        {
            x.AdrId = targetAdrId;
            x.Position = 0;
        });
        var targetSection2 = entityProvider.Create<AdrSection>(x =>
        {
            x.AdrId = targetAdrId;
            x.Position = 1;
        });
        var deletedSection = entityProvider.Create<AdrSection>(x =>
        {
            x.AdrId = targetAdrId;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        var otherAdrSection = entityProvider.Create<AdrSection>(x =>
        {
            x.AdrId = otherAdrId;
            x.Position = 0;
        });
        await Context.AddRangeAsync(targetSection1, targetSection2, deletedSection, otherAdrSection);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrSectionReadRepository.GetByAdrIdAsync(targetAdrId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == targetSection1.Id)
            .And.ContainSingle(x => x.Id == targetSection2.Id);
    }

    /// <summary>
    /// Возвращает секции ADR отсортированные по позиции
    /// </summary>
    [Fact]
    public async Task GetByAdrIdShouldReturnOrderedByPosition()
    {
        // Arrange
        var targetAdrId = Guid.NewGuid();
        var sectionThird = entityProvider.Create<AdrSection>(x =>
        {
            x.AdrId = targetAdrId;
            x.Position = 2;
        });
        var sectionFirst = entityProvider.Create<AdrSection>(x =>
        {
            x.AdrId = targetAdrId;
            x.Position = 0;
        });
        var sectionSecond = entityProvider.Create<AdrSection>(x =>
        {
            x.AdrId = targetAdrId;
            x.Position = 1;
        });
        await Context.AddRangeAsync(sectionThird, sectionFirst, sectionSecond);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrSectionReadRepository.GetByAdrIdAsync(targetAdrId, CancellationToken.None);

        // Assert
        result.Select(x => x.Id).Should()
            .NotBeEmpty()
            .And.ContainInOrder(sectionFirst.Id, sectionSecond.Id, sectionThird.Id);
    }
}
