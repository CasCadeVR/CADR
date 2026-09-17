using Ahatornn.TestGenerator;
using AutoMapper;
using CADR.Administrations.Entities;
using CADR.Administrations.Entities.Enums;
using CADR.Adrs.Entities;
using CADR.Adrs.Services.AutoMappers;
using CADR.Adrs.Services.Comments;
using CADR.Adrs.Services.Contracts.Exceptions;
using CADR.Adrs.Services.Contracts.Interfaces;
using CADR.Adrs.Services.Contracts.Models.Comments;
using CADR.Adrs.Services.Tests.UnitOfWork;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;

namespace CADR.Adrs.Services.Tests;

/// <summary>
/// Тесты на <see cref="IAdrCommentManager"/>
/// </summary>
public class AdrCommentManagerTests : CadrContextInMemory
{
    private readonly IAdrCommentManager adrCommentManager;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrCommentManagerTests"/>
    /// </summary>
    public AdrCommentManagerTests()
    {
        var config = new MapperConfiguration(opts =>
        {
            opts.AddProfile(new AdrServiceProfile());
        });
        var unitOfWorkMock = new TestUnitOfWork(WriterContext, Context, Context);

        adrCommentManager = new AdrCommentManager(unitOfWorkMock.AdrUnitOfWork,
            config.CreateMapper(),
            unitOfWorkMock.UserOrganizationReadRepository,
            unitOfWorkMock.UserReadRepository);
    }

