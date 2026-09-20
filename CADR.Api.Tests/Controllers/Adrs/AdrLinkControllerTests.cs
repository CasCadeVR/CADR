using CADR.Api.Client;
using CADR.Api.Tests.Infrastructures;
using FluentAssertions;
using Xunit;

namespace CADR.Api.Tests.Controllers.Adrs;

/// <summary>
/// Тесты сценариев работы со связями ADR
/// </summary>
[Collection(nameof(CadrApiAuthorizedUserTestCollection))]
public class AdrLinkControllerTests
{
    private readonly ICadrApiClient apiClient;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrLinkControllerTests"/>
    /// </summary>
    public AdrLinkControllerTests(CadrApiAuthorizedUserFixture fixture)
    {
        apiClient = fixture.CreateApiClient();
    }

    /// <summary>
    /// Создаёт связь ADR и получает её в обе стороны
    /// </summary>
    [Fact]
    public async Task AdrLinkCreateAndGetByAdrShouldReturnLink()
    {
        // Arrange
        var organization = await apiClient.OrganizationCreateAsync(new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        });
        var source = await apiClient.AdrCreateAsync(new CreateAdrApiModel
        {
            Title = $"SourceTitle{Guid.NewGuid():N}",
            Status = AdrStatusApi.Draft,
            OrganizationId = organization.Id,
        });
        var target = await apiClient.AdrCreateAsync(new CreateAdrApiModel
        {
            Title = $"TargetTitle{Guid.NewGuid():N}",
            Status = AdrStatusApi.Draft,
            OrganizationId = organization.Id,
        });
        var createModel = new CreateAdrLinkApiModel
        {
            Type = AdrLinkTypeApi.Supersedes,
            SourceAdrId = source.Id,
            TargetAdrId = target.Id,
        };

        // Act
        var createResult = await apiClient.AdrLinkCreateAsync(createModel);
        var sourceLinks = await apiClient.AdrLinkGetByAdrAsync(source.Id);
        var targetLinks = await apiClient.AdrLinkGetByAdrAsync(target.Id);

        // Assert
        createResult.Should().NotBeNull();
        createResult.Type.Should().Be(AdrLinkTypeApi.Supersedes);
        createResult.SourceAdrId.Should().Be(source.Id);
        createResult.TargetAdrId.Should().Be(target.Id);
        createResult.TargetAdrNumber.Should().Be(target.Number);
        createResult.TargetAdrName.Should().Be(target.Title);
        sourceLinks.Should().ContainSingle(x => x.Id == createResult.Id);
        targetLinks.Should().ContainSingle(x => x.Id == createResult.Id);
    }

    /// <summary>
    /// Удаляет связь ADR
    /// </summary>
    [Fact]
    public async Task AdrLinkDeleteShouldRemoveLink()
    {
        // Arrange
        var organization = await apiClient.OrganizationCreateAsync(new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        });
        var source = await apiClient.AdrCreateAsync(new CreateAdrApiModel
        {
            Title = $"SourceTitle{Guid.NewGuid():N}",
            Status = AdrStatusApi.Draft,
            OrganizationId = organization.Id,
        });
        var target = await apiClient.AdrCreateAsync(new CreateAdrApiModel
        {
            Title = $"TargetTitle{Guid.NewGuid():N}",
            Status = AdrStatusApi.Draft,
            OrganizationId = organization.Id,
        });
        var link = await apiClient.AdrLinkCreateAsync(new CreateAdrLinkApiModel
        {
            Type = AdrLinkTypeApi.RelatedTo,
            SourceAdrId = source.Id,
            TargetAdrId = target.Id,
        });

        // Act
        await apiClient.AdrLinkDeleteAsync(link.Id);
        var sourceLinks = await apiClient.AdrLinkGetByAdrAsync(source.Id);
        var targetLinks = await apiClient.AdrLinkGetByAdrAsync(target.Id);

        // Assert
        sourceLinks.Should().BeEmpty();
        targetLinks.Should().BeEmpty();
    }
}
