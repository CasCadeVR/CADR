using Ahatornn.TestGenerator;
using AutoMapper;
using CADR.Administrations.Entities;
using CADR.Administrations.Entities.Enums;
using CADR.Administrations.Services.AutoMappers;
using CADR.Administrations.Services.Contracts.Exceptions;
using CADR.Administrations.Services.Contracts.Interfaces;
using CADR.Administrations.Services.Contracts.Models.Enums;
using CADR.Administrations.Services.Contracts.Models.Organizations;
using CADR.Administrations.Services.Organizations;
using CADR.Administrations.Services.Tests.UnitOfWork;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;

namespace CADR.Administrations.Services.Tests;

/// <summary>
/// Тесты на <see cref="IOrganizationManager"/>
/// </summary>
public class OrganizationManagerTests : CadrContextInMemory
{
    private readonly IOrganizationManager organizationManager;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="OrganizationManagerTests"/>
    /// </summary>
    public OrganizationManagerTests()
    {
        var config = new MapperConfiguration(opts =>
        {
            opts.AddProfile(new AdministrationServiceProfile());
        });
        var unitOfWorkMock = new TestUnitOfWork(WriterContext, Context, Context);

        organizationManager = new OrganizationManager(unitOfWorkMock.AdministrationUnitOfWork, config.CreateMapper());
    }

