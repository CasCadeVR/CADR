using CADR.Api.Client;
using CADR.Api.Tests.Infrastructures;
using FluentAssertions;
using Xunit;

namespace CADR.Api.Tests.Controllers.Adrs;

/// <summary>
/// Тесты сценариев работы с шаблонами ADR
/// </summary>
[Collection(nameof(CadrApiAuthorizedUserTestCollection))]
public class AdrTemplateControllerTests
{
    private readonly ICadrApiClient apiClient;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrTemplateControllerTests"/>
    /// </summary>
    public AdrTemplateControllerTests(CadrApiAuthorizedUserFixture fixture)
    {
        apiClient = fixture.CreateApiClient();
    }

    /// <summary>
    /// Создаёт, получает и обновляет шаблон ADR вместе с разделами
    /// </summary>
    [Fact]
    public async Task AdrTemplateCreateGetAndUpdateShouldReturnValue()
    {
        // Arrange
        var organization = await apiClient.OrganizationCreateAsync(new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        });
        var createModel = new CreateAdrTemplateApiModel
        {
            Name = $"Template{Guid.NewGuid():N}",
            OrganizationId = organization.Id,
            Sections = new List<CreateAdrTemplateSectionApiModel>
            {
                new() { Position = 1, Title = "Context", Hint = "ContextHint", Placeholder = "ContextPlaceholder" },
                new() { Position = 2, Title = "Decision", Hint = "DecisionHint", Placeholder = "DecisionPlaceholder" },
            },
        };

        // Act
        var createResult = await apiClient.AdrTemplateCreateAsync(createModel);
        var getResult = await apiClient.AdrTemplateGetAsync(createResult.Id);
        var updateModel = new UpdateAdrTemplateApiModel
        {
            Id = createResult.Id,
            Name = $"NewTemplate{Guid.NewGuid():N}",
            OrganizationId = organization.Id,
            Sections = new List<AdrTemplateSectionApiModel>
            {
                new() { Position = 1, Title = "Context", Hint = "ContextHint", Placeholder = "ContextPlaceholder" },
            },
        };
        var updateResult = await apiClient.AdrTemplateUpdateAsync(updateModel);

        // Assert
        createResult.Should().NotBeNull();
        createResult.Name.Should().Be(createModel.Name);
        createResult.OrganizationId.Should().Be(organization.Id);
        createResult.IsBuiltIn.Should().BeFalse();
        getResult.Sections.Should().BeEquivalentTo(
          createModel.Sections.Select(x => new { x.Position, x.Title, x.Hint, x.Placeholder }),
          options => options.WithStrictOrdering());
        getResult.Should().NotBeNull();
        getResult.Id.Should().Be(createResult.Id);
        getResult.Name.Should().Be(createModel.Name);
        getResult.Sections.Should().HaveCount(2);
        updateResult.Should().NotBeNull();
        updateResult.Id.Should().Be(createResult.Id);
        updateResult.Name.Should().Be(updateModel.Name);
        updateResult.Sections.Should().HaveCount(1);
    }

    /// <summary>
    /// Получает доступные организации шаблоны и проверяет исчезновение шаблона после удаления
    /// </summary>
    [Fact]
    public async Task AdrTemplateGetAvailableForOrganizationShouldContainCreatedAndBeEmptyAfterDelete()
    {
        // Arrange
        var organization = await apiClient.OrganizationCreateAsync(new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        });
        var template = await apiClient.AdrTemplateCreateAsync(new CreateAdrTemplateApiModel
        {
            Name = $"Template{Guid.NewGuid():N}",
            OrganizationId = organization.Id,
            Sections = new List<CreateAdrTemplateSectionApiModel>
            {
                new() { Position = 1, Title = "Context", Hint = "ContextHint", Placeholder = "ContextPlaceholder" },
            },
        });

        // Act
        var beforeDelete = await apiClient.AdrTemplateGetAvailableForOrganizationAsync(organization.Id);
        await apiClient.AdrTemplateDeleteAsync(template.Id);
        var afterDelete = await apiClient.AdrTemplateGetAvailableForOrganizationAsync(organization.Id);

        // Assert
        beforeDelete.Should().Contain(x => x.Id == template.Id && x.OrganizationId == organization.Id && !x.IsBuiltIn);
        afterDelete.Should().NotContain(x => x.Id == template.Id);
    }
}
