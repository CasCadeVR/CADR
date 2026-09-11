using Ahatornn.TestGenerator;
using AutoMapper;
using CADR.Administrations.Entities;
using CADR.Administrations.Entities.Enums;
using CADR.Administrations.Services.AutoMappers;
using CADR.Administrations.Services.Contracts.Exceptions;
using CADR.Administrations.Services.Contracts.Interfaces;
using CADR.Administrations.Services.Contracts.Models.Enums;
using CADR.Administrations.Services.Contracts.Models.Invites;
using CADR.Administrations.Services.Invite;
using CADR.Administrations.Services.Tests.UnitOfWork;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;

namespace CADR.Administrations.Services.Tests;

/// <summary>
/// Тесты на <see cref="IInviteManager"/>
/// </summary>
public class InviteManagerTests : CadrContextInMemory
{
    private readonly IInviteManager inviteManager;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="InviteManagerTests"/>
    /// </summary>
    public InviteManagerTests()
    {
        var config = new MapperConfiguration(opts =>
        {
            opts.AddProfile(new AdministrationServiceProfile());
        });
        var unitOfWorkMock = new TestUnitOfWork(WriterContext, Context, Context);
        inviteManager = new InviteManager(unitOfWorkMock.AdministrationUnitOfWork, config.CreateMapper());
    }
    /// <summary>
    /// Создание приглашения не работает, организация не найдена
    /// </summary>
    [Fact]
    public Task CreateInviteShouldThrowByOrganization()
    {
        //Arrange
        var model = TestEntityProvider.Shared.Create<InviteModel>();

        // Act
        Func<Task> act = () => inviteManager.CreateInviteAsync(model, CancellationToken.None);

        // Assert
        return act.Should().ThrowAsync<AdministrationEntityNotFoundException<UserOrganization>>().WithMessage($"*{model.OrganizationId}*");
    }

