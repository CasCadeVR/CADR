using Ahatornn.TestGenerator;
using CADR.Administrations.Entities;
using CADR.Administrations.Entities.Enums;
using CADR.Administrations.Repositories.Contracts;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;

namespace CADR.Administrations.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="IOrganizationReadRepository"/>
/// </summary>
public class OrganizationReadRepositoryTests : CadrContextInMemory
{
    private readonly IOrganizationReadRepository organizationReadRepository;
    private readonly TestEntityProvider entityProvider;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="OrganizationReadRepositoryTests"/>
    /// </summary>
    public OrganizationReadRepositoryTests()
    {
        entityProvider = new TestEntityProviderBuilder()
            .AddPreset<Organization>(x =>
            {
                var name = $"Name{Guid.NewGuid():N}";
                x.Name = name;
                x.NameLowerCase = name.ToLower();
            })
            .AddPreset<UserOrganization>(x => x.Role = Role.Admin)
            .Build();
        organizationReadRepository = new OrganizationReadRepository(Context);
    }

    /// <summary>
    /// Возвращает null если не находит организацию по Id
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnNull()
    {
        //Arrange
        var targetId = Guid.NewGuid();
        var targetUserId = Guid.NewGuid();

        // Act
        var result = await organizationReadRepository.GetActiveByIdAsync(targetId, targetUserId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает null если не находит организацию по Id т.к. удалена
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnDeleted()
    {
        //Arrange
        var targetUserId = Guid.NewGuid();
        var targetOrganization = entityProvider.Create<Organization>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddRangeAsync(targetOrganization, entityProvider.Create<UserOrganization>(x =>
        {
            x.UserId = targetUserId;
            x.OrganizationId = targetOrganization.Id;
        }));
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationReadRepository.GetActiveByIdAsync(targetOrganization.Id, targetUserId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает организацию по Id
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnValue()
    {
        //Arrange
        var targetUserId = Guid.NewGuid();
        var targetOrganization = entityProvider.Create<Organization>();
        await Context.AddRangeAsync(targetOrganization, entityProvider.Create<UserOrganization>(x =>
        {
            x.UserId = targetUserId;
            x.OrganizationId = targetOrganization.Id;
        }));
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationReadRepository.GetActiveByIdAsync(targetOrganization.Id, targetUserId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(targetOrganization, options =>
                options.Excluding(o => o.Users));
    }

    /// <summary>
    /// Возвращает пустой список организаций по Id пользователя
    /// </summary>
    [Fact]
    public async Task GetByUserIdShouldReturnNull()
    {
        //Arrange
        var targetId = Guid.NewGuid();

        // Act
        var result = await organizationReadRepository.GetByUserIdAsync(targetId, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Возвращает список организаций по Id пользователя
    /// </summary>
    [Fact]
    public async Task GetByUserIdShouldReturnValue()
    {
        //Arrange
        var targetId = Guid.NewGuid();
        var organization1 = entityProvider.Create<Organization>();
        var organization2 = entityProvider.Create<Organization>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        var organization3 = entityProvider.Create<Organization>();
        await Context.AddRangeAsync(organization1,
            organization2,
            organization3,
            entityProvider.Create<UserOrganization>(x =>
            {
                x.UserId = targetId;
                x.OrganizationId = organization1.Id;
            }),
            entityProvider.Create<UserOrganization>(x =>
            {
                x.UserId = targetId;
                x.OrganizationId = organization2.Id;
            }),
            entityProvider.Create<UserOrganization>(x => x.OrganizationId = organization3.Id));
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationReadRepository.GetByUserIdAsync(targetId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(1)
            .And.ContainSingle(x => x.Id == organization1.Id);
    }

    /// <summary>
    /// Проверяет, что организация с указанным именем не существует
    /// </summary>
    [Fact]
    public async Task CheckIfOrganizationNameExistShouldReturnFalse()
    {
        //Arrange
        await Context.AddAsync(entityProvider.Create<Organization>());
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationReadRepository.IsActiveNameExistsAsync("CheckIfOrganizationNameExistShouldReturnFalse", CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Проверяет, что организация с указанным именем не существует т.к. удалена
    /// </summary>
    [Fact]
    public async Task CheckIfOrganizationNameExistShouldReturnFalseDeleted()
    {
        //Arrange
        var targetOrganization = entityProvider.Create<Organization>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddAsync(targetOrganization);
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationReadRepository.IsActiveNameExistsAsync(targetOrganization.Name, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Проверяет, что организация с указанным именем существует
    /// </summary>
    [Fact]
    public async Task CheckIfOrganizationNameExistShouldReturnTrue()
    {
        //Arrange
        var targetOrganization = entityProvider.Create<Organization>();
        await Context.AddAsync(targetOrganization);
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationReadRepository.IsActiveNameExistsAsync(targetOrganization.Name, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Возвращает null если не находит организацию по имени
    /// </summary>
    [Fact]
    public async Task GetActiveByNameShouldReturnNull()
    {
        //Arrange
        var targetName = $"targetName{Guid.NewGuid():N}";

        // Act
        var result = await organizationReadRepository.GetActiveByNameAsync(targetName, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает null если не находит организацию по имени т.к. удалена
    /// </summary>
    [Fact]
    public async Task GetActiveByNameShouldReturnDeleted()
    {
        //Arrange
        var targetOrganization = entityProvider.Create<Organization>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddAsync(targetOrganization);
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationReadRepository.GetActiveByNameAsync(targetOrganization.Name, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает организацию по имени
    /// </summary>
    [Fact]
    public async Task GetActiveByNameShouldReturnValue()
    {
        //Arrange
        var targetOrganization = entityProvider.Create<Organization>();
        await Context.AddAsync(targetOrganization);
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationReadRepository.GetActiveByNameAsync(targetOrganization.Name, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(targetOrganization);
    }

    /// <summary>
    /// Пользователь не является администратором организации для другого пользователя
    /// т.к. находится в другой организации
    /// </summary>
    [Fact]
    public async Task IsUserAdminForOtherOrganizationReturnFalse()
    {
        //Arrange
        var targetUserId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var targetOrganization = entityProvider.Create<Organization>();
        await Context.AddRangeAsync(targetOrganization,
            entityProvider.Create<UserOrganization>(x =>
            {
                x.UserId = targetUserId;
                x.OrganizationId = targetOrganization.Id;
                x.Role = Role.Admin;
            }),
            entityProvider.Create<UserOrganization>(x => x.UserId = otherUserId));
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationReadRepository.IsUserAdminForOtherUserOrganizationAsync(targetUserId, otherUserId, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Пользователь не является администратором организации для другого пользователя
    /// </summary>
    [Theory]
    [InlineData(Role.User)]
    [InlineData(Role.Architect)]
    public async Task IsUserAdminNotAdminReturnFalse(Role targetUserRole)
    {
        //Arrange
        var targetUserId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var targetOrganization = entityProvider.Create<Organization>();
        await Context.AddRangeAsync(targetOrganization,
            entityProvider.Create<UserOrganization>(x =>
            {
                x.UserId = targetUserId;
                x.OrganizationId = targetOrganization.Id;
                x.Role = targetUserRole;
            }),
            entityProvider.Create<UserOrganization>(x =>
            {
                x.UserId = otherUserId;
                x.OrganizationId = targetOrganization.Id;
            }));
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationReadRepository.IsUserAdminForOtherUserOrganizationAsync(targetUserId, otherUserId, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Пользователь является администратором организации для другого пользователя
    /// </summary>
    [Fact]
    public async Task IsUserAdminForOtherOrganizationReturnTrue()
    {
        //Arrange
        var targetUserId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var targetOrganization = entityProvider.Create<Organization>();
        await Context.AddRangeAsync(targetOrganization,
            entityProvider.Create<UserOrganization>(x =>
            {
                x.UserId = targetUserId;
                x.OrganizationId = targetOrganization.Id;
                x.Role = Role.Admin;
            }),
            entityProvider.Create<UserOrganization>(x =>
            {
                x.UserId = otherUserId;
                x.OrganizationId = targetOrganization.Id;
            }));
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationReadRepository.IsUserAdminForOtherUserOrganizationAsync(targetUserId, otherUserId, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Возвращает пустой список
    /// </summary>
    [Fact]
    public async Task GetActiveByCollectionIdShouldReturnEmpty()
    {
        // Act
        var result = await organizationReadRepository.GetActiveByCollectionIdAsync([], CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Возвращает действующие организации по списку Id
    /// </summary>
    [Fact]
    public async Task GetActiveByCollectionIdShouldReturnValue()
    {
        //Arrange
        var targetOrganization1 = entityProvider.Create<Organization>();
        var targetOrganization2 = entityProvider.Create<Organization>();
        var targetOrganization3 = entityProvider.Create<Organization>();
        var otherOrganization4 = entityProvider.Create<Organization>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        var guids = new List<Guid>
        {
            targetOrganization1.Id,
            targetOrganization2.Id,
            targetOrganization3.Id,
            otherOrganization4.Id
        };
        await Context.AddRangeAsync(targetOrganization1, targetOrganization2, targetOrganization3, otherOrganization4);
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationReadRepository.GetActiveByCollectionIdAsync(guids, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.HaveCount(3)
            .And.ContainSingle(x => x.Key == targetOrganization1.Id)
            .And.ContainSingle(x => x.Key == targetOrganization2.Id)
            .And.ContainSingle(x => x.Key == targetOrganization3.Id);
    }
}
