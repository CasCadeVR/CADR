using Ahatornn.TestGenerator;
using CADR.Administrations.Entities;
using CADR.Administrations.Repositories.Contracts;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;

namespace CADR.Administrations.Repositories.Tests;

/// <summary>
/// Тесты на <see cref="UserInviteReadRepository"/>
/// </summary>
public class UserInviteReadRepositoryTests : CadrContextInMemory
{
    private readonly IUserInviteReadRepository userInviteReadRepository;
    private readonly TestEntityProvider entityProvider;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserInviteReadRepositoryTests"/>
    /// </summary>
    public UserInviteReadRepositoryTests()
    {
        entityProvider = new TestEntityProviderBuilder()
            .AddPreset<UserInvite>(x =>
            {
                x.UserId = Guid.NewGuid();
                x.OrganizationId = Guid.NewGuid();
            })
            .Build();
        userInviteReadRepository = new UserInviteReadRepository(Context);
    }

    /// <summary>
    /// Получает действующий <see cref="UserInvite"/> по идентификатору пользователя и организации возвращает
    /// пустоту т.к. отсутствуют оба составляющих
    /// </summary>
    [Fact]
    public async Task GetActualByUserAndOrganizationIdShouldEmpty()
    {
        // Act
        var result = await userInviteReadRepository.GetActualByUserAndOrganizationIdAsync(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Получает действующий <see cref="UserInvite"/> по идентификатору пользователя и организации возвращает
    /// пустоту т.к. отсутствует пользователь
    /// </summary>
    [Fact]
    public async Task GetActualByUserAndOrganizationIdShouldEmptyByUser()
    {
        //Arrange
        var item = entityProvider.Create<UserInvite>();
        await Context.AddAsync(item);
        await Context.SaveChangesAsync();

        // Act
        var result = await userInviteReadRepository.GetActualByUserAndOrganizationIdAsync(Guid.NewGuid(), item.OrganizationId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Получает действующий <see cref="UserInvite"/> по идентификатору пользователя и организации возвращает
    /// пустоту т.к. отсутствует организация
    /// </summary>
    [Fact]
    public async Task GetActualByUserAndOrganizationIdShouldEmptyByOrganization()
    {
        //Arrange
        var item = entityProvider.Create<UserInvite>();
        await Context.AddAsync(item);
        await Context.SaveChangesAsync();

        // Act
        var result = await userInviteReadRepository.GetActualByUserAndOrganizationIdAsync(item.UserId, Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Получает действующий <see cref="UserInvite"/> по идентификатору пользователя и организации возвращает
    /// пустоту т.к. удалён
    /// </summary>
    [Fact]
    public async Task GetActualByUserAndOrganizationIdShouldEmptyByDeleted()
    {
        //Arrange
        var item = entityProvider.Create<UserInvite>(x => x.DeletedAt = DateTimeOffset.Now);
        await Context.AddAsync(item);
        await Context.SaveChangesAsync();

        // Act
        var result = await userInviteReadRepository.GetActualByUserAndOrganizationIdAsync(item.UserId, item.OrganizationId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Получает действующий <see cref="UserInvite"/> по идентификатору пользователя и организации работает
    /// </summary>
    [Fact]
    public async Task GetActualByUserAndOrganizationIdShouldWork()
    {
        //Arrange
        var item = entityProvider.Create<UserInvite>();
        await Context.AddRangeAsync(item,
            entityProvider.Create<UserInvite>(),
            entityProvider.Create<UserInvite>(x => x.UserId = item.UserId),
            entityProvider.Create<UserInvite>(x => x.OrganizationId = item.OrganizationId),
            entityProvider.Create<UserInvite>(x =>
            {
                x.UserId = item.UserId;
                x.OrganizationId = item.OrganizationId;
                x.DeletedAt = DateTimeOffset.UtcNow;
            }));
        await Context.SaveChangesAsync();

        // Act
        var result = await userInviteReadRepository.GetActualByUserAndOrganizationIdAsync(item.UserId, item.OrganizationId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(item);
    }

    /// <summary>
    /// Получает пустой список приглашений для указанной организации
    /// </summary>
    [Fact]
    public async Task GetActualWthUserByOrganizationIdShouldReturnEmpty()
    {
        //Arrange
        var targetOrganizationId = Guid.NewGuid();

        // Act
        var result = await userInviteReadRepository.GetActualWthUserByOrganizationIdAsync(targetOrganizationId, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Получает список приглашений для указанной организации
    /// </summary>
    [Fact]
    public async Task GetActualWthUserByOrganizationIdShouldReturnValue()
    {
        //Arrange
        var targetOrganizationId = Guid.NewGuid();
        var item = entityProvider.Create<UserInvite>(x => x.OrganizationId = targetOrganizationId);
        var user = entityProvider.Create<User>(x => x.Id = item.UserId);
        await Context.AddRangeAsync(item,
            user,
            entityProvider.Create<UserInvite>(x =>
            {
                x.UserId = item.UserId;
                x.OrganizationId = Guid.NewGuid();
            }),
            entityProvider.Create<UserInvite>(x => x.OrganizationId = item.OrganizationId),
            entityProvider.Create<UserInvite>(x =>
            {
                x.UserId = item.UserId;
                x.OrganizationId = item.OrganizationId;
                x.DeletedAt = DateTimeOffset.UtcNow;
            }));
        await Context.SaveChangesAsync();

        // Act
        var result = await userInviteReadRepository.GetActualWthUserByOrganizationIdAsync(targetOrganizationId, CancellationToken.None);

        // Assert
        result.Should()
            .HaveCount(1)
            .And.ContainSingle(x => x.Id == item.Id &&
                                    x.OrganizationId == targetOrganizationId &&
                                    x.UserId == user.Id);
    }

    /// <summary>
    /// Получение <see cref="UserInvite"/> по идентификатору вернуло null
    /// запись не найдена
    /// </summary>
    [Fact]
    public async Task GetActualByIdShouldReturnNull()
    {
        //Arrange
        var targetId = Guid.NewGuid();

        // Act
        var result = await userInviteReadRepository.GetActualByIdAsync(targetId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Получение <see cref="UserInvite"/> по идентификатору вернуло null
    /// запись не найдена т.к. удалена
    /// </summary>
    [Fact]
    public async Task GetActualByIdShouldReturnNullDeleted()
    {
        //Arrange
        var target = entityProvider.Create<UserInvite>(x => x.DeletedAt = DateTimeOffset.Now);
        await Context.AddAsync(target);
        await Context.SaveChangesAsync();

        // Act
        var result = await userInviteReadRepository.GetActualByIdAsync(target.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Получение <see cref="UserInvite"/> по идентификатору вернуло значение
    /// </summary>
    [Fact]
    public async Task GetActualByIdShouldReturnValue()
    {
        //Arrange
        var target = entityProvider.Create<UserInvite>();
        await Context.AddRangeAsync(target,
            entityProvider.Create<UserInvite>(),
            entityProvider.Create<UserInvite>());
        await Context.SaveChangesAsync();

        // Act
        var result = await userInviteReadRepository.GetActualByIdAsync(target.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(target);
    }

    /// <summary>
    /// Получает пустой список действующих <see cref="UserInvite" /> по
    /// идентификатору организации
    /// </summary>
    [Fact]
    public async Task GetActualByOrganizationIdShouldReturnEmpty()
    {
        //Arrange
        var targetOrganizationId = Guid.NewGuid();

        // Act
        var result = await userInviteReadRepository.GetActualByOrganizationIdAsync(targetOrganizationId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEmpty();
    }

    /// <summary>
    /// Получает список действующих <see cref="UserInvite" /> по
    /// идентификатору организации
    /// </summary>
    [Fact]
    public async Task GetActualByOrganizationIdShouldReturnValue()
    {
        //Arrange
        var targetOrganizationId = Guid.NewGuid();
        var target1 = entityProvider.Create<UserInvite>(x => x.OrganizationId = targetOrganizationId);
        var target2 = entityProvider.Create<UserInvite>(x => x.OrganizationId = targetOrganizationId);
        await Context.AddRangeAsync(target1,
            entityProvider.Create<UserInvite>(),
            entityProvider.Create<UserInvite>(x =>
            {
                x.OrganizationId = targetOrganizationId;
                x.DeletedAt = DateTimeOffset.Now;
            }),
            target2);
        await Context.SaveChangesAsync();

        // Act
        var result = await userInviteReadRepository.GetActualByOrganizationIdAsync(targetOrganizationId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == target1.Id)
            .And.ContainSingle(x => x.Id == target2.Id);
    }

    /// <summary>
    /// Получает список действующих <see cref="UserInvite" /> по
    /// идентификатору пользователя
    /// </summary>
    [Fact]
    public async Task GetActualByUserIdShouldReturnValue()
    {
        //Arrange
        var targetUserId = Guid.NewGuid();
        var targetInvite1 = entityProvider.Create<UserInvite>(x => x.UserId = targetUserId);
        var targetInvite2 = entityProvider.Create<UserInvite>(x => x.UserId = targetUserId);
        await Context.AddRangeAsync(targetInvite1,
            entityProvider.Create<UserInvite>(),
            entityProvider.Create<UserInvite>(x =>
            {
                x.UserId = targetUserId;
                x.DeletedAt = DateTimeOffset.Now;
            }),
            targetInvite2);
        await Context.SaveChangesAsync();

        // Act
        var result = await userInviteReadRepository.GetActualByUserIdAsync(targetUserId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == targetInvite1.Id)
            .And.ContainSingle(x => x.Id == targetInvite2.Id);
    }

    /// <summary>
    /// Получает пустой список действующих <see cref="UserInvite" /> по
    /// идентификатору пользователя
    /// </summary>
    [Fact]
    public async Task GetActualByUserIdShouldReturnEmpty()
    {
        //Arrange
        var targetUserId = Guid.NewGuid();

        // Act
        var result = await userInviteReadRepository.GetActualByUserIdAsync(targetUserId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEmpty();
    }
}
