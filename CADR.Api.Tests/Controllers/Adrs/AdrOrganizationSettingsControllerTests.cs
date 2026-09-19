using CADR.Api.Client;
using CADR.Api.Tests.Infrastructures;
using FluentAssertions;
using Xunit;

namespace CADR.Api.Tests.Controllers.Adrs;

/// <summary>
/// Тесты сценариев работы с настройками ADR организации
/// </summary>
[Collection(nameof(CadrApiAuthorizedUserTestCollection))]
public class AdrOrganizationSettingsControllerTests
{
    private readonly ICadrApiClient apiClient;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrOrganizationSettingsControllerTests"/>
    /// </summary>
    public AdrOrganizationSettingsControllerTests(CadrApiAuthorizedUserFixture fixture)
    {
        apiClient = fixture.CreateApiClient();
    }

    /// <summary>
    /// Получает настройки ADR по умолчанию для новой организации
    /// </summary>
    [Fact]
    public async Task AdrOrganizationSettingsGetShouldReturnDefaults()
    {
        // Arrange
        var organization = await apiClient.OrganizationCreateAsync(new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        });

        // Act
        var getResult = await apiClient.AdrOrganizationSettingsGetByOrganizationIdAsync(organization.Id);

        // Assert
        getResult.Should().NotBeNull();
        getResult.OrganizationId.Should().Be(organization.Id);
        getResult.LikesRequiredForApproval.Should().Be(1);
        getResult.Id.Should().BeEmpty();
    }

    /// <summary>
    /// Сохраняет настройки ADR организации и получает их
    /// </summary>
    [Fact]
    public async Task AdrOrganizationSettingsSaveShouldCreateThenUpdateSettings()
    {
        // Arrange
        var organization = await apiClient.OrganizationCreateAsync(new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        });

        // Act
        var createResult = await apiClient.AdrOrganizationSettingsSaveAsync(new UpdateAdrOrganizationSettingsApiModel
        {
            OrganizationId = organization.Id,
            LikesRequiredForApproval = 2,
        });
        var firstGet = await apiClient.AdrOrganizationSettingsGetByOrganizationIdAsync(organization.Id);
        var updateResult = await apiClient.AdrOrganizationSettingsSaveAsync(new UpdateAdrOrganizationSettingsApiModel
        {
            Id = createResult.Id,
            OrganizationId = organization.Id,
            LikesRequiredForApproval = 3,
        });
        var secondGet = await apiClient.AdrOrganizationSettingsGetByOrganizationIdAsync(organization.Id);

        // Assert
        createResult.Should().NotBeNull();
        createResult.OrganizationId.Should().Be(organization.Id);
        createResult.LikesRequiredForApproval.Should().Be(2);
        createResult.Id.Should().NotBeEmpty();
        firstGet.LikesRequiredForApproval.Should().Be(2);
        firstGet.Id.Should().Be(createResult.Id);
        updateResult.Id.Should().Be(createResult.Id);
        updateResult.LikesRequiredForApproval.Should().Be(3);
        secondGet.LikesRequiredForApproval.Should().Be(3);
        secondGet.Id.Should().Be(createResult.Id);
    }
}
