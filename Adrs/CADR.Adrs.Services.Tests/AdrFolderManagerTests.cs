using Ahatornn.TestGenerator;
using AutoMapper;
using CADR.Administrations.Entities;
using CADR.Administrations.Entities.Enums;
using CADR.Adrs.Entities;
using CADR.Adrs.Services.AutoMappers;
using CADR.Adrs.Services.Contracts.Exceptions;
using CADR.Adrs.Services.Contracts.Interfaces;
using CADR.Adrs.Services.Contracts.Models.Folders;
using CADR.Adrs.Services.Folders;
using CADR.Adrs.Services.Tests.UnitOfWork;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;

namespace CADR.Adrs.Services.Tests;

/// <summary>
/// Тесты на <see cref="IAdrFolderManager"/>
/// </summary>
public class AdrFolderManagerTests : CadrContextInMemory
{
    private readonly IAdrFolderManager adrFolderManager;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrFolderManagerTests"/>
    /// </summary>
    public AdrFolderManagerTests()
    {
        var config = new MapperConfiguration(opts =>
        {
            opts.AddProfile(new AdrServiceProfile());
        });
        var unitOfWorkMock = new TestUnitOfWork(WriterContext, Context, Context);

        adrFolderManager = new AdrFolderManager(unitOfWorkMock.AdrUnitOfWork,
            config.CreateMapper(),
            unitOfWorkMock.UserOrganizationReadRepository);
    }

    /// <summary>
    /// Создание папки архитектором работает
    /// </summary>
    [Fact]
    public async Task CreateShouldWork()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var architect = TestEntityProvider.Shared.Create<User>();
        var parent = TestEntityProvider.Shared.Create<AdrFolder>(x => x.OrganizationId = organization.Id);
        await Context.AddRangeAsync(organization,
            architect,
            parent,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = architect.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.Architect;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<CreateAdrFolderModel>(x =>
        {
            x.OrganizationId = organization.Id;
            x.ParentAdrFolderId = parent.Id;
            x.UserId = architect.Id;
        });

        // Act
        var result = await adrFolderManager.CreateAsync(model, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { model.Name, model.OrganizationId, model.ParentAdrFolderId, });
        Context.Set<AdrFolder>().Should().HaveCount(2);
    }

    /// <summary>
    /// Создание папки выдаёт ошибку: не хватает прав
    /// </summary>
    [Fact]
    public async Task CreateShouldThrowDeny()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var user = TestEntityProvider.Shared.Create<User>();
        await Context.AddRangeAsync(organization,
            user,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = user.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.User;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<CreateAdrFolderModel>(x =>
        {
            x.OrganizationId = organization.Id;
            x.UserId = user.Id;
        });

        // Act
        Func<Task> act = () => adrFolderManager.CreateAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrAccessException>();
    }

