using CADR.Api.Client;
using CADR.Api.Tests.Infrastructures;
using FluentAssertions;
using Xunit;

namespace CADR.Api.Tests.Controllers.Adrs;

/// <summary>
/// Тесты сценариев работы с комментариями ADR
/// </summary>
[Collection(nameof(CadrApiAuthorizedUserTestCollection))]
public class AdrCommentControllerTests
{
    private readonly ICadrApiClient apiClient;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrCommentControllerTests"/>
    /// </summary>
    public AdrCommentControllerTests(CadrApiAuthorizedUserFixture fixture)
    {
        apiClient = fixture.CreateApiClient();
    }

    /// <summary>
    /// Создаёт комментарий ADR и получает его в списке
    /// </summary>
    [Fact]
    public async Task AdrCommentCreateAndGetShouldReturnValue()
    {
        // Arrange
        var adr = await CreateAdrAsync();
        var createModel = new CreateAdrCommentApiModel
        {
            Text = $"Text{Guid.NewGuid():N}",
            AdrId = adr.Id,
        };

        // Act
        var createResult = await apiClient.AdrCommentCreateAsync(createModel);
        var getResult = await apiClient.AdrCommentGetByAdrAsync(adr.Id);

        // Assert
        createResult.Should().NotBeNull();
        createResult.Text.Should().Be(createModel.Text);
        createResult.AdrId.Should().Be(adr.Id);
        createResult.AuthorId.Should().NotBeEmpty();
        createResult.AuthorName.Should().NotBeNullOrEmpty();
        createResult.AuthorLogin.Should().NotBeNullOrEmpty();
        getResult.Should().ContainSingle(x => x.Id == createResult.Id && x.Text == createModel.Text);
    }

    /// <summary>
    /// Обновляет текст комментария ADR
    /// </summary>
    [Fact]
    public async Task AdrCommentUpdateShouldChangeText()
    {
        // Arrange
        var adr = await CreateAdrAsync();
        var comment = await apiClient.AdrCommentCreateAsync(new CreateAdrCommentApiModel
        {
            Text = $"Text{Guid.NewGuid():N}",
            AdrId = adr.Id,
        });
        var updateModel = new UpdateAdrCommentApiModel
        {
            Id = comment.Id,
            Text = $"NewText{Guid.NewGuid():N}",
            AdrId = adr.Id,
        };

        // Act
        var updateResult = await apiClient.AdrCommentUpdateAsync(updateModel);
        var getResult = await apiClient.AdrCommentGetByAdrAsync(adr.Id);

        // Assert
        updateResult.Should().NotBeNull();
        updateResult.Id.Should().Be(comment.Id);
        updateResult.Text.Should().Be(updateModel.Text);
        getResult.Should().ContainSingle(x => x.Id == comment.Id && x.Text == updateModel.Text);
    }

    /// <summary>
    /// Удаляет комментарий ADR
    /// </summary>
    [Fact]
    public async Task AdrCommentDeleteShouldRemoveComment()
    {
        // Arrange
        var adr = await CreateAdrAsync();
        var comment = await apiClient.AdrCommentCreateAsync(new CreateAdrCommentApiModel
        {
            Text = $"Text{Guid.NewGuid():N}",
            AdrId = adr.Id,
        });

        // Act
        await apiClient.AdrCommentDeleteAsync(comment.Id);
        var getResult = await apiClient.AdrCommentGetByAdrAsync(adr.Id);

        // Assert
        getResult.Should().BeEmpty();
    }

    /// <summary>
    /// Создаёт организацию и ADR в ней
    /// </summary>
    private async Task<AdrApiModel> CreateAdrAsync()
    {
        var organization = await apiClient.OrganizationCreateAsync(new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        });
        return await apiClient.AdrCreateAsync(new CreateAdrApiModel
        {
            Title = $"Title{Guid.NewGuid():N}",
            Status = AdrStatusApi.Draft,
            OrganizationId = organization.Id,
        });
    }
}
