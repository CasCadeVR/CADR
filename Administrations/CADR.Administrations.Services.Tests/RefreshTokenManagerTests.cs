using Ahatornn.TestGenerator;
using CADR.Administrations.Entities;
using CADR.Administrations.Repositories;
using CADR.Administrations.Services.Contracts.Exceptions;
using CADR.Administrations.Services.Contracts.Interfaces;
using CADR.Administrations.Services.Contracts.Models.Token;
using CADR.Administrations.Services.Tests.UnitOfWork;
using CADR.Administrations.Services.Users;
using CADR.Common.Core.Implementations;
using CADR.Context.Tests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CADR.Administrations.Services.Tests;

/// <summary>
/// Тесты для <see cref="IRefreshTokenManager"/>
/// </summary>
public class RefreshTokenManagerTests : CadrContextInMemory
{
    private readonly IRefreshTokenManager refreshTokenManager;
    private readonly TestEntityProvider entityProvider;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="RefreshTokenManagerTests"/>
    /// </summary>
    public RefreshTokenManagerTests()
    {
        entityProvider = new TestEntityProviderBuilder()
            .AddPreset<RefreshToken>(x => x.Expires = DateTimeOffset.MaxValue)
            .Build();
        var unitOfWorkMock = new TestUnitOfWork(WriterContext, Context, Context);

        refreshTokenManager = new RefreshTokenManager(unitOfWorkMock.AdministrationUnitOfWork,
            new DateTimeProvider(),
            new UserReadRepository(Context));
    }

    /// <summary>
    /// Создаёт токен обновления
    /// </summary>
    [Fact]
    public async Task CreateRefreshTokenAsyncShouldWork()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var targetToken = entityProvider.Create<RefreshToken>(x => x.UserId = userId);
        await Context.AddAsync(targetToken, CancellationToken.None);
        await UnitOfWork.SaveChangesAsync();
        var model = entityProvider.Create<CreateRefreshTokenModel>(x =>
        {
            x.UserId = userId;
            x.ExpiredDays = 10;
        });

        // Act
        var result = await refreshTokenManager.CreateRefreshTokenAsync(model, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        var oldToken = await Context.Set<RefreshToken>().FirstOrDefaultAsync(x => x.Id == targetToken.Id, CancellationToken.None);
        oldToken?.DeletedAt.Should().NotBeNull();
        var newToken = await Context.Set<RefreshToken>().FirstOrDefaultAsync(x => x.Id == result, CancellationToken.None);
        newToken.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new
            {
                UserId = userId,
                model.AccessPayload,
                model.SecurityStamp,
                DeletedAt = (DateTimeOffset?)null,
            });
    }

    /// <summary>
    /// Обновление токена падает с ошибкой: токен не найден
    /// </summary>
    [Fact]
    public async Task UpdateRefreshTokenShouldThrowNotFound()
    {
        //Arrange
        var targetToken = entityProvider.Create<RefreshToken>(x =>
        {
            x.UserId = Guid.NewGuid();
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        await Context.AddAsync(targetToken, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        Func<Task> act = () => refreshTokenManager.UpdateRefreshTokenAsync(targetToken.Id, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<AdministrationEntityNotFoundException<RefreshToken>>()
            .Where(x => x.Message.Contains(targetToken.Id.ToString()));
    }

    /// <summary>
    /// Обновление токена падает с ошибкой: срок действия токена окончен
    /// </summary>
    [Fact]
    public async Task UpdateRefreshTokenShouldThrowExpires()
    {
        //Arrange
        var targetToken = entityProvider.Create<RefreshToken>(x =>
        {
            x.UserId = Guid.NewGuid();
            x.Expires = DateTimeOffset.UtcNow.AddDays(-1);
        });
        await Context.AddAsync(targetToken, CancellationToken.None);
        await UnitOfWork.SaveChangesAsync();

        // Act
        Func<Task> act = () => refreshTokenManager.UpdateRefreshTokenAsync(targetToken.Id, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<AdministrationInvalidOperationException>()
            .Where(x => x.Message.Contains("окончен", StringComparison.InvariantCultureIgnoreCase));
    }

    /// <summary>
    /// Обновление токена падает с ошибкой: невалидный SecurityStamp
    /// </summary>
    [Fact]
    public async Task UpdateRefreshTokenShouldThrowSecurityStamp()
    {
        //Arrange
        var user = entityProvider.Create<Entities.User>();
        await Context.AddAsync(user, CancellationToken.None);
        var targetToken = entityProvider.Create<RefreshToken>(x => x.UserId = user.Id);
        ;
        await Context.AddAsync(targetToken, CancellationToken.None);
        await UnitOfWork.SaveChangesAsync();

        // Act
        Func<Task> act = () => refreshTokenManager.UpdateRefreshTokenAsync(targetToken.Id, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<AdministrationInvalidOperationException>()
            .Where(x => x.Message.Contains("Невалидный", StringComparison.InvariantCultureIgnoreCase));
    }

    /// <summary>
    /// Обновление токена работает
    /// </summary>
    [Fact]
    public async Task UpdateRefreshTokenShouldWork()
    {
        //Arrange
        var user = entityProvider.Create<Entities.User>();
        await Context.AddAsync(user, CancellationToken.None);
        var targetToken = entityProvider.Create<RefreshToken>(x =>
        {
            x.UserId = user.Id;
            x.SecurityStamp = user.SecurityStamp;
            x.Expires = DateTimeOffset.UtcNow.AddDays(5);
        });
        await Context.AddAsync(targetToken, CancellationToken.None);
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await refreshTokenManager.UpdateRefreshTokenAsync(targetToken.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new
            {
                ClaimPayload = targetToken.AccessPayload,
            });
        var oldToken = await Context.Set<RefreshToken>().FirstOrDefaultAsync(x => x.Id == targetToken.Id, CancellationToken.None);
        oldToken?.DeletedAt.Should().NotBeNull();
        var newToken = await Context.Set<RefreshToken>().FirstOrDefaultAsync(x => x.Id == result.TokenId, CancellationToken.None);
        newToken.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new
            {
                UserId = user.Id,
                targetToken.AccessPayload,
                targetToken.SecurityStamp,
                DeletedAt = (DateTimeOffset?)null,
            });
    }

    /// <summary>
    /// Удаление токена не падает, даже если токен не найден
    /// </summary>
    [Fact]
    public Task DeleteRefreshTokenByUserIdShouldNotThrow()
    {
        //Arrange
        var id = Guid.NewGuid();

        // Act
        Func<Task> act = () => refreshTokenManager.DeleteRefreshTokenByUserIdAsync(id, CancellationToken.None);

        // Assert
        return act.Should().NotThrowAsync();
    }

    /// <summary>
    /// Удаление токена работает
    /// </summary>
    [Fact]
    public async Task DeleteRefreshTokenByUserIdShouldWork()
    {
        //Arrange
        var targetToken = entityProvider.Create<RefreshToken>(x => x.UserId = Guid.NewGuid());
        await Context.AddAsync(targetToken, CancellationToken.None);
        await UnitOfWork.SaveChangesAsync();

        // Act
        Func<Task> act = () => refreshTokenManager.DeleteRefreshTokenByUserIdAsync(targetToken.UserId, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        var oldToken = await Context.Set<RefreshToken>().FirstOrDefaultAsync(x => x.Id == targetToken.Id, CancellationToken.None);
        oldToken?.DeletedAt.Should().NotBeNull();
    }
}