    /// <summary>
    /// Создание папки выдаёт ошибку: родительская папка не найдена
    /// </summary>
    [Fact]
    public async Task CreateShouldThrowParentNotFound()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var architect = TestEntityProvider.Shared.Create<User>();
        await Context.AddRangeAsync(organization,
            architect,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = architect.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.Architect;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<CreateAdrFolderModel>(x =>
        {
            x.OrganizationId = organization.Id;
            x.UserId = architect.Id;
        });

        // Act
        Func<Task> act = () => adrFolderManager.CreateAsync(model, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<AdrEntityNotFoundException<AdrFolder>>()
            .WithMessage($"*{model.ParentAdrFolderId}*");
    }

    /// <summary>
    /// Создание папки выдаёт ошибку: родительская папка из другой организации
    /// </summary>
    [Fact]
    public async Task CreateShouldThrowParentFromAnotherOrganization()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var otherOrganization = TestEntityProvider.Shared.Create<Organization>();
        var architect = TestEntityProvider.Shared.Create<User>();
        var parent = TestEntityProvider.Shared.Create<AdrFolder>(x => x.OrganizationId = otherOrganization.Id);
        await Context.AddRangeAsync(organization,
            otherOrganization,
            architect,
            parent,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = architect.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.Architect;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<CreateAdrFolderModel>(x =>
        {
            x.OrganizationId = organization.Id;
            x.ParentAdrFolderId = parent.Id;
            x.UserId = architect.Id;
        });

        // Act
        Func<Task> act = () => adrFolderManager.CreateAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrInvalidOperationException>();
    }

    /// <summary>
    /// Получение списка папок возвращает только активные папки организации
    /// </summary>
    [Fact]
    public async Task GetByOrganizationIdShouldReturnOnlyActive()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var otherOrganization = TestEntityProvider.Shared.Create<Organization>();
        var user = TestEntityProvider.Shared.Create<User>();
        var folder1 = TestEntityProvider.Shared.Create<AdrFolder>(x => x.OrganizationId = organization.Id);
        var folder2 = TestEntityProvider.Shared.Create<AdrFolder>(x =>
        {
            x.OrganizationId = organization.Id;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        var folder3 = TestEntityProvider.Shared.Create<AdrFolder>(x => x.OrganizationId = otherOrganization.Id);
        await Context.AddRangeAsync(organization,
            otherOrganization,
            user,
            folder1,
            folder2,
            folder3,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = user.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.User;
            }));
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await adrFolderManager.GetByOrganizationIdAsync(organization.Id, user.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(1)
            .And.ContainSingle(x => x.Id == folder1.Id);
    }

    /// <summary>
    /// Обновление папки (переименование и перемещение) работает
    /// </summary>
    [Fact]
    public async Task UpdateShouldWork()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var architect = TestEntityProvider.Shared.Create<User>();
        var parent = TestEntityProvider.Shared.Create<AdrFolder>(x => x.OrganizationId = organization.Id);
        var folder = TestEntityProvider.Shared.Create<AdrFolder>(x => x.OrganizationId = organization.Id);
        await Context.AddRangeAsync(organization,
            architect,
            parent,
            folder,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = architect.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.Architect;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<UpdateAdrFolderModel>(x =>
        {
            x.Id = folder.Id;
            x.OrganizationId = organization.Id;
            x.ParentAdrFolderId = parent.Id;
            x.UserId = architect.Id;
        });

        // Act
        var result = await adrFolderManager.UpdateAsync(model, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { model.Name, model.ParentAdrFolderId, });
        var updatedFolder = Context.Set<AdrFolder>().First(x => x.Id == folder.Id);
        updatedFolder.Name.Should().Be(model.Name);
        updatedFolder.ParentAdrFolderId.Should().Be(parent.Id);
    }

    /// <summary>
    /// Обновление папки выдаёт ошибку: перемещение в саму себя
    /// </summary>
    [Fact]
    public async Task UpdateShouldThrowMoveToItself()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var architect = TestEntityProvider.Shared.Create<User>();
        var folder = TestEntityProvider.Shared.Create<AdrFolder>(x => x.OrganizationId = organization.Id);
        await Context.AddRangeAsync(organization,
            architect,
            folder,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = architect.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.Architect;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<UpdateAdrFolderModel>(x =>
        {
            x.Id = folder.Id;
            x.OrganizationId = organization.Id;
            x.ParentAdrFolderId = folder.Id;
            x.UserId = architect.Id;
        });

        // Act
        Func<Task> act = () => adrFolderManager.UpdateAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrInvalidOperationException>();
    }

    /// <summary>
    /// Обновление папки выдаёт ошибку: перемещение в свою вложенную папку
    /// </summary>
    [Fact]
    public async Task UpdateShouldThrowMoveToDescendant()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var architect = TestEntityProvider.Shared.Create<User>();
        var root = TestEntityProvider.Shared.Create<AdrFolder>(x => x.OrganizationId = organization.Id);
        var child = TestEntityProvider.Shared.Create<AdrFolder>(x =>
        {
            x.OrganizationId = organization.Id;
            x.ParentAdrFolderId = root.Id;
        });
        await Context.AddRangeAsync(organization,
            architect,
            root,
            child,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = architect.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.Architect;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<UpdateAdrFolderModel>(x =>
        {
            x.Id = root.Id;
            x.OrganizationId = organization.Id;
            x.ParentAdrFolderId = child.Id;
            x.UserId = architect.Id;
        });

        // Act
        Func<Task> act = () => adrFolderManager.UpdateAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrInvalidOperationException>();
    }

    /// <summary>
    /// Получение числа ADR в папке вместе с вложенными работает
    /// </summary>
    [Fact]
    public async Task GetAdrCountShouldWork()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var user = TestEntityProvider.Shared.Create<User>();
        var root = TestEntityProvider.Shared.Create<AdrFolder>(x => x.OrganizationId = organization.Id);
        var child = TestEntityProvider.Shared.Create<AdrFolder>(x =>
        {
            x.OrganizationId = organization.Id;
            x.ParentAdrFolderId = root.Id;
        });
        var otherFolder = TestEntityProvider.Shared.Create<AdrFolder>(x => x.OrganizationId = organization.Id);
        var adr1 = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.ParentAdrFolderId = root.Id;
        });
        var adr2 = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.ParentAdrFolderId = child.Id;
        });
        var adr3 = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.ParentAdrFolderId = otherFolder.Id;
        });
        var adr4 = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.ParentAdrFolderId = child.Id;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        await Context.AddRangeAsync(organization,
            user,
            root,
            child,
            otherFolder,
            adr1,
            adr2,
            adr3,
            adr4,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = user.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.User;
            }));
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await adrFolderManager.GetAdrCountAsync(organization.Id, root.Id, user.Id, CancellationToken.None);

        // Assert
        result.Should().Be(2);
    }

    /// <summary>
    /// Удаление папки с вложенными папками и ADR работает
    /// </summary>
    [Fact]
    public async Task DeleteShouldWork()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var architect = TestEntityProvider.Shared.Create<User>();
        var root = TestEntityProvider.Shared.Create<AdrFolder>(x => x.OrganizationId = organization.Id);
        var child = TestEntityProvider.Shared.Create<AdrFolder>(x =>
        {
            x.OrganizationId = organization.Id;
            x.ParentAdrFolderId = root.Id;
        });
        var otherFolder = TestEntityProvider.Shared.Create<AdrFolder>(x => x.OrganizationId = organization.Id);
        var adr1 = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.ParentAdrFolderId = root.Id;
        });
        var adr2 = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.ParentAdrFolderId = child.Id;
        });
        var adr3 = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.ParentAdrFolderId = otherFolder.Id;
        });
        await Context.AddRangeAsync(organization,
            architect,
            root,
            child,
            otherFolder,
            adr1,
            adr2,
            adr3,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = architect.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.Architect;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<DeleteAdrFolderModel>(x =>
        {
            x.AdrFolderId = root.Id;
            x.UserId = architect.Id;
        });

        // Act
        Func<Task> act = () => adrFolderManager.DeleteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        Context.Set<AdrFolder>().First(x => x.Id == root.Id).DeletedAt.Should().NotBeNull();
        Context.Set<AdrFolder>().First(x => x.Id == child.Id).DeletedAt.Should().NotBeNull();
        Context.Set<AdrFolder>().First(x => x.Id == otherFolder.Id).DeletedAt.Should().BeNull();
        Context.Set<Adr>().First(x => x.Id == adr1.Id).DeletedAt.Should().NotBeNull();
        Context.Set<Adr>().First(x => x.Id == adr2.Id).DeletedAt.Should().NotBeNull();
        Context.Set<Adr>().First(x => x.Id == adr3.Id).DeletedAt.Should().BeNull();
    }

    /// <summary>
    /// Удаление папки выдаёт ошибку: не хватает прав
    /// </summary>
    [Fact]
    public async Task DeleteShouldThrowDeny()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var user = TestEntityProvider.Shared.Create<User>();
        var folder = TestEntityProvider.Shared.Create<AdrFolder>(x => x.OrganizationId = organization.Id);
        await Context.AddRangeAsync(organization,
            user,
            folder,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = user.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.User;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<DeleteAdrFolderModel>(x =>
        {
            x.AdrFolderId = folder.Id;
            x.UserId = user.Id;
        });

        // Act
        Func<Task> act = () => adrFolderManager.DeleteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrAccessException>();
    }
}
