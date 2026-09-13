using Ahatornn.TestGenerator;
using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;

namespace CADR.Adrs.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="IAdrTemplateSectionReadRepository"/>
/// </summary>
public class AdrTemplateSectionReadRepositoryTests : CadrContextInMemory
{
    private readonly IAdrTemplateSectionReadRepository adrTemplateSectionReadRepository;
    private readonly TestEntityProvider entityProvider;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrTemplateSectionReadRepositoryTests"/>
    /// </summary>
    public AdrTemplateSectionReadRepositoryTests()
    {
        entityProvider = new TestEntityProviderBuilder()
            .AddPreset<AdrTemplate>(x =>
                x.Name = $"Template{Guid.NewGuid():N}")
            .AddPreset<AdrTemplateSection>(x =>
                x.Title = $"Section{Guid.NewGuid():N}")
            .Build();
        adrTemplateSectionReadRepository = new AdrTemplateSectionReadRepository(Context);
    }

    /// <summary>
    /// Возвращает пустой список секций шаблона
    /// </summary>
    [Fact]
    public async Task GetByTemplateIdShouldReturnEmpty()
    {
        // Arrange
        var targetTemplateId = Guid.NewGuid();

        // Act
        var result = await adrTemplateSectionReadRepository.GetByTemplateIdAsync(targetTemplateId, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Возвращает секции шаблона, исключая удалённые и секции других шаблонов
    /// </summary>
    [Fact]
    public async Task GetByTemplateIdShouldReturnValue()
    {
        // Arrange
        var targetTemplate = entityProvider.Create<AdrTemplate>();
        var otherTemplate = entityProvider.Create<AdrTemplate>();
        var targetSection1 = entityProvider.Create<AdrTemplateSection>(x =>
        {
            x.TemplateId = targetTemplate.Id;
            x.Position = 0;
        });
        var targetSection2 = entityProvider.Create<AdrTemplateSection>(x =>
        {
            x.TemplateId = targetTemplate.Id;
            x.Position = 1;
        });
        var deletedSection = entityProvider.Create<AdrTemplateSection>(x =>
        {
            x.TemplateId = targetTemplate.Id;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        var otherTemplateSection = entityProvider.Create<AdrTemplateSection>(x =>
        {
            x.TemplateId = otherTemplate.Id;
            x.Position = 0;
        });
        await Context.AddRangeAsync(targetTemplate, otherTemplate, targetSection1, targetSection2, deletedSection, otherTemplateSection);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrTemplateSectionReadRepository.GetByTemplateIdAsync(targetTemplate.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == targetSection1.Id)
            .And.ContainSingle(x => x.Id == targetSection2.Id);
    }

    /// <summary>
    /// Возвращает секции шаблона отсортированные по позиции
    /// </summary>
    [Fact]
    public async Task GetByTemplateIdShouldReturnOrderedByPosition()
    {
        // Arrange
        var targetTemplate = entityProvider.Create<AdrTemplate>();
        var sectionThird = entityProvider.Create<AdrTemplateSection>(x =>
        {
            x.TemplateId = targetTemplate.Id;
            x.Position = 2;
        });
        var sectionFirst = entityProvider.Create<AdrTemplateSection>(x =>
        {
            x.TemplateId = targetTemplate.Id;
            x.Position = 0;
        });
        var sectionSecond = entityProvider.Create<AdrTemplateSection>(x =>
        {
            x.TemplateId = targetTemplate.Id;
            x.Position = 1;
        });
        await Context.AddRangeAsync(targetTemplate, sectionThird, sectionFirst, sectionSecond);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrTemplateSectionReadRepository.GetByTemplateIdAsync(targetTemplate.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(3)
            .And.ContainInOrder(sectionFirst, sectionSecond, sectionThird);
    }
}