    /// <summary>
    /// Создание приглашения не работает, пользователь не найден
    /// </summary>
    [Fact]
    public async Task CreateInviteShouldThrowByUser()
    {
        //Arrange
        var user1 = TestEntityProvider.Shared.Create<User>();
        var userOrganization = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = Guid.NewGuid();
            x.UserId = user1.Id;
        });
        await Context.AddRangeAsync(userOrganization, user1);
        await Context.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<InviteModel>(x =>
        {
            x.OrganizationId = userOrganization.OrganizationId!.Value;
            x.OwnerId = user1.Id;
        });

        // Act
        Func<Task> act = () => inviteManager.CreateInviteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationNotFoundException>().WithMessage($"*{model.UserMail}*");
    }

    /// <summary>
    /// Создание приглашения не работает, пользователь уже добавлен в организацию
    /// </summary>
    [Fact]
    public async Task CreateInviteShouldThrowUserAlreadyExist()
    {
        //Arrange
        var ownUser = TestEntityProvider.Shared.Create<User>();
        var targetUser = TestEntityProvider.Shared.Create<User>(x => x.Email = x.EmailLowerCase = "login@mail.domain");
        var ownUserOrganization = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = Guid.NewGuid();
            x.UserId = ownUser.Id;
        });
        var targetUserOrganization = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = ownUserOrganization.OrganizationId!.Value;
            x.UserId = targetUser.Id;
        });
        await Context.AddRangeAsync(ownUserOrganization, ownUser, targetUser, targetUserOrganization);
        await Context.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<InviteModel>(x =>
        {
            x.OrganizationId = ownUserOrganization.OrganizationId!.Value;
            x.OwnerId = ownUser.Id;
            x.UserMail = targetUser.Email;
        });

        // Act
        Func<Task> act = () => inviteManager.CreateInviteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationInvalidOperationException>().WithMessage($"*{model.UserMail}*");
    }

    /// <summary>
    /// Создание приглашения не работает, пользователь уже получил приглашение
    /// </summary>
    [Fact]
    public async Task CreateInviteShouldThrowUserAlreadyInvited()
    {
        //Arrange
        var ownUser = TestEntityProvider.Shared.Create<User>();
        var targetUser = TestEntityProvider.Shared.Create<User>(x => x.Email = x.EmailLowerCase = "login@mail.domain");
        var ownUserOrganization = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = Guid.NewGuid();
            x.UserId = ownUser.Id;
        });
        var invited = TestEntityProvider.Shared.Create<UserInvite>(x =>
        {
            x.OrganizationId = ownUserOrganization.OrganizationId!.Value;
            x.UserId = targetUser.Id;
        });
        await Context.AddRangeAsync(ownUserOrganization, ownUser, targetUser, invited);
        await Context.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<InviteModel>(x =>
        {
            x.OrganizationId = ownUserOrganization.OrganizationId!.Value;
            x.OwnerId = ownUser.Id;
            x.UserMail = targetUser.Email;
        });

        // Act
        Func<Task> act = () => inviteManager.CreateInviteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationInvalidOperationException>().WithMessage($"*{invited.CreatedAt:d.M.yyyy HH:mm}*");
    }

    /// <summary>
    /// Создание приглашения работает
    /// </summary>
    [Fact]
    public async Task CreateInviteShouldWork()
    {
        //Arrange
        var ownUser = TestEntityProvider.Shared.Create<User>();
        var targetUser = TestEntityProvider.Shared.Create<User>(x => x.Email = x.EmailLowerCase = "login@mail.domain");
        var ownUserOrganization = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = Guid.NewGuid();
            x.UserId = ownUser.Id;
            x.Role = Role.Architect;
        });
        await Context.AddRangeAsync(ownUserOrganization, ownUser, targetUser);
        await Context.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<InviteModel>(x =>
        {
            x.OrganizationId = ownUserOrganization.OrganizationId!.Value;
            x.OwnerId = ownUser.Id;
            x.UserMail = targetUser.Email;
            x.Role = UserRole.Admin;
        });
        // Act
        Func<Task> act = () => inviteManager.CreateInviteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        var invite = Context.Set<UserInvite>().First(x => x.UserId == targetUser.Id);
        invite.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new
            {
                OrganizationId = ownUserOrganization.OrganizationId!.Value,
                Role = Role.Architect,
            });
    }

    /// <summary>
    /// Получает список приглашений организации по идентификатору
    /// </summary>
    [Fact]
    public async Task GetInvitesByOrganizationIdShouldReturnValue()
    {
        //Arrange
        var targetOrganizationId = Guid.NewGuid();
        var ownUser = TestEntityProvider.Shared.Create<User>();
        var ownUserOrganization = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.UserId = ownUser.Id;
            x.Role = Role.User;
        });
        var item = TestEntityProvider.Shared.Create<UserInvite>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.Role = Role.Admin;
        });
        var user = TestEntityProvider.Shared.Create<User>(x => x.Id = item.UserId);
        await Context.AddRangeAsync(item,
            user,
            ownUser,
            ownUserOrganization,
            TestEntityProvider.Shared.Create<Organization>(x => x.Id = targetOrganizationId),
            TestEntityProvider.Shared.Create<UserInvite>(x =>
            {
                x.UserId = item.UserId;
                x.OrganizationId = Guid.NewGuid();
            }),
            TestEntityProvider.Shared.Create<UserInvite>(x => x.OrganizationId = item.OrganizationId),
            TestEntityProvider.Shared.Create<UserInvite>(x =>
            {
                x.UserId = item.UserId;
                x.OrganizationId = item.OrganizationId;
                x.DeletedAt = DateTimeOffset.UtcNow;
            }));
        await Context.SaveChangesAsync();

        // Act
        var result = await inviteManager.GetInvitesByOrganizationIdAsync(targetOrganizationId, ownUser.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.HaveCount(1)
            .And.ContainSingle(x => x.Id == item.Id &&
                                    x.UserName == user.Name &&
                                    x.UserMail == user.Email &&
                                    x.Role == UserRole.Admin);
    }

    /// <summary>
    /// Удаление приглашения не работает, пользователь не принадлежит указанной организации
    /// </summary>
    [Fact]
    public async Task DeleteInviteShouldThrowByOrganization()
    {
        //Arrange
        var targetUserOrganization = TestEntityProvider.Shared.Create<UserOrganization>(x => x.OrganizationId = Guid.NewGuid());
        var targetInvite = TestEntityProvider.Shared.Create<UserInvite>();
        await Context.AddRangeAsync(targetUserOrganization, targetInvite);
        await Context.SaveChangesAsync();
        var model = TestEntityProvider.Shared
            .Create<DeleteOrganizationInviteModel>(x =>
            {
                x.UserId = targetUserOrganization.UserId;
                x.InviteId = targetInvite.Id;
            });

        // Act
        Func<Task> act = () => inviteManager.DeleteInviteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationAccessException>();
    }

    /// <summary>
    /// Удаление приглашения не работает, пользователь организации не админ
    /// </summary>
    [Theory]
    [InlineData(Role.User)]
    [InlineData(Role.Architect)]
    public async Task DeleteInviteShouldThrowByRole(Role role)
    {
        //Arrange
        var organizationId = Guid.NewGuid();
        var targetUserOrganization = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = organizationId;
            x.Role = role;
        });
        var targetInvite = TestEntityProvider.Shared.Create<UserInvite>(x => x.OrganizationId = organizationId);
        await Context.AddRangeAsync(targetUserOrganization, targetInvite);
        await Context.SaveChangesAsync();
        var model = new DeleteOrganizationInviteModel
        {
            UserId = targetUserOrganization.UserId,
            InviteId = targetInvite.Id,
        };

        // Act
        Func<Task> act = () => inviteManager.DeleteInviteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationAccessException>();
    }

    /// <summary>
    /// Удаление приглашения не работает, приглашение не найдено
    /// </summary>
    [Fact]
    public async Task DeleteInviteShouldThrowByNotFound()
    {
        //Arrange
        var item = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = Guid.NewGuid();
            x.Role = Role.Admin;
        });
        await Context.AddAsync(item);
        await Context.SaveChangesAsync();
        var model = new DeleteOrganizationInviteModel
        {
            UserId = item.UserId,
            OrganizationId = item.OrganizationId!.Value,
        };

        // Act
        Func<Task> act = () => inviteManager.DeleteInviteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationEntityNotFoundException<UserInvite>>()
            .WithMessage($"*{model.InviteId}*");
    }

    /// <summary>
    /// Удаление приглашения работает
    /// </summary>
    [Fact]
    public async Task DeleteInviteShouldWork()
    {
        //Arrange
        var organizationId = Guid.NewGuid();
        var targetUserOrganization = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.OrganizationId = organizationId;
            x.Role = Role.Admin;
        });
        var targetInvite = TestEntityProvider.Shared.Create<UserInvite>(x => x.OrganizationId = organizationId);
        await Context.AddRangeAsync(targetUserOrganization, targetInvite);
        await UnitOfWork.SaveChangesAsync();
        var model = new DeleteOrganizationInviteModel
        {
            UserId = targetUserOrganization.UserId,
            InviteId = targetInvite.Id,
        };

        // Act
        Func<Task> act = () => inviteManager.DeleteInviteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        var deletedItem = Context.Set<UserInvite>().First(x => x.Id == targetInvite.Id);
        deletedItem.DeletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Получение приглашений для пользователя возвращает значение
    /// </summary>
    [Fact]
    public async Task GetInvitesForUserShouldReturnValue()
    {
        //Arrange
        var targetUser = TestEntityProvider.Shared.Create<User>();
        var organization1 = TestEntityProvider.Shared.Create<Organization>();
        var targetInvite1 = TestEntityProvider.Shared.Create<UserInvite>(x =>
        {
            x.UserId = targetUser.Id;
            x.Role = Role.User;
            x.OrganizationId = organization1.Id;
        });
        var organization2 = TestEntityProvider.Shared.Create<Organization>();
        var targetInvite2 = TestEntityProvider.Shared.Create<UserInvite>(x =>
        {
            x.UserId = targetUser.Id;
            x.Role = Role.User;
            x.OrganizationId = organization2.Id;
        });
        var nonUserInvite = TestEntityProvider.Shared.Create<UserInvite>(x =>
        {
            x.UserId = Guid.NewGuid();
            x.Role = Role.User;
            x.OrganizationId = organization2.Id;
        });
        await Context.AddRangeAsync(targetUser, organization1, organization2, targetInvite1, targetInvite2, nonUserInvite);
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await inviteManager.GetInvitesByUserIdAsync(targetUser.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == targetInvite1.Id)
            .And.ContainSingle(x => x.Id == targetInvite2.Id);
    }

    /// <summary>
    /// Получение приглашений для пользователя не нашло пользователя
    /// </summary>
    [Fact]
    public async Task GetInvitesForUserShouldReturnEmpty()
    {
        //Arrange
        var targetUser = TestEntityProvider.Shared.Create<User>();
        var organization1 = TestEntityProvider.Shared.Create<Organization>();
        var targetInvite1 = TestEntityProvider.Shared.Create<UserInvite>(x =>
        {
            x.UserId = Guid.NewGuid();
            x.Role = Role.User;
            x.OrganizationId = organization1.Id;
        });
        var organization2 = TestEntityProvider.Shared.Create<Organization>();
        var targetInvite2 = TestEntityProvider.Shared.Create<UserInvite>(x =>
        {
            x.UserId = Guid.NewGuid();
            x.Role = Role.User;
            x.OrganizationId = organization2.Id;
        });
        await Context.AddRangeAsync(targetUser, organization1, organization2, targetInvite1, targetInvite2);
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await inviteManager.GetInvitesByUserIdAsync(targetUser.Id, CancellationToken.None);

        // Assert
        result.Should()
            .BeEmpty();
    }

    /// <summary>
    /// Принятие приглашения, адресованного пользователю работает
    /// </summary>
    [Fact]
    public async Task AcceptInviteShouldWork()
    {
        //Arrange
        var targetUser = TestEntityProvider.Shared.Create<User>();
        var otherUser = TestEntityProvider.Shared.Create<User>();
        var targetOrganization = TestEntityProvider.Shared.Create<Organization>();
        var targetInvite = TestEntityProvider.Shared.Create<UserInvite>(x =>
        {
            x.UserId = targetUser.Id;
            x.Role = Role.User;
            x.OrganizationId = targetOrganization.Id;
        });
        var otherInvite = TestEntityProvider.Shared.Create<UserInvite>(x =>
        {
            x.UserId = otherUser.Id;
            x.Role = Role.User;
            x.OrganizationId = targetOrganization.Id;
        });
        await Context.AddRangeAsync(targetUser, targetOrganization, targetInvite, otherInvite);
        await UnitOfWork.SaveChangesAsync();

        // Act
        Func<Task> act = () => inviteManager.AcceptInviteAsync(targetInvite.Id, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        var userOrganization = Context.Set<UserOrganization>().FirstOrDefault(x => x.UserId == targetUser.Id);
        userOrganization.Should()
            .NotBeNull();
    }

    /// <summary>
    /// Принятие приглашения, адресованного пользователю выдаёт ошибку: приглашение не найдено
    /// </summary>
    [Fact]
    public async Task AcceptInviteShouldThrowNotFound()
    {
        //Arrange
        var targetUser = TestEntityProvider.Shared.Create<User>();
        var otherUser = TestEntityProvider.Shared.Create<User>();
        var targetOrganization = TestEntityProvider.Shared.Create<Organization>();
        var targetInvite = TestEntityProvider.Shared.Create<UserInvite>(x =>
        {
            x.UserId = targetUser.Id;
            x.Role = Role.User;
            x.OrganizationId = targetOrganization.Id;
        });
        var otherInvite = TestEntityProvider.Shared.Create<UserInvite>(x =>
        {
            x.UserId = otherUser.Id;
            x.Role = Role.User;
            x.OrganizationId = targetOrganization.Id;
        });
        await Context.AddRangeAsync(targetUser, targetOrganization, otherInvite);
        await UnitOfWork.SaveChangesAsync();

        // Act
        Func<Task> act = () => inviteManager.AcceptInviteAsync(targetInvite.Id, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationEntityNotFoundException<UserInvite>>()
            .WithMessage($"*{targetInvite.Id}*");
    }

    /// <summary>
    /// Принятие приглашения, адресованного пользователю выдаёт ошибку: пользователь уже присутствует в организации
    /// </summary>
    [Fact]
    public async Task AcceptInviteShouldThrowNotAcceptable()
    {
        //Arrange
        var targetUser = TestEntityProvider.Shared.Create<User>(x =>
        x.Email = "testEmail@gmail.com");
        var otherUser = TestEntityProvider.Shared.Create<User>();
        var targetOrganization = TestEntityProvider.Shared.Create<Organization>(x =>
        {
            x.Name = "TestName";
        });
        var targetUserOrganization = TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.UserId = targetUser.Id;
            x.Role = Role.User;
            x.OrganizationId = targetOrganization.Id;
        });
        var targetInvite = TestEntityProvider.Shared.Create<UserInvite>(x =>
        {
            x.UserId = targetUser.Id;
            x.Role = Role.User;
            x.OrganizationId = targetOrganization.Id;
        });
        var otherInvite = TestEntityProvider.Shared.Create<UserInvite>(x =>
        {
            x.UserId = otherUser.Id;
            x.Role = Role.User;
            x.OrganizationId = targetOrganization.Id;
        });
        await Context.AddRangeAsync(targetUser,
            otherUser,
            targetOrganization,
            targetUserOrganization,
            targetInvite,
            otherInvite);
        await UnitOfWork.SaveChangesAsync();

        // Act
        Func<Task> act = () => inviteManager.AcceptInviteAsync(targetInvite.Id, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationInvalidOperationException>()
            .WithMessage($"*{targetUser.Email}*");
    }

    /// <summary>
    /// Отклонение приглашения, адресованного пользователю работает
    /// </summary>
    [Fact]
    public async Task RejectInviteShouldWork()
    {
        //Arrange
        var targetUser = TestEntityProvider.Shared.Create<User>();
        var otherUser = TestEntityProvider.Shared.Create<User>();
        var targetOrganization = TestEntityProvider.Shared.Create<Organization>();
        var targetInvite = TestEntityProvider.Shared.Create<UserInvite>(x =>
        {
            x.UserId = targetUser.Id;
            x.Role = Role.User;
            x.OrganizationId = targetOrganization.Id;
        });
        var otherInvite = TestEntityProvider.Shared.Create<UserInvite>(x =>
        {
            x.UserId = otherUser.Id;
            x.Role = Role.User;
            x.OrganizationId = targetOrganization.Id;
        });
        await Context.AddRangeAsync(targetUser, otherUser, targetOrganization, targetInvite, otherInvite);
        await UnitOfWork.SaveChangesAsync();

        // Act
        Func<Task> act = () => inviteManager.RejectInviteAsync(targetInvite.Id, targetUser.Id, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        var invite = Context.Set<UserInvite>().First(x => x.Id == targetInvite.Id);
        invite.DeletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Отклонение приглашения, адресованного пользователю не нашло приглашения, и выдаёт ошибку: У вас нет прав для выполнения действия
    /// </summary>
    [Fact]
    public async Task RejectInviteShouldNotFound()
    {
        //Arrange
        var targetUser = TestEntityProvider.Shared.Create<User>();
        var otherUser = TestEntityProvider.Shared.Create<User>();
        var targetOrganization = TestEntityProvider.Shared.Create<Organization>();
        var targetInvite = TestEntityProvider.Shared.Create<UserInvite>(x =>
        {
            x.UserId = targetUser.Id;
            x.Role = Role.User;
            x.OrganizationId = targetOrganization.Id;
        });
        var otherInvite = TestEntityProvider.Shared.Create<UserInvite>(x =>
        {
            x.UserId = otherUser.Id;
            x.Role = Role.User;
            x.OrganizationId = targetOrganization.Id;
        });
        await Context.AddRangeAsync(targetUser, otherUser, targetOrganization, otherInvite);
        await UnitOfWork.SaveChangesAsync();

        // Act
        Func<Task> act = () => inviteManager.RejectInviteAsync(targetInvite.Id, targetUser.Id, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationAccessException>();
    }
}