    /// <summary>
    /// Создание комментария работает
    /// </summary>
    [Fact]
    public async Task CreateShouldWork()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var user = TestEntityProvider.Shared.Create<User>();
        var adr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        await Context.AddRangeAsync(organization,
            user,
            adr,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = user.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.User;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<CreateAdrCommentModel>(x =>
        {
            x.AdrId = adr.Id;
            x.UserId = user.Id;
        });

        // Act
        var result = await adrCommentManager.CreateAsync(model, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new
            {
                model.Text,
                model.AdrId,
                AuthorId = model.UserId,
                AuthorName = user.Name,
                AuthorLogin = user.Login,
            });
        var comment = Context.Set<AdrComment>().FirstOrDefault(x => x.AdrId == adr.Id);
        comment.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { model.Text, model.AdrId, AuthorId = model.UserId, });
    }

    /// <summary>
    /// Создание комментария выдаёт ошибку: ADR не найден
    /// </summary>
    [Fact]
    public async Task CreateShouldThrowAdrNotFound()
    {
        //Arrange
        var model = TestEntityProvider.Shared.Create<CreateAdrCommentModel>();

        // Act
        Func<Task> act = () => adrCommentManager.CreateAsync(model, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<AdrEntityNotFoundException<Adr>>()
            .WithMessage($"*{model.AdrId}*");
    }

    /// <summary>
    /// Создание комментария выдаёт ошибку: пользователь не состоит в организации
    /// </summary>
    [Fact]
    public async Task CreateShouldThrowNotMember()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var adr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        await Context.AddRangeAsync(organization, adr);
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<CreateAdrCommentModel>(x => x.AdrId = adr.Id);

        // Act
        Func<Task> act = () => adrCommentManager.CreateAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrAccessException>();
    }

    /// <summary>
    /// Получение комментариев возвращает только активные
    /// </summary>
    [Fact]
    public async Task GetByAdrIdShouldReturnOnlyActive()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var user = TestEntityProvider.Shared.Create<User>();
        var adr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        var comment1 = TestEntityProvider.Shared.Create<AdrComment>(x => x.AdrId = adr.Id);
        var comment2 = TestEntityProvider.Shared.Create<AdrComment>(x =>
        {
            x.AdrId = adr.Id;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        var comment3 = TestEntityProvider.Shared.Create<AdrComment>(x => x.AdrId = Guid.NewGuid());
        await Context.AddRangeAsync(organization,
            user,
            adr,
            comment1,
            comment2,
            comment3,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = user.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.User;
            }));
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await adrCommentManager.GetByAdrIdAsync(adr.Id, user.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(1)
            .And.ContainSingle(x => x.Id == comment1.Id);
    }

    /// <summary>
    /// Обновление комментария автором работает
    /// </summary>
    [Fact]
    public async Task UpdateShouldWork()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var user = TestEntityProvider.Shared.Create<User>();
        var adr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        var comment = TestEntityProvider.Shared.Create<AdrComment>(x =>
        {
            x.AdrId = adr.Id;
            x.AuthorId = user.Id;
        });
        await Context.AddRangeAsync(organization,
            user,
            adr,
            comment,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = user.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.User;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<UpdateAdrCommentModel>(x =>
        {
            x.Id = comment.Id;
            x.AdrId = adr.Id;
            x.AuthorId = user.Id;
            x.UserId = user.Id;
        });

        // Act
        var result = await adrCommentManager.UpdateAsync(model, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { model.Text, });
        var updatedComment = Context.Set<AdrComment>().First(x => x.Id == comment.Id);
        updatedComment.Text.Should().Be(model.Text);
        updatedComment.DeletedAt.Should().BeNull();
    }

    /// <summary>
    /// Обновление комментария выдаёт ошибку: пользователь не автор комментария
    /// </summary>
    [Fact]
    public async Task UpdateShouldThrowDenyForNonAuthor()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var author = TestEntityProvider.Shared.Create<User>();
        var otherUser = TestEntityProvider.Shared.Create<User>();
        var adr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        var comment = TestEntityProvider.Shared.Create<AdrComment>(x =>
        {
            x.AdrId = adr.Id;
            x.AuthorId = author.Id;
        });
        await Context.AddRangeAsync(organization,
            author,
            otherUser,
            adr,
            comment,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = otherUser.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.User;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<UpdateAdrCommentModel>(x =>
        {
            x.Id = comment.Id;
            x.AdrId = adr.Id;
            x.UserId = otherUser.Id;
        });

        // Act
        Func<Task> act = () => adrCommentManager.UpdateAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrAccessException>();
    }

    /// <summary>
    /// Обновление комментария выдаёт ошибку: комментарий не найден
    /// </summary>
    [Fact]
    public async Task UpdateShouldThrowNotFound()
    {
        //Arrange
        var model = TestEntityProvider.Shared.Create<UpdateAdrCommentModel>();

        // Act
        Func<Task> act = () => adrCommentManager.UpdateAsync(model, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<AdrEntityNotFoundException<AdrComment>>()
            .WithMessage($"*{model.Id}*");
    }

    /// <summary>
    /// Удаление комментария автором работает
    /// </summary>
    [Fact]
    public async Task DeleteShouldWorkByAuthor()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var user = TestEntityProvider.Shared.Create<User>();
        var adr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        var comment = TestEntityProvider.Shared.Create<AdrComment>(x =>
        {
            x.AdrId = adr.Id;
            x.AuthorId = user.Id;
        });
        await Context.AddRangeAsync(organization,
            user,
            adr,
            comment,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = user.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.User;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<DeleteAdrCommentModel>(x =>
        {
            x.AdrCommentId = comment.Id;
            x.UserId = user.Id;
        });

        // Act
        Func<Task> act = () => adrCommentManager.DeleteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        Context.Set<AdrComment>().First(x => x.Id == comment.Id).DeletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Удаление чужого комментария архитектором работает
    /// </summary>
    [Fact]
    public async Task DeleteShouldWorkByArchitect()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var author = TestEntityProvider.Shared.Create<User>();
        var architect = TestEntityProvider.Shared.Create<User>();
        var adr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        var comment = TestEntityProvider.Shared.Create<AdrComment>(x =>
        {
            x.AdrId = adr.Id;
            x.AuthorId = author.Id;
        });
        await Context.AddRangeAsync(organization,
            author,
            architect,
            adr,
            comment,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = architect.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.Architect;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<DeleteAdrCommentModel>(x =>
        {
            x.AdrCommentId = comment.Id;
            x.UserId = architect.Id;
        });

        // Act
        Func<Task> act = () => adrCommentManager.DeleteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        Context.Set<AdrComment>().First(x => x.Id == comment.Id).DeletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Удаление чужого комментария выдаёт ошибку: не хватает прав
    /// </summary>
    [Fact]
    public async Task DeleteShouldThrowDenyForUserRole()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var author = TestEntityProvider.Shared.Create<User>();
        var otherUser = TestEntityProvider.Shared.Create<User>();
        var adr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        var comment = TestEntityProvider.Shared.Create<AdrComment>(x =>
        {
            x.AdrId = adr.Id;
            x.AuthorId = author.Id;
        });
        await Context.AddRangeAsync(organization,
            author,
            otherUser,
            adr,
            comment,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = otherUser.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.User;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<DeleteAdrCommentModel>(x =>
        {
            x.AdrCommentId = comment.Id;
            x.UserId = otherUser.Id;
        });

        // Act
        Func<Task> act = () => adrCommentManager.DeleteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrAccessException>();
    }

    /// <summary>
    /// Удаление комментария выдаёт ошибку: комментарий не найден
    /// </summary>
    [Fact]
    public async Task DeleteShouldThrowNotFound()
    {
        //Arrange
        var model = TestEntityProvider.Shared.Create<DeleteAdrCommentModel>();

        // Act
        Func<Task> act = () => adrCommentManager.DeleteAsync(model, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<AdrEntityNotFoundException<AdrComment>>()
            .WithMessage($"*{model.AdrCommentId}*");
    }
}
