using Ahatornn.TestGenerator;
using CADR.Administrations.Entities;
using CADR.Administrations.Entities.Enums;
using CADR.Administrations.Repositories.Contracts;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;

namespace CADR.Administrations.Repositories.Tests;

/// <summary>
/// Тесты на <see cref="IUserReadRepository"/>
/// </summary>
public class UserReadRepositoryTests : CadrContextInMemory
{
    private readonly IUserReadRepository userReadRepository;
    private readonly TestEntityProvider entityProvider;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserReadRepositoryTests"/>
    /// </summary>
    public UserReadRepositoryTests()
    {
        entityProvider = new TestEntityProviderBuilder()
            .AddPreset<User>(x =>
            {
                var email = $"Email{Guid.NewGuid():N}";
                var login = $"Login{Guid.NewGuid():N}";
                x.Email = email;
                x.EmailLowerCase = email.ToLower();
                x.EmailConfirmed = true;
                x.Login = login;
                x.LoginLowerCase = login.ToLower();
            })
            .Build();
        userReadRepository = new UserReadRepository(Context);
    }

    /// <summary>
    /// Возвращает null если не находит пользователя по Id
    /// </summary>
    [Fact]
    public async Task GetByIdShouldReturnNull()
    {
        //Arrange
        var targetId = Guid.NewGuid();

        // Act
        var result = await userReadRepository.GetByIdAsync(targetId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает пользователя по Id
    /// </summary>
    [Fact]
    public async Task GetByIdShouldReturnValue()
    {
        //Arrange
        var targetUser = entityProvider.Create<User>();
        await Context.AddAsync(targetUser, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await userReadRepository.GetByIdAsync(targetUser.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(targetUser);
    }

    /// <summary>
    /// Возвращает данные при запросе существующего логина
    /// </summary>
    [Theory]
    [MemberData(nameof(IsActiveExistsData))]
    public async Task IsActiveLoginExistsShouldReturnValue(DateTimeOffset? deletedAt, bool expected)
    {
        //Arrange
        var targetUser = entityProvider.Create<User>(x => x.DeletedAt = deletedAt);
        await Context.AddAsync(targetUser, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await userReadRepository.IsActiveLoginExistsAsync(targetUser.Login, CancellationToken.None);

        // Assert
        result.Should().Be(expected);
    }

    /// <summary>
    /// Возвращает данные при запросе существующего логина
    /// </summary>
    [Theory]
    [MemberData(nameof(IsActiveExistsData))]
    public async Task IsActiveEmailExistsShouldReturnValue(DateTimeOffset? deletedAt, bool expected)
    {
        //Arrange
        var targetUser = entityProvider.Create<User>(x => x.DeletedAt = deletedAt);
        await Context.AddAsync(targetUser, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await userReadRepository.IsActiveEmailExistsAsync(targetUser.Email, CancellationToken.None);

        // Assert
        result.Should().Be(expected);
    }

    /// <summary>
    /// Возвращает null если не находит активного пользователя по Id
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnNull()
    {
        //Arrange
        var targetId = Guid.NewGuid();

        // Act
        var result = await userReadRepository.GetActiveByIdAsync(targetId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает null если не находит активного пользователя по Id
    /// т.к. пользователь удалён
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnDeletedNull()
    {
        //Arrange
        var targetUser = entityProvider.Create<User>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddAsync(targetUser, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await userReadRepository.GetActiveByIdAsync(targetUser.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает null если не находит активного пользователя по Id
    /// т.к. пользователь заблокирован
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnBlockedNull()
    {
        //Arrange
        var targetUser = entityProvider.Create<User>(x => x.Blocked = true);
        await Context.AddAsync(targetUser, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await userReadRepository.GetActiveByIdAsync(targetUser.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает активного пользователя по Id
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnValue()
    {
        //Arrange
        var targetUser = entityProvider.Create<User>();
        await Context.AddAsync(targetUser, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await userReadRepository.GetActiveByIdAsync(targetUser.Id, CancellationToken.None);

        // Assert
        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(targetUser);
    }

    /// <summary>
    /// Возвращает null если не находит пользователя по email
    /// </summary>
    [Fact]
    public async Task GetActiveByMailShouldReturnNull()
    {
        //Arrange
        var targetMail = "GetActiveByMailShouldReturnNull_mail";

        // Act
        var result = await userReadRepository.GetActiveByEmailAsync(targetMail, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает пользователя по email
    /// </summary>
    [Fact]
    public async Task GetActiveByMailShouldReturnValue()
    {
        //Arrange
        var targetUser = entityProvider.Create<User>();
        await Context.AddAsync(targetUser, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await userReadRepository.GetActiveByEmailAsync(targetUser.Email, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(targetUser);
    }

    /// <summary>
    /// Список пользователей организации пуст
    /// </summary>
    [Fact]
    public async Task GetShouldReturnEmpty()
    {
        // Act
        var result = await userReadRepository.GetByOrganizationIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Список пользователей содержит значения
    /// </summary>
    [Fact]
    public async Task GetShouldReturnValue()
    {
        //Arrange
        var organizationId = Guid.NewGuid();

        var targetUser1 = entityProvider.Create<User>();
        await Context.AddRangeAsync(targetUser1,
            entityProvider.Create<UserOrganization>(x =>
            {
                x.OrganizationId = organizationId;
                x.UserId = targetUser1.Id;
            }));
        var targetUser2 = entityProvider.Create<User>();
        await Context.AddRangeAsync(targetUser2,
            entityProvider.Create<UserOrganization>(x =>
            {
                x.OrganizationId = Guid.NewGuid();
                x.UserId = targetUser2.Id;
            }));
        var targetUser3 = entityProvider.Create<User>();
        await Context.AddRangeAsync(targetUser3,
            entityProvider.Create<UserOrganization>(x =>
            {
                x.OrganizationId = organizationId;
                x.UserId = targetUser3.Id;
            }));

        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await userReadRepository.GetByOrganizationIdAsync(organizationId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And
            .HaveCount(2)
            .And
            .ContainSingle(x => x.Id == targetUser1.Id)
            .And
            .ContainSingle(x => x.Id == targetUser3.Id);
    }

    /// <summary>
    /// Репозиторий должен возвращать false на запрос, является ли пользователь админом в организации, если пользователя нет в организации
    /// </summary>
    [Fact]
    public async Task IsAdminInOrganizationShouldReturnFalseForNonOrganizationMember()
    {
        // Arrange
        var organization = entityProvider.Create<Organization>();
        Context.Add(organization);
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await userReadRepository.IsAdminInOrganizationAsync(Guid.NewGuid(), organization.Id, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Репозиторий должен узнавать админа, если он админ в организации
    /// </summary>
    [Fact]
    public async Task IsAdminInOrganizationShouldReturnTrueForAdminInOrganization()
    {
        // Arrange
        var organization = entityProvider.Create<Organization>();
        var userOrganization = entityProvider.Create<UserOrganization>(x =>
        {
            x.OrganizationId = organization.Id;
            x.Role = Role.Admin;
        });
        Context.AddRange(organization, userOrganization);
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await userReadRepository.IsAdminInOrganizationAsync(userOrganization.UserId, organization.Id, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Репозиторий должен возвращать false на запрос, если пользователь не админ в организации
    /// </summary>
    [Fact]
    public async Task IsAdminInTeamOrOrganizationShouldReturnFalse()
    {
        // Arrange
        var organization = entityProvider.Create<Organization>();
        var userOrganization = entityProvider.Create<UserOrganization>(x =>
        {
            x.OrganizationId = organization.Id;
            x.Role = Role.User;
        });
        Context.AddRange(organization, userOrganization);
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await userReadRepository.IsAdminInOrganizationAsync(userOrganization.UserId, organization.Id, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Данные для проверки поиска в БД
    /// </summary>
    public static IEnumerable<object?[]> IsActiveExistsData()
    {
        yield return new object?[] { DateTimeOffset.MinValue, false };
        yield return new object?[] { null, true };
    }
}
