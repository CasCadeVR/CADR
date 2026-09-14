using Ahatornn.TestGenerator;
using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;

namespace CADR.Adrs.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="IAdrCommentReadRepository"/>
/// </summary>
public class AdrCommentReadRepositoryTests : CadrContextInMemory
{
    private readonly IAdrCommentReadRepository adrCommentReadRepository;
    private readonly TestEntityProvider entityProvider;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrCommentReadRepositoryTests"/>
    /// </summary>
    public AdrCommentReadRepositoryTests()
    {
        entityProvider = new TestEntityProviderBuilder()
            .AddPreset<AdrComment>(x =>
                x.Text = $"Comment{Guid.NewGuid():N}")
            .Build();
        adrCommentReadRepository = new AdrCommentReadRepository(Context);
    }

    /// <summary>
    /// Возвращает null если не находит комментарий по Id
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnNull()
    {
        // Arrange
        var targetId = Guid.NewGuid();

        // Act
        var result = await adrCommentReadRepository.GetActiveByIdAsync(targetId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает null если не находит комментарий по Id т.к. он удалён
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnNullIfDeleted()
    {
        // Arrange
        var targetComment = entityProvider.Create<AdrComment>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddAsync(targetComment);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrCommentReadRepository.GetActiveByIdAsync(targetComment.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает комментарий по Id
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnValue()
    {
        // Arrange
        var targetComment = entityProvider.Create<AdrComment>();
        await Context.AddAsync(targetComment);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrCommentReadRepository.GetActiveByIdAsync(targetComment.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(targetComment, options => options
                .Excluding(o => o.Adr)
                .Excluding(o => o.Author));
    }

    /// <summary>
    /// Возвращает пустой список комментариев ADR
    /// </summary>
    [Fact]
    public async Task GetByAdrIdShouldReturnEmpty()
    {
        // Arrange
        var targetAdrId = Guid.NewGuid();

        // Act
        var result = await adrCommentReadRepository.GetByAdrIdAsync(targetAdrId, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Возвращает комментарии ADR, исключая удалённые и комментарии других ADR
    /// </summary>
    [Fact]
    public async Task GetByAdrIdShouldReturnValue()
    {
        // Arrange
        var targetAdrId = Guid.NewGuid();
        var otherAdrId = Guid.NewGuid();
        var targetComment1 = entityProvider.Create<AdrComment>(x => x.AdrId = targetAdrId);
        var targetComment2 = entityProvider.Create<AdrComment>(x => x.AdrId = targetAdrId);
        var deletedComment = entityProvider.Create<AdrComment>(x =>
        {
            x.AdrId = targetAdrId;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        var otherAdrComment = entityProvider.Create<AdrComment>(x => x.AdrId = otherAdrId);
        await Context.AddRangeAsync(targetComment1, targetComment2, deletedComment, otherAdrComment);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrCommentReadRepository.GetByAdrIdAsync(targetAdrId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == targetComment1.Id)
            .And.ContainSingle(x => x.Id == targetComment2.Id);
    }

    /// <summary>
    /// Возвращает комментарии ADR отсортированные по дате создания
    /// </summary>
    [Fact]
    public async Task GetByAdrIdShouldReturnOrderedByCreatedAt()
    {
        // Arrange
        var targetAdrId = Guid.NewGuid();
        var baseTime = DateTimeOffset.UtcNow;
        var commentThird = entityProvider.Create<AdrComment>(x =>
        {
            x.AdrId = targetAdrId;
            x.CreatedAt = baseTime.AddMinutes(2);
        });
        var commentFirst = entityProvider.Create<AdrComment>(x =>
        {
            x.AdrId = targetAdrId;
            x.CreatedAt = baseTime;
        });
        var commentSecond = entityProvider.Create<AdrComment>(x =>
        {
            x.AdrId = targetAdrId;
            x.CreatedAt = baseTime.AddMinutes(1);
        });
        await Context.AddRangeAsync(commentThird, commentFirst, commentSecond);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrCommentReadRepository.GetByAdrIdAsync(targetAdrId, CancellationToken.None);

        // Assert
        result.Select(x => x.Id).Should()
            .NotBeEmpty()
            .And.ContainInOrder(commentFirst.Id, commentSecond.Id, commentThird.Id);
    }
}
