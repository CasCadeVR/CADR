using CADR.Api.Client;
using CADR.Api.Tests.Infrastructures;
using FluentAssertions;
using Xunit;

namespace CADR.Api.Tests.Controllers.Adrs;

/// <summary>
/// Тесты сценариев работы с папками ADR
/// </summary>
[Collection(nameof(CadrApiAuthorizedUserTestCollection))]
public class AdrFolderControllerTests
{
    private readonly ICadrApiClient apiClient;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrFolderControllerTests"/>
    /// </summary>
    public AdrFolderControllerTests(CadrApiAuthorizedUserFixture fixture)
    {
        apiClient = fixture.CreateApiClient();
    }

    /// <summary>
    /// Создаёт корневую и вложенную папки и получает путь к вложенной
    /// </summary>
    [Fact]
    public async Task AdrFolderCreateAndGetPathShouldReturnOrderedFolders()
    {
        // Arrange
        var organization = await apiClient.OrganizationCreateAsync(new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        });
        var root = await apiClient.AdrFolderCreateAsync(new CreateAdrFolderApiModel
        {
            Name = $"Root{Guid.NewGuid():N}",
            OrganizationId = organization.Id,
        });
        var child = await apiClient.AdrFolderCreateAsync(new CreateAdrFolderApiModel
        {
            Name = $"Child{Guid.NewGuid():N}",
            OrganizationId = organization.Id,
            ParentAdrFolderId = root.Id,
        });

        // Act
        var path = await apiClient.AdrFolderGetPathAsync(organization.Id, child.Id);
        var folders = await apiClient.AdrFolderGetByOrganizationAsync(organization.Id);

        // Assert
        path.Should().HaveCount(2);
        path.Select(x => x.Id).Should().Equal(root.Id, child.Id);
        folders.Should().Contain(x => x.Id == root.Id && x.Name == root.Name);
        folders.Should().Contain(x => x.Id == child.Id && x.ParentAdrFolderId == root.Id);
    }

    /// <summary>
    /// Получает число ADR в папке и путь к папке ADR
    /// </summary>
    [Fact]
    public async Task AdrFolderGetAdrCountShouldCountAdrsInFolder()
    {
        // Arrange
        var organization = await apiClient.OrganizationCreateAsync(new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        });
        var root = await apiClient.AdrFolderCreateAsync(new CreateAdrFolderApiModel
        {
            Name = $"Root{Guid.NewGuid():N}",
            OrganizationId = organization.Id,
        });
        var child = await apiClient.AdrFolderCreateAsync(new CreateAdrFolderApiModel
        {
            Name = $"Child{Guid.NewGuid():N}",
            OrganizationId = organization.Id,
            ParentAdrFolderId = root.Id,
        });
        var adr = await apiClient.AdrCreateAsync(new CreateAdrApiModel
        {
            Title = $"Title{Guid.NewGuid():N}",
            Status = AdrStatusApi.Draft,
            OrganizationId = organization.Id,
            ParentAdrFolderId = child.Id,
        });

        // Act
        var adrCount = await apiClient.AdrFolderGetAdrCountAsync(organization.Id, child.Id);
        var folderAdrs = await apiClient.AdrGetByFolderAsync(organization.Id, child.Id);
        var adrResult = await apiClient.AdrGetAsync(adr.Id);

        // Assert
        adrCount.Should().Be(1);
        folderAdrs.Should().ContainSingle(x => x.Id == adr.Id);
        adrResult.FolderPath.Should().HaveCount(2);
        adrResult.FolderPath.Select(x => x.Id).Should().Equal(root.Id, child.Id);
    }

    /// <summary>
    /// Переименовывает папку ADR
    /// </summary>
    [Fact]
    public async Task AdrFolderUpdateShouldRenameFolder()
    {
        // Arrange
        var organization = await apiClient.OrganizationCreateAsync(new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        });
        var root = await apiClient.AdrFolderCreateAsync(new CreateAdrFolderApiModel
        {
            Name = $"Root{Guid.NewGuid():N}",
            OrganizationId = organization.Id,
        });
        var updateModel = new UpdateAdrFolderApiModel
        {
            Id = root.Id,
            Name = $"NewName{Guid.NewGuid():N}",
            OrganizationId = organization.Id,
        };

        // Act
        var updateResult = await apiClient.AdrFolderUpdateAsync(updateModel);
        var folders = await apiClient.AdrFolderGetByOrganizationAsync(organization.Id);

        // Assert
        updateResult.Should().NotBeNull();
        updateResult.Id.Should().Be(root.Id);
        updateResult.Name.Should().Be(updateModel.Name);
        folders.Should().Contain(x => x.Id == root.Id && x.Name == updateModel.Name);
    }

    /// <summary>
    /// Удаляет папку ADR вместе с вложенными папками
    /// </summary>
    [Fact]
    public async Task AdrFolderDeleteShouldRemoveFolder()
    {
        // Arrange
        var organization = await apiClient.OrganizationCreateAsync(new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        });
        var root = await apiClient.AdrFolderCreateAsync(new CreateAdrFolderApiModel
        {
            Name = $"Root{Guid.NewGuid():N}",
            OrganizationId = organization.Id,
        });
        var child = await apiClient.AdrFolderCreateAsync(new CreateAdrFolderApiModel
        {
            Name = $"Child{Guid.NewGuid():N}",
            OrganizationId = organization.Id,
            ParentAdrFolderId = root.Id,
        });

        // Act
        await apiClient.AdrFolderDeleteAsync(child.Id);
        var folders = await apiClient.AdrFolderGetByOrganizationAsync(organization.Id);

        // Assert
        folders.Should().Contain(x => x.Id == root.Id);
        folders.Should().NotContain(x => x.Id == child.Id);
    }
}
