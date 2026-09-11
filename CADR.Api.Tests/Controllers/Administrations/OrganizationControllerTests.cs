using CADR.Api.Client;
using CADR.Api.Tests.Infrastructures;
using FluentAssertions;
using Xunit;

namespace CADR.Api.Tests.Controllers.Administrations;

/// <summary>
/// Тесты сценариев работы с организацией
/// </summary>
[Collection(nameof(CadrApiAuthorizedUserTestCollection))]
public class OrganizationControllerTests
{
    private readonly ICadrApiClient apiClient;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="OrganizationControllerTests"/>
    /// </summary>
    public OrganizationControllerTests(CadrApiAuthorizedUserFixture fixture)
    {
        apiClient = fixture.CreateApiClient();
    }

    /// <summary>
    /// Создаёт организацию и получает её в виде списка
    /// </summary>
    [Fact]
    public async Task OrganizationCreateAndGetShouldReturnValue()
    {
        // Arrange
        var createModel = new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        };

        // Act
        var createResult = await apiClient.OrganizationCreateAsync(createModel);
        var getResult = await apiClient.OrganizationGetAsync();

        // Assert
        createResult.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { createModel.Name, createModel.Description, });
        getResult.Should()
            .NotBeNull()
            .And.ContainSingle(x => x.Id == createResult.Id);
    }

    /// <summary>
    /// Создаёт организацию и редактирует её
    /// </summary>
    [Fact]
    public async Task OrganizationCreateAndUpdateShouldReturnValue()
    {
        // Arrange
        var createModel = new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        };
        var updateModel = new OrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        };

        // Act
        var createResult = await apiClient.OrganizationCreateAsync(createModel);
        updateModel.Id = createResult.Id;
        await apiClient.OrganizationUpdateAsync(updateModel);
        var getResult = await apiClient.OrganizationGetAsync();

        // Assert
        createResult.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { createModel.Name, createModel.Description, });
        getResult.Should()
            .NotBeNull()
            .And.ContainSingle(x => x.Id == createResult.Id &&
                                    x.Name == updateModel.Name &&
                                    x.Description == updateModel.Description);
    }

    /// <summary>
    /// Создаёт организацию и удаляет её
    /// </summary>
    [Fact]
    public async Task OrganizationCreateAndDeleteShouldReturnValue()
    {
        // Arrange
        var createModel = new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        };

        // Act
        var createResult = await apiClient.OrganizationCreateAsync(createModel);
        await apiClient.OrganizationDeleteAsync(createResult.Id);
        var getResult = await apiClient.OrganizationGetAsync();

        // Assert
        createResult.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { createModel.Name, createModel.Description, });
        getResult.Should().NotContain(x => x.Id == createResult.Id);
    }
}
