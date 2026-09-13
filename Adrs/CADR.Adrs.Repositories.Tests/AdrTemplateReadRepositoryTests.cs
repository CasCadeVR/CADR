using Ahatornn.TestGenerator;
using CADR.Administrations.Entities;
using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;

namespace CADR.Adrs.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="IAdrTemplateReadRepository"/>
/// </summary>
public class AdrTemplateReadRepositoryTests : CadrContextInMemory
{
    private readonly IAdrTemplateReadRepository adrTemplateReadRepository;
    private readonly TestEntityProvider entityProvider;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrTemplateReadRepositoryTests"/>
    /// </summary>
    public AdrTemplateReadRepositoryTests()
    {
        entityProvider = new TestEntityProviderBuilder()
            .AddPreset<AdrTemplate>(x =>
                x.Name = $"Template{Guid.NewGuid():N}")
            .Build();
        adrTemplateReadRepository = new AdrTemplateReadRepository(Context);
    }

    /// <summary>
    /// Возвращает null если не находит шаблон по Id
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnNull()
    {
        // Arrange
        var targetId = Guid.NewGuid();

        // Act
        var result = await adrTemplateReadRepository.GetActiveByIdAsync(targetId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает null если не находит шаблон по Id т.к. он удалён
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnNullIfDeleted()
    {
        // Arrange
        var targetTemplate = entityProvider.Create<AdrTemplate>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddAsync(targetTemplate);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrTemplateReadRepository.GetActiveByIdAsync(targetTemplate.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает шаблон по Id
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnValue()
    {
        // Arrange
        var targetTemplate = entityProvider.Create<AdrTemplate>();
        await Context.AddAsync(targetTemplate);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrTemplateReadRepository.GetActiveByIdAsync(targetTemplate.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(targetTemplate, options =>
                options.Excluding(o => o.Sections));
    }

    /// <summary>
    /// Возвращает пустой список шаблонов организации
    /// </summary>
    [Fact]
    public async Task GetAvailableForOrganizationShouldReturnEmpty()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();

        // Act
        var result = await adrTemplateReadRepository.GetAvailableForOrganizationAsync(targetOrganizationId, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Возвращает глобальные и собственные шаблоны организации,
    /// исключая удалённые и шаблоны других организаций
    /// </summary>
    [Fact]
    public async Task GetAvailableForOrganizationShouldReturnGlobalAndOwn()
    {
        // Arrange
        var targetOrganization = entityProvider.Create<Organization>();
        var otherOrganization = entityProvider.Create<Organization>();
        var globalTemplate = entityProvider.Create<AdrTemplate>();
        var ownTemplate = entityProvider.Create<AdrTemplate>(x => x.OrganizationId = targetOrganization.Id);
        var deletedOwnTemplate = entityProvider.Create<AdrTemplate>(x =>
        {
            x.OrganizationId = targetOrganization.Id;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        var otherOrganizationTemplate = entityProvider.Create<AdrTemplate>(x => x.OrganizationId = otherOrganization.Id);
        await Context.AddRangeAsync(targetOrganization,
            otherOrganization,
            globalTemplate,
            ownTemplate,
            deletedOwnTemplate,
            otherOrganizationTemplate);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrTemplateReadRepository.GetAvailableForOrganizationAsync(targetOrganization.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == globalTemplate.Id)
            .And.ContainSingle(x => x.Id == ownTemplate.Id);
    }

    /// <summary>
    /// Возвращает шаблоны организации отсортированные по имени
    /// </summary>
    [Fact]
    public async Task GetAvailableForOrganizationShouldReturnOrderedByName()
    {
        // Arrange
        var targetOrganization = entityProvider.Create<Organization>();
        var templateC = entityProvider.Create<AdrTemplate>(x => x.Name = "ccc");
        var templateA = entityProvider.Create<AdrTemplate>(x => x.Name = "aaa");
        var templateB = entityProvider.Create<AdrTemplate>(x => x.Name = "bbb");
        await Context.AddRangeAsync(targetOrganization, templateC, templateA, templateB);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrTemplateReadRepository.GetAvailableForOrganizationAsync(targetOrganization.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(3)
            .And.ContainInOrder(templateA, templateB, templateC);
    }

    /// <summary>
    /// Проверяет, что шаблон с указанным именем не существует в организации
    /// </summary>
    [Fact]
    public async Task IsActiveNameExistsShouldReturnFalse()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();
        await Context.AddAsync(entityProvider.Create<Organization>(x => x.Id = targetOrganizationId));
        await Context.SaveChangesAsync();

        // Act
        var result = await adrTemplateReadRepository.IsActiveNameExistsAsync(targetOrganizationId,
            $"CheckIfTemplateExistShouldReturnFalse{Guid.NewGuid():N}",
            CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Проверяет, что шаблон с указанным именем не существует т.к. он удалён
    /// </summary>
    [Fact]
    public async Task IsActiveNameExistsShouldReturnFalseIfDeleted()
    {
        // Arrange
        var targetOrganization = entityProvider.Create<Organization>();
        var targetTemplate = entityProvider.Create<AdrTemplate>(x =>
        {
            x.OrganizationId = targetOrganization.Id;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        await Context.AddRangeAsync(targetOrganization, targetTemplate);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrTemplateReadRepository.IsActiveNameExistsAsync(targetOrganization.Id, targetTemplate.Name, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Проверяет, что шаблон с указанным именем существует в организации
    /// </summary>
    [Fact]
    public async Task IsActiveNameExistsShouldReturnTrue()
    {
        // Arrange
        var targetOrganization = entityProvider.Create<Organization>();
        var targetTemplate = entityProvider.Create<AdrTemplate>(x => x.OrganizationId = targetOrganization.Id);
        await Context.AddRangeAsync(targetOrganization, targetTemplate);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrTemplateReadRepository.IsActiveNameExistsAsync(targetOrganization.Id, targetTemplate.Name, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Проверяет, что имя шаблона сравнивается без учёта регистра
    /// </summary>
    [Fact]
    public async Task IsActiveNameExistsShouldReturnTrueIgnoreCase()
    {
        // Arrange
        var targetOrganization = entityProvider.Create<Organization>();
        var targetTemplate = entityProvider.Create<AdrTemplate>(x => x.OrganizationId = targetOrganization.Id);
        await Context.AddRangeAsync(targetOrganization, targetTemplate);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrTemplateReadRepository.IsActiveNameExistsAsync(targetOrganization.Id,
            targetTemplate.Name.ToUpper(),
            CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }
}
