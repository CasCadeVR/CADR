using Ahatornn.TestGenerator;
using CADR.Adrs.Entities;
using CADR.Adrs.Entities.Enums;
using CADR.Adrs.Repositories.Contracts;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;

namespace CADR.Adrs.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="IAdrVoteReadRepository"/>
/// </summary>
public class AdrVoteReadRepositoryTests : CadrContextInMemory
{
    private readonly IAdrVoteReadRepository adrVoteReadRepository;
    private readonly TestEntityProvider entityProvider;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrVoteReadRepositoryTests"/>
    /// </summary>
    public AdrVoteReadRepositoryTests()
    {
        entityProvider = new TestEntityProviderBuilder()
            .Build();
        adrVoteReadRepository = new AdrVoteReadRepository(Context);
    }

    /// <summary>
    /// Возвращает null если пользователь не голосовал за ADR
    /// </summary>
    [Fact]
    public async Task GetByAdrAndUserIdShouldReturnNull()
    {
        // Arrange
        var targetAdrId = Guid.NewGuid();
        var targetUserId = Guid.NewGuid();

        // Act
        var result = await adrVoteReadRepository.GetByAdrAndUserIdAsync(targetAdrId, targetUserId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает null если голос отозван (мягко удалён)
    /// </summary>
    [Fact]
    public async Task GetByAdrAndUserIdShouldReturnNullIfDeleted()
    {
        // Arrange
        var targetAdrId = Guid.NewGuid();
        var targetUserId = Guid.NewGuid();
        var targetVote = entityProvider.Create<AdrVote>(x =>
        {
            x.AdrId = targetAdrId;
            x.UserId = targetUserId;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        await Context.AddAsync(targetVote);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrVoteReadRepository.GetByAdrAndUserIdAsync(targetAdrId, targetUserId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает голос пользователя за ADR
    /// </summary>
    [Theory]
    [InlineData(AdrVoteType.Like)]
    [InlineData(AdrVoteType.Dislike)]
    public async Task GetByAdrAndUserIdShouldReturnValue(AdrVoteType voteType)
    {
        // Arrange
        var targetAdrId = Guid.NewGuid();
        var targetUserId = Guid.NewGuid();
        var targetVote = entityProvider.Create<AdrVote>(x =>
        {
            x.AdrId = targetAdrId;
            x.UserId = targetUserId;
            x.Value = voteType;
        });
        await Context.AddAsync(targetVote);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrVoteReadRepository.GetByAdrAndUserIdAsync(targetAdrId, targetUserId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(targetVote, options => options
                .Excluding(o => o.Adr)
                .Excluding(o => o.User));
    }

    /// <summary>
    /// Возвращает пустой список голосов ADR
    /// </summary>
    [Fact]
    public async Task GetByAdrIdShouldReturnEmpty()
    {
        // Arrange
        var targetAdrId = Guid.NewGuid();

        // Act
        var result = await adrVoteReadRepository.GetByAdrIdAsync(targetAdrId, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Возвращает голоса ADR, исключая отозванные и голоса других ADR
    /// </summary>
    [Fact]
    public async Task GetByAdrIdShouldReturnValue()
    {
        // Arrange
        var targetAdrId = Guid.NewGuid();
        var otherAdrId = Guid.NewGuid();
        var targetVote1 = entityProvider.Create<AdrVote>(x =>
        {
            x.AdrId = targetAdrId;
            x.Value = AdrVoteType.Like;
        });
        var targetVote2 = entityProvider.Create<AdrVote>(x =>
        {
            x.AdrId = targetAdrId;
            x.Value = AdrVoteType.Dislike;
        });
        var deletedVote = entityProvider.Create<AdrVote>(x =>
        {
            x.AdrId = targetAdrId;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        var otherAdrVote = entityProvider.Create<AdrVote>(x => x.AdrId = otherAdrId);
        await Context.AddRangeAsync(targetVote1, targetVote2, deletedVote, otherAdrVote);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrVoteReadRepository.GetByAdrIdAsync(targetAdrId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == targetVote1.Id)
            .And.ContainSingle(x => x.Id == targetVote2.Id);
    }
}
