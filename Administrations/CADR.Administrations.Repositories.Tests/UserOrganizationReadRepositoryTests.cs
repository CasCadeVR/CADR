using Ahatornn.TestGenerator;
using CADR.Administrations.Entities;
using CADR.Administrations.Entities.Enums;
using CADR.Administrations.Repositories.Contracts;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;

namespace CADR.Administrations.Repositories.Tests;

/// <summary>
/// Тесты на <see cref="IUserOrganizationReadRepository"/>
/// </summary>
public class UserOrganizationReadRepositoryTests : CadrContextInMemory
{
    private readonly IUserOrganizationReadRepository userOrganizationReadRepository;
    private readonly TestEntityProvider entityProvider;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserOrganizationReadRepositoryTests"/>
    /// </summary>
    public UserOrganizationReadRepositoryTests()
    {
        entityProvider = new TestEntityProviderBuilder()
            .AddPreset<UserOrganization>(x =>
            {
                x.Role = Role.Admin;
                x.OrganizationId = Guid.NewGuid();
            })
            .Build();
        userOrganizationReadRepository = new UserOrganizationReadRepository(Context);
    }

    /// <summary>
    /// Получает <see cref="UserOrganization"/> по идентификаторам пользователя и организации
    /// </summary>
    [Fact]
    public async Task GetByUserAndOrganizationIdShouldReturnValue()
    {
        //Arrange
        var targetUserId = Guid.NewGuid();
        var targetOrganizationId = Guid.NewGuid();
        var userOrganization1 = entityProvider.Create<UserOrganization>(x =>
        {
            x.UserId = targetUserId;
            x.OrganizationId = targetOrganizationId;
        });
        await Context.AddRangeAsync(userOrganization1,
            entityProvider.Create<UserOrganization>(x =>
            {
                x.UserId = targetUserId;
                x.OrganizationId = targetOrganizationId;
                x.DeletedAt = DateTimeOffset.UtcNow;
            }),
            entityProvider.Create<UserOrganization>(x => x.UserId = targetUserId),
            entityProvider.Create<UserOrganization>(x => x.OrganizationId = targetOrganizationId));
        await Context.SaveChangesAsync();

        // Act
        var result = await userOrganizationReadRepository.GetByUserAndOrganizationIdAsync(targetUserId, targetOrganizationId,
            CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(userOrganization1);
    }

    /// <summary>
    /// Получает список <see cref="UserOrganization"/> по идентификатору организации
    /// </summary>
    [Fact]
    public async Task GetByOrganizationIdShouldReturnValue()
    {
        //Arrange
        var targetOrganizationId = Guid.NewGuid();
        var userOrganization1 = entityProvider.Create<UserOrganization>(x => x.OrganizationId = targetOrganizationId);
        await Context.AddRangeAsync(userOrganization1,
            entityProvider.Create<UserOrganization>(x =>
            {
                x.OrganizationId = targetOrganizationId;
                x.DeletedAt = DateTimeOffset.UtcNow;
            }),
            entityProvider.Create<UserOrganization>());
        await Context.SaveChangesAsync();

        // Act
        var result = await userOrganizationReadRepository.GetByOrganizationIdAsync(targetOrganizationId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(1)
            .And.ContainSingle(x => x.Id == userOrganization1.Id);
    }

    /// <summary>
    /// Получает пустой список <see cref="UserOrganization"/> по идентификаторам организаций и пользователю
    /// </summary>
    [Fact]
    public async Task GetRoleByUserAndOrganizationIdsReturnEmpty()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var organizations = Array.Empty<Guid>();

        // Act
        var result = await userOrganizationReadRepository.GetRoleByUserAndOrganizationIdsAsync(userId, organizations, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Получает список <see cref="UserOrganization"/> с одним элементом по идентификаторам организаций и пользователю
    /// </summary>
    [Fact]
    public async Task GetRoleByUserAndOrganizationIdsReturnOne()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var userOrganization1 = entityProvider.Create<UserOrganization>(x => x.UserId = userId);
        var userOrganization2 = entityProvider.Create<UserOrganization>(x =>
        {
            x.UserId = userId;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        await Context.AddRangeAsync(userOrganization1, userOrganization2);
        await Context.SaveChangesAsync();

        // Act
        var result = await userOrganizationReadRepository.GetRoleByUserAndOrganizationIdsAsync(userId, new[]
            {
                userOrganization1.OrganizationId!.Value,
                userOrganization2.OrganizationId!.Value,
            },
            CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(1)
            .And.ContainKey(userOrganization1.OrganizationId.Value);
    }

    /// <summary>
    /// Получает список <see cref="UserOrganization"/> по идентификаторам организаций и пользователю
    /// </summary>
    [Fact]
    public async Task GetRoleByUserAndOrganizationIdsReturnMany()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var userOrganization1 = entityProvider.Create<UserOrganization>(x => x.UserId = userId);
        var userOrganization2 = entityProvider.Create<UserOrganization>(x =>
        {
            x.UserId = userId;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        var userOrganization3 = entityProvider.Create<UserOrganization>(x => x.UserId = userId);
        await Context.AddRangeAsync(userOrganization1, userOrganization2, userOrganization3);
        await Context.SaveChangesAsync();

        // Act
        var result = await userOrganizationReadRepository.GetRoleByUserAndOrganizationIdsAsync(userId, new[]
            {
                userOrganization1.OrganizationId!.Value,
                userOrganization2.OrganizationId!.Value,
                userOrganization3.OrganizationId!.Value,
            },
            CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(2)
            .And.ContainKey(userOrganization1.OrganizationId.Value)
            .And.ContainKey(userOrganization3.OrganizationId.Value);
    }
}
