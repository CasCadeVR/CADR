using Ahatornn.TestGenerator;
using AutoMapper;
using CADR.Administrations.Entities;
using CADR.Administrations.Services.AutoMappers;
using CADR.Administrations.Services.Contracts.Exceptions;
using CADR.Administrations.Services.Contracts.Interfaces;
using CADR.Administrations.Services.Contracts.Models.User;
using CADR.Administrations.Services.Tests.UnitOfWork;
using CADR.Administrations.Services.Users;
using CADR.Common.Core.Implementations;
using CADR.Context.Tests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CADR.Administrations.Services.Tests;

/// <summary>
/// Тесты для <see cref="IUserManager"/>
/// </summary>
public class UserManagerTests : CadrContextInMemory
{
    private readonly IUserManager userManager;
    private readonly TestEntityProvider entityProvider;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserManagerTests"/>
    /// </summary>
    public UserManagerTests()
    {
        entityProvider = new TestEntityProviderBuilder()
            .AddPreset<User>(x =>
            {
                var login = $"Login{Guid.NewGuid():N}";
                x.Login = login;
                x.LoginLowerCase = login.ToLower();
                x.PasswordHash = "Ca/UHnIVztnb4HEEL22BMq2Z3fwbMlCqMKyiIFfp+bg=";
                x.PasswordSalt = "/f0Ji7HVTA4iydlXzHux7Ex9KKIJGssQ0wc6vU0x23g=";
            })
            .AddPreset<LoginModel>(x =>
            {
                x.Login = "Login";
                x.Password = "Password";
            })
            .Build();
        var config = new MapperConfiguration(opts =>
        {
            opts.AddProfile(new AdministrationServiceProfile());
        });
        var unitOfWorkMock = new TestUnitOfWork(WriterContext, Context, Context);
        var mapper = config.CreateMapper();
        userManager = new UserManager(unitOfWorkMock.AdministrationUnitOfWork,
            new DateTimeProvider(),
            mapper);
    }

    /// <summary>
    /// Ошибка создания нового пользователя, логин уже существует
    /// </summary>
    [Fact]
    public async Task CreateUserShouldThrow()
    {
        //Arrange
        var user = entityProvider.Create<User>();
        await Context.AddAsync(user);
        await Context.SaveChangesAsync(CancellationToken.None);
        var model = entityProvider.Create<CreateUserModel>();
        model.Login = user.Login;

        // Act
        Func<Task> act = () => userManager.CreateUserAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationInvalidOperationException>();
    }

    /// <summary>
    /// Создаёт нового пользователя без ошибок
    /// </summary>
    [Fact]
    public async Task CreateUserShouldWork()
    {
        //Arrange
        var model = entityProvider.Create<CreateUserModel>();

        // Act
        Func<Task> act = () => userManager.CreateUserAsync(model, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        var user = await Context.Set<User>().FirstOrDefaultAsync();
        user.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new
            {
                model.Name,
                model.Email,
                model.Login,
                LoginLowerCase = model.Login.ToLower(),
            });
    }

    /// <summary>
    /// Логин с ошибкой: пользователь не найден
    /// </summary>
    [Fact]
    public Task LoginShouldThrowByNotFound()
    {
        // Act
        Func<Task> act = () => userManager.GetActiveByLoginAndPasswordAsync(entityProvider.Create<LoginModel>(), CancellationToken.None);

        // Assert
        return act.Should().ThrowAsync<AdministrationNotFoundException>();
    }

    /// <summary>
    /// Логин с ошибкой: пользователь заблокирован без даты
    /// </summary>
    [Fact]
    public async Task LoginShouldThrowByBlocked()
    {
        //Arrange
        var user = entityProvider.Create<User>(x => x.Blocked = true);
        await Context.AddAsync(user, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        Func<Task> act = () => userManager.GetActiveByLoginAndPasswordAsync(entityProvider.Create<LoginModel>(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationNotFoundException>();
    }

    /// <summary>
    /// Логин с ошибкой: пользователь заблокирован с указанием даты
    /// </summary>
    [Fact]
    public async Task LoginShouldThrowByBlockedWithDate()
    {
        //Arrange
        var user = entityProvider.Create<User>(x =>
        {
            x.Blocked = true;
            x.BlockedAt = DateTimeOffset.UtcNow.AddDays(1);
        });
        await Context.AddAsync(user, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        Func<Task> act = () => userManager.GetActiveByLoginAndPasswordAsync(entityProvider.Create<LoginModel>(),
            CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationNotFoundException>();
    }

    /// <summary>
    /// Логин удался
    /// </summary>
    [Fact]
    public async Task LoginShouldWork()
    {
        //Arrange
        var user = entityProvider.Create<User>();
        await Context.AddAsync(user, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await userManager.GetActiveByLoginAndPasswordAsync(entityProvider.Create<LoginModel>(x => x.Login = user.Login),
            CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new
            {
                user.Id,
                user.Name,
                user.SecurityStamp,
                user.Login
            });
    }

    /// <summary>
    /// Логин удался если блокировка кончилась
    /// </summary>
    [Fact]
    public async Task LoginShouldBlockedWork()
    {
        //Arrange
        var user = entityProvider.Create<User>(x =>
        {
            x.Blocked = true;
            x.BlockedAt = DateTimeOffset.UtcNow.AddDays(-1);
        });
        await Context.AddAsync(user, CancellationToken.None);
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await userManager.GetActiveByLoginAndPasswordAsync(entityProvider.Create<LoginModel>(x => x.Login = user.Login),
            CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new
            {
                user.Id,
                user.Name,
                user.SecurityStamp,
                user.Login
            });
        var entity = await Context.Set<User>().SingleOrDefaultAsync(x => x.Id == user.Id);
        entity.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new
            {
                Blocked = false,
                BlockedAt = (DateTimeOffset?)null,
            });
    }
}
