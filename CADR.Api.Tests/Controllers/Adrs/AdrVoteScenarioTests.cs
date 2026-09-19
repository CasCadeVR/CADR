using Ahatornn.TestGenerator;
using CADR.Administrations.Entities;
using CADR.Administrations.Entities.Enums;
using CADR.Api.Client;
using CADR.Api.Tests.Infrastructures;
using CADR.Common.Mvc.Models;
using FluentAssertions;
using Xunit;

namespace CADR.Api.Tests.Controllers.Adrs;

/// <summary>
/// Тесты сценариев голосования за ADR
/// </summary>
[Collection(nameof(CadrApiAuthorizedUserTestCollection))]
public class AdrVoteScenarioTests
{
    private readonly CadrApiAuthorizedUserFixture fixture;
    private readonly ICadrApiClient apiClient;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrVoteScenarioTests"/>
    /// </summary>
    public AdrVoteScenarioTests(CadrApiAuthorizedUserFixture fixture)
    {
        this.fixture = fixture;
        apiClient = fixture.CreateApiClient();
    }

    /// <summary>
    /// Голос архитектора за предложенный ADR автоматически утверждает его
    /// </summary>
    [Fact]
    public async Task AdrVoteShouldAutoApproveProposedAdr()
    {
        // Arrange
        var organization = await apiClient.OrganizationCreateAsync(new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        });
        var adr = await apiClient.AdrCreateAsync(new CreateAdrApiModel
        {
            Title = $"Title{Guid.NewGuid():N}",
            Status = AdrStatusApi.Draft,
            OrganizationId = organization.Id,
        });
        await apiClient.AdrChangeStatusAsync(adr.Id, new ChangeAdrStatusApiModel
        {
            Status = AdrStatusApi.Proposed,
        });
        var voterOptions = await SeedSecondMemberAsync(organization.Id);
        var voterClient = fixture.CreateApiClient(voterOptions);

        // Act
        await voterClient.AdrVoteAsync(adr.Id, new VoteAdrApiModel
        {
            Vote = AdrVoteTypeApi.Like,
        });
        var getResult = await voterClient.AdrGetAsync(adr.Id);

        // Assert
        getResult.Score.Should().Be(1);
        getResult.UserVote.Should().Be(AdrVoteTypeApi.Like);
        getResult.Status.Should().Be(AdrStatusApi.Approved);
    }

    /// <summary>
    /// Снятие голоса обнуляет авторитет ADR и голос пользователя
    /// </summary>
    [Fact]
    public async Task AdrWithdrawVoteShouldResetScore()
    {
        // Arrange
        var organization = await apiClient.OrganizationCreateAsync(new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        });
        var adr = await apiClient.AdrCreateAsync(new CreateAdrApiModel
        {
            Title = $"Title{Guid.NewGuid():N}",
            Status = AdrStatusApi.Draft,
            OrganizationId = organization.Id,
        });
        await apiClient.AdrChangeStatusAsync(adr.Id, new ChangeAdrStatusApiModel
        {
            Status = AdrStatusApi.Proposed,
        });
        var voterOptions = await SeedSecondMemberAsync(organization.Id);
        var voterClient = fixture.CreateApiClient(voterOptions);
        await voterClient.AdrVoteAsync(adr.Id, new VoteAdrApiModel
        {
            Vote = AdrVoteTypeApi.Like,
        });

        // Act
        await voterClient.AdrWithdrawVoteAsync(adr.Id);
        var getResult = await voterClient.AdrGetAsync(adr.Id);

        // Assert
        getResult.Score.Should().Be(0);
        getResult.UserVote.Should().BeNull();
        getResult.Status.Should().Be(AdrStatusApi.Approved);
    }

    /// <summary>
    /// Добавляет в контекст второго участника организации с ролью архитектора
    /// </summary>
    private async Task<PersonalOptions> SeedSecondMemberAsync(Guid organizationId)
    {
        var options = new PersonalOptions
        {
            Identifier = Guid.NewGuid(),
            Email = $"Email{Guid.NewGuid():N}",
            Login = $"Login{Guid.NewGuid():N}",
            Name = $"Name{Guid.NewGuid():N}",
            SecurityStamp = Guid.NewGuid().ToString(),
            Params = new Dictionary<string, string>(),
        };
        fixture.CadrContext.Add(TestEntityProvider.Shared.Create<User>(x =>
        {
            x.Id = options.Identifier;
            x.Email = options.Email;
            x.Login = options.Login;
            x.Name = options.Name;
            x.SecurityStamp = options.SecurityStamp;
            x.EmailLowerCase = x.Email.ToLower();
            x.LoginLowerCase = x.Login.ToLower();
        }));
        fixture.CadrContext.Add(TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.UserId = options.Identifier;
            x.OrganizationId = organizationId;
            x.Role = Role.Architect;
        }));
        await fixture.UnitOfWork.SaveChangesAsync();
        return options;
    }
}