    /// <summary>
    /// Добавление организации работает
    /// </summary>
    [Fact]
    public async Task CreateShouldWork()
    {
        //Arrange
        var model = TestEntityProvider.Shared.Create<CreateOrganizationModel>();

        // Act
        var result = await organizationManager.CreateAsync(model, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { model.Name, model.Description, });
        var userOrganization = Context.Set<UserOrganization>().FirstOrDefault(x => x.OrganizationId == result.Id);
        userOrganization.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { model.UserId, Role = Role.Admin, });
    }

    /// <summary>
    /// Получает список организаций для указанного идентификатора пользователя
    /// </summary>
    [Fact]
    public async Task GetByUserIdShouldReturnValue()
    {
        //Arrange
        var targetUserId = Guid.NewGuid();
        var organization1 = TestEntityProvider.Shared.Create<Organization>();
        var organization2 = TestEntityProvider.Shared.Create<Organization>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        var organization3 = TestEntityProvider.Shared.Create<Organization>();
        var organization4 = TestEntityProvider.Shared.Create<Organization>();
        await Context.AddRangeAsync(organization1, organization2, organization3, organization4);
        await Context.AddRangeAsync(TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = targetUserId;
                x.OrganizationId = organization1.Id;
                x.Role = Role.Admin;
            }),
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = targetUserId;
                x.OrganizationId = organization2.Id;
            }),
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = targetUserId;
                x.OrganizationId = organization3.Id;
                x.DeletedAt = DateTimeOffset.UtcNow;
            }),
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = targetUserId;
                x.OrganizationId = organization4.Id;
                x.Role = Role.User;
            }));

        await Context.SaveChangesAsync();

        // Act
        var result = await organizationManager.GetByUserIdAsync(targetUserId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == organization1.Id &&
                                    x.UserIsAdmin)
            .And.ContainSingle(x => x.Id == organization4.Id &&
                                    !x.UserIsAdmin);
    }

    /// <summary>
    /// Редактирование организации выдаёт ошибку: организация не найдена
    /// </summary>
    [Fact]
    public Task UpdateShouldThrowNotFound()
    {
        //Arrange
        var model = TestEntityProvider.Shared.Create<UpdateOrganizationModel>();

        // Act
        Func<Task> act = () => organizationManager.UpdateAsync(model, CancellationToken.None);

        // Assert
        return act.Should()
            .ThrowAsync<AdministrationEntityNotFoundException<Organization>>()
            .WithMessage($"*{model.Id}*");
    }

    /// <summary>
    /// Редактирование организации выдаёт ошибку: не хватает прав
    /// </summary>
    [Theory]
    [InlineData(Role.User)]
    [InlineData(Role.Architect)]
    public async Task UpdateShouldThrowDeny(Role targetRole)
    {
        //Arrange
        var model = TestEntityProvider.Shared.Create<UpdateOrganizationModel>();
        var organization1 = TestEntityProvider.Shared.Create<Organization>(x => x.Id = model.Id);
        await Context.AddRangeAsync(organization1, TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.UserId = model.UserId;
            x.OrganizationId = organization1.Id;
            x.Role = targetRole;
        }));
        await Context.SaveChangesAsync();

        // Act
        Func<Task> act = () => organizationManager.UpdateAsync(model, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<AdministrationAccessException>()
            .WithMessage($"*прав*");
    }

    /// <summary>
    /// Редактирование организации работает
    /// </summary>
    [Fact]
    public async Task UpdateShouldWork()
    {
        //Arrange
        var model = TestEntityProvider.Shared.Create<UpdateOrganizationModel>();
        var organization1 = TestEntityProvider.Shared.Create<Organization>(x => x.Id = model.Id);
        await Context.AddRangeAsync(organization1, TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.UserId = model.UserId;
            x.OrganizationId = organization1.Id;
            x.Role = Role.Admin;
        }));
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await organizationManager.UpdateAsync(model, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { model.Name, model.Description, });
    }

    /// <summary>
    /// Удаление организации выдаёт ошибку: не хватает прав
    /// </summary>
    [Theory]
    [InlineData(Role.User)]
    [InlineData(Role.Architect)]
    public async Task DeleteShouldThrowDeny(Role targetRole)
    {
        //Arrange
        var model = new DeleteOrganizationModel { UserId = Guid.NewGuid(), OrganizationId = Guid.NewGuid(), };
        var organization1 = TestEntityProvider.Shared.Create<Organization>(x => x.Id = model.OrganizationId);
        var targetUser = TestEntityProvider.Shared.Create<User>(x => x.Id = model.UserId);
        var targetUserOrganization = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.UserId = model.UserId;
            x.OrganizationId = organization1.Id;
            x.Role = targetRole;
        });
        await Context.AddRangeAsync(organization1,
            targetUser,
            targetUserOrganization);
        await Context.SaveChangesAsync();

        // Act
        Func<Task> act = () => organizationManager.DeleteAsync(model, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<AdministrationAccessException>()
            .WithMessage($"*прав*");
    }

    /// <summary>
    /// Удаление организации работает
    /// </summary>
    [Fact]
    public async Task DeleteShouldWork()
    {
        //Arrange
        var model = new DeleteOrganizationModel { UserId = Guid.NewGuid(), OrganizationId = Guid.NewGuid(), };
        var organization1 = TestEntityProvider.Shared.Create<Organization>();
        organization1.Id = model.OrganizationId;
        var targetUser = TestEntityProvider.Shared.Create<User>(x => x.Id = model.UserId);
        var targetUserOrganization = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.UserId = model.UserId;
            x.OrganizationId = organization1.Id;
            x.Role = Role.Admin;
        });

        await Context.AddRangeAsync(organization1,
            targetUser,
            targetUserOrganization);
        await UnitOfWork.SaveChangesAsync();

        // Act
        Func<Task> act = () => organizationManager.DeleteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        var entityOrganization = Context.Set<Organization>().First(x => x.Id == organization1.Id);
        entityOrganization.DeletedAt.Should().NotBeNull();
        var entityUserOrganization = Context.Set<UserOrganization>().First(x => x.Id == targetUserOrganization.Id);
        entityUserOrganization.DeletedAt.Should().NotBeNull();
        var entityUser = Context.Set<User>().First(x => x.Id == targetUser.Id);
        entityUser.DeletedAt.Should().BeNull();
    }

    /// <summary>
    /// Получает организацию по идентификатору
    /// </summary>
    [Fact]
    public async Task GetByIdShouldReturnValue()
    {
        //Arrange
        var targetUserId = Guid.NewGuid();
        var organization1 = TestEntityProvider.Shared.Create<Organization>();
        var organization2 = TestEntityProvider.Shared.Create<Organization>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        var organization3 = TestEntityProvider.Shared.Create<Organization>();
        var organization4 = TestEntityProvider.Shared.Create<Organization>();
        await Context.AddRangeAsync(organization1, organization2, organization3, organization4);
        await Context.AddRangeAsync(TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = targetUserId;
                x.OrganizationId = organization1.Id;
            }),
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = targetUserId;
                x.OrganizationId = organization2.Id;
            }),
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = targetUserId;
                x.OrganizationId = organization3.Id;
                x.DeletedAt = DateTimeOffset.UtcNow;
            }),
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = targetUserId;
                x.OrganizationId = organization4.Id;
            }));
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationManager.GetByIdAsync(organization1.Id, targetUserId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { organization1.Id, organization1.Name, organization1.Description, });
    }

    /// <summary>
    /// Получает организацию по идентификатору, но организация не найдена
    /// </summary>
    [Fact]
    public Task GetByIdShouldThrowNotFound()
    {
        //Arrange
        var targetId = Guid.NewGuid();

        // Act
        Func<Task> act = () => organizationManager.GetByIdAsync(targetId, Guid.NewGuid(), CancellationToken.None);

        // Assert
        return act.Should().ThrowAsync<AdministrationEntityNotFoundException<Organization>>().WithMessage($"*{targetId}*");
    }

    /// <summary>
    /// Получает список пользователей организации по идентификатору
    /// </summary>
    [Fact]
    public async Task GetUsersByOrganizationIdAsyncShouldReturnValue()
    {
        //Arrange
        var targetOrganization = TestEntityProvider.Shared.Create<Organization>();
        var user1 = TestEntityProvider.Shared.Create<User>();
        var user2 = TestEntityProvider.Shared.Create<User>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        var user3 = TestEntityProvider.Shared.Create<User>();
        var user4 = TestEntityProvider.Shared.Create<User>();
        await Context.AddRangeAsync(targetOrganization, user1, user2, user3, user4);
        await Context.AddRangeAsync(TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = user1.Id;
                x.OrganizationId = targetOrganization.Id;
            }),
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = user2.Id;
                x.OrganizationId = targetOrganization.Id;
            }),
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = user3.Id;
                x.OrganizationId = targetOrganization.Id;
                x.DeletedAt = DateTimeOffset.UtcNow;
            }),
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = user4.Id;
                x.OrganizationId = targetOrganization.Id;
            }));
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationManager.GetUsersByOrganizationIdAsync(targetOrganization.Id, user1.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == user1.Id)
            .And.ContainSingle(x => x.Id == user4.Id);
    }


    /// <summary>
    /// Удаление пользователя не работает; пользователь, выполнивший запрос, не принадлежит указанной организации
    /// </summary>
    [Fact]
    public async Task DeleteUserShouldThrowByOrganization()
    {
        //Arrange
        var item = TestEntityProvider.Shared.Create<UserOrganization>(x => x.OrganizationId = Guid.NewGuid());
        await Context.AddAsync(item);
        await Context.SaveChangesAsync();
        var model = TestEntityProvider.Shared
            .Create<DeleteUserOrganizationModel>(x => x.UserId = item.UserId);

        // Act
        Func<Task> act = () => organizationManager.DeleteUserAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationAccessException>();
    }

    /// <summary>
    /// Удаление пользователя не работает, пользователь организации не админ
    /// </summary>
    [Theory]
    [InlineData(Role.User)]
    [InlineData(Role.Architect)]
    public async Task DeleteUserShouldThrowByRole(Role role)
    {
        //Arrange
        var item = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = Guid.NewGuid();
            x.Role = role;
        });
        await Context.AddAsync(item);
        await Context.SaveChangesAsync();
        var model = TestEntityProvider.Shared
            .Create<DeleteUserOrganizationModel>(x =>
            {
                x.UserId = item.UserId;
                x.OrganizationId = item.OrganizationId!.Value;
            });

        // Act
        Func<Task> act = () => organizationManager.DeleteUserAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationAccessException>();
    }

    /// <summary>
    /// Удаление пользователя не работает, пользователь не найден
    /// </summary>
    [Fact]
    public async Task DeleteUserShouldThrowByNotFound()
    {
        //Arrange
        var item = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = Guid.NewGuid();
            x.Role = Role.Admin;
        });
        await Context.AddAsync(item);
        await Context.SaveChangesAsync();
        var model = TestEntityProvider.Shared
            .Create<DeleteUserOrganizationModel>(x =>
            {
                x.UserId = item.UserId;
                x.OrganizationId = item.OrganizationId!.Value;
            });

        // Act
        Func<Task> act = () => organizationManager.DeleteUserAsync(model, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<AdministrationEntityNotFoundException<User>>()
            .WithMessage($"*{model.UserToDeleteId}*");
    }

    /// <summary>
    /// Удаление пользователя работает
    /// </summary>
    [Fact]
    public async Task DeleteUserShouldWork()
    {
        //Arrange
        var item = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = Guid.NewGuid();
            x.Role = Role.Admin;
        });
        var targetUserOrganization = TestEntityProvider.Shared.Create<UserOrganization>(x => x.OrganizationId = item.OrganizationId!.Value);

        await Context.AddRangeAsync(item, targetUserOrganization);
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared
            .Create<DeleteUserOrganizationModel>(x =>
            {
                x.UserId = item.UserId;
                x.OrganizationId = item.OrganizationId!.Value;
                x.UserToDeleteId = targetUserOrganization.UserId;
            });

        // Act
        Func<Task> act = () => organizationManager.DeleteUserAsync(model, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();

        var deletedItem = Context.Set<UserOrganization>().FirstOrDefault(x => x.Id == targetUserOrganization.Id);
        deletedItem.Should().NotBeNull();
        deletedItem!.DeletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Изменение роли пользователя не работает, пользователи в разных организациях
    /// </summary>
    [Fact]
    public async Task ChangeUserRoleShouldThrowByOrganization()
    {
        //Arrange
        var item = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = Guid.NewGuid();
            x.Role = Role.Admin;
        });
        var targetUserOrganization = TestEntityProvider.Shared.Create<UserOrganization>();
        await Context.AddRangeAsync(item, targetUserOrganization);
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared
            .Create<ChangeUserRoleModel>(x =>
            {
                x.OrganizationId = item.OrganizationId!.Value; //Организация пользователя, выполнившего запрос
                x.UserId = item.UserId;
                x.UserToUpdateId = targetUserOrganization.UserId;
            });

        // Act
        Func<Task> act = () => organizationManager.ChangeUserRoleAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationEntityNotFoundException<User>>()
            .WithMessage($"*{model.UserToUpdateId}*");
    }

    /// <summary>
    /// Изменение роли пользователя не работает, пользователи в разных организациях
    /// </summary>
    [Fact]
    public async Task ChangeUserRoleShouldThrowByTargetOrganization()
    {
        //Arrange
        var item = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = Guid.NewGuid();
            x.Role = Role.Admin;
        });
        var targetUserOrganization = TestEntityProvider.Shared.Create<UserOrganization>(x => x.OrganizationId = Guid.NewGuid());
        await Context.AddRangeAsync(item, targetUserOrganization);
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared
            .Create<ChangeUserRoleModel>(x =>
            {
                x.OrganizationId = targetUserOrganization.OrganizationId!.Value; //Организация пользователя, которому надо поменять роль
                x.UserId = item.UserId;
                x.UserToUpdateId = targetUserOrganization.UserId;
            });

        // Act
        Func<Task> act = () => organizationManager.ChangeUserRoleAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationAccessException>();
    }

    /// <summary>
    /// Изменение роли пользователя не работает, пользователь, выполнивший запрос, не админ
    /// </summary>
    [Theory]
    [InlineData(Role.User)]
    [InlineData(Role.Architect)]
    public async Task ChangeUserRoleShouldThrowByRole(Role role)
    {
        //Arrange
        var item = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = Guid.NewGuid();
            x.Role = role;
        });
        var targetUserOrganization = TestEntityProvider.Shared.Create<UserOrganization>(x => x.OrganizationId = item.OrganizationId!.Value);
        await Context.AddRangeAsync(item, targetUserOrganization);
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared
            .Create<ChangeUserRoleModel>(x =>
            {
                x.OrganizationId = item.OrganizationId!.Value;
                x.UserId = item.UserId;
                x.UserToUpdateId = targetUserOrganization.UserId;
            });

        // Act
        Func<Task> act = () => organizationManager.ChangeUserRoleAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationAccessException>();
    }

    /// <summary>
    /// Изменение роли пользователя не работает, пользователь, которому нужно изменить роль, не найден
    /// </summary>
    [Fact]
    public async Task ChangeUserRoleShouldThrowByNotFound()
    {
        //Arrange
        var item = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = Guid.NewGuid();
            x.Role = Role.Admin;
        });
        await Context.AddRangeAsync(item);
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared
            .Create<ChangeUserRoleModel>(x =>
            {
                x.OrganizationId = item.OrganizationId!.Value;
                x.UserId = item.UserId;
            });

        // Act
        Func<Task> act = () => organizationManager.ChangeUserRoleAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationEntityNotFoundException<User>>()
            .WithMessage($"*{model.UserToUpdateId}*");
    }

    /// <summary>
    /// Изменение роли пользователя работает
    /// </summary>
    [Theory]
    [InlineData(UserRole.User, Role.User)]
    [InlineData(UserRole.Architect, Role.Architect)]
    [InlineData(UserRole.Admin, Role.Admin)]
    public async Task ChangeUserRoleShouldWork(UserRole userRole, Role role)
    {
        //Arrange
        var item = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = Guid.NewGuid();
            x.Role = Role.Admin;
        });
        var roleToArrange = role == Role.User ? Role.Architect : Role.User;
        var targetUserOrganization = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = item.OrganizationId!.Value;
            x.Role = roleToArrange;
        });
        var notTargetUserOrganization = TestEntityProvider.Shared.Create<UserOrganization>(x => x.OrganizationId = item.OrganizationId!.Value);
        var notTargetUserOrganizationRole = notTargetUserOrganization.Role;
        await Context.AddRangeAsync(item, targetUserOrganization, notTargetUserOrganization);
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared
            .Create<ChangeUserRoleModel>(x =>
            {
                x.UserId = item.UserId;
                x.OrganizationId = item.OrganizationId!.Value;
                x.UserToUpdateId = targetUserOrganization.UserId;
                x.Role = userRole;
            });

        // Act
        Func<Task> act = () => organizationManager.ChangeUserRoleAsync(model, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        Context.Set<UserOrganization>().Should().HaveCount(3);

        var targetItem = Context.Set<UserOrganization>().FirstOrDefault(x => x.Id == targetUserOrganization.Id);
        targetItem.Should().NotBeNull();
        targetItem!.Role.Should().Be(role);

        var notTargetItem = Context.Set<UserOrganization>().FirstOrDefault(x => x.Id == notTargetUserOrganization.Id);
        notTargetItem.Should().NotBeNull();
        notTargetItem!.Role.Should().Be(notTargetUserOrganizationRole);
    }

    /// <summary>
    /// Изменение роли пользователя не работает, пользователь пытается изменить роль самому себе
    /// </summary>
    [Fact]
    public async Task ChangeUserRoleShouldThrowBySameUser()
    {
        //Arrange
        var item = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = Guid.NewGuid();
            x.Role = Role.Admin;
        });
        await Context.AddRangeAsync(item);
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared
            .Create<ChangeUserRoleModel>(x =>
            {
                x.UserId = item.UserId;
                x.OrganizationId = item.OrganizationId!.Value;
                x.UserToUpdateId = item.UserId;
                x.Role = UserRole.User;
            });

        // Act
        Func<Task> act = () => organizationManager.ChangeUserRoleAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationInvalidOperationException>();
    }

    /// <summary>
    /// Изменение роли пользователя не работает, пользователь пытается задать пользователю роль, которая уже ему назначена
    /// </summary>
    [Fact]
    public async Task ChangeUserRoleShouldThrowBySameRole()
    {
        //Arrange
        var item = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = Guid.NewGuid();
            x.Role = Role.Admin;
        });
        var targetUserOrganization = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = item.OrganizationId!.Value;
            x.Role = Role.User;
        });
        await Context.AddRangeAsync(item, targetUserOrganization);
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared
            .Create<ChangeUserRoleModel>(x =>
            {
                x.UserId = item.UserId;
                x.OrganizationId = item.OrganizationId!.Value;
                x.UserToUpdateId = targetUserOrganization.UserId;
                x.Role = UserRole.User;
            });

        // Act
        Func<Task> act = () => organizationManager.ChangeUserRoleAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationInvalidOperationException>();
    }

    /// <summary>
    /// Получение пользователя в организации бросает исключение для пользователя, которого нет в организации
    /// </summary>
    [Fact]
    public async Task GetUserOrganizationShouldThrowForNonMember()
    {
        // Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>(x => x.Id = Guid.NewGuid());
        var user = TestEntityProvider.Shared.Create<User>(x => x.Id = Guid.NewGuid());
        var userOrganization = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = Guid.NewGuid();
            x.UserId = user.Id;
        });
        Context.AddRange(organization, user, userOrganization);
        await UnitOfWork.SaveChangesAsync();

        // Act
        var act = () => organizationManager.GetUserOrganizationAsync(
            userOrganization.UserId,
            organization.Id,
            CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationAccessException>();
    }

    /// <summary>
    /// Получение пользователя в организации бросает исключение для пользователя, которого нет в базе данных
    /// </summary>
    [Fact]
    public async Task GetUserOrganizationShouldThrowForNonExisting()
    {
        // Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>(x => x.Id = Guid.NewGuid());
        var userOrganization = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = Guid.NewGuid();
            x.UserId = Guid.NewGuid();
        });
        Context.AddRange(organization, userOrganization);
        await UnitOfWork.SaveChangesAsync();

        // Act
        var act = () => organizationManager.GetUserOrganizationAsync(
            userOrganization.UserId,
            userOrganization.OrganizationId!.Value,
            CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationEntityNotFoundException<User>>()
            .WithMessage($"*{userOrganization.UserId}*");
    }

    /// <summary>
    /// Получение пользователя в организации работает
    /// </summary>
    [Fact]
    public async Task GetUserOrganizationShouldWork()
    {
        // Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>(x => x.Id = Guid.NewGuid());
        var user = TestEntityProvider.Shared.Create<User>(x => x.Id = Guid.NewGuid());
        var userOrganization = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = organization.Id;
            x.UserId = user.Id;
            x.Role = Role.User;
        });
        Context.AddRange(organization, user, userOrganization);
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await organizationManager.GetUserOrganizationAsync(
            userOrganization.UserId,
            userOrganization.OrganizationId!.Value,
            CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(new { user.Id, userOrganization.Role });
    }
}
