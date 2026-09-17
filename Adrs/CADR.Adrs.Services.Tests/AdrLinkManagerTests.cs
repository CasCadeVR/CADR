using Ahatornn.TestGenerator;
using AutoMapper;
using CADR.Administrations.Entities;
using CADR.Administrations.Entities.Enums;
using CADR.Adrs.Entities;
using CADR.Adrs.Services.AutoMappers;
using CADR.Adrs.Services.Contracts.Exceptions;
using CADR.Adrs.Services.Contracts.Interfaces;
using CADR.Adrs.Services.Contracts.Models.Links;
using CADR.Adrs.Services.Links;
using CADR.Adrs.Services.Tests.UnitOfWork;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;
using ContractEnums = CADR.Adrs.Services.Contracts.Models.Enums;

namespace CADR.Adrs.Services.Tests;

/// <summary>
/// Тесты на <see cref="IAdrLinkManager"/>
/// </summary>
public class AdrLinkManagerTests : CadrContextInMemory
{
    private readonly IAdrLinkManager adrLinkManager;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrLinkManagerTests"/>
    /// </summary>
    public AdrLinkManagerTests()
    {
        var config = new MapperConfiguration(opts =>
        {
            opts.AddProfile(new AdrServiceProfile());
        });
        var unitOfWorkMock = new TestUnitOfWork(WriterContext, Context, Context);

        adrLinkManager = new AdrLinkManager(unitOfWorkMock.AdrUnitOfWork,
            config.CreateMapper(),
            unitOfWorkMock.UserOrganizationReadRepository);
    }

    /// <summary>
    /// Создание связи архитектором работает
    /// </summary>
    [Fact]
    public async Task CreateShouldWork()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var architect = TestEntityProvider.Shared.Create<User>();
        var sourceAdr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        var targetAdr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        await Context.AddRangeAsync(organization,
            architect,
            sourceAdr,
            targetAdr,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = architect.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.Architect;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<CreateAdrLinkModel>(x =>
        {
            x.SourceAdrId = sourceAdr.Id;
            x.TargetAdrId = targetAdr.Id;
            x.UserId = architect.Id;
        });

        // Act
        var result = await adrLinkManager.CreateAsync(model, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new
            {
                model.Type,
                model.SourceAdrId,
                model.TargetAdrId,
                TargetAdrNumber = targetAdr.Number,
                TargetAdrName = targetAdr.Title,
            });
        Context.Set<AdrLink>().Should()
            .ContainSingle(x => x.SourceAdrId == sourceAdr.Id && x.TargetAdrId == targetAdr.Id);
    }

    /// <summary>
    /// Создание связи выдаёт ошибку: источник не найден
    /// </summary>
    [Fact]
    public async Task CreateShouldThrowSourceNotFound()
    {
        //Arrange
        var model = TestEntityProvider.Shared.Create<CreateAdrLinkModel>();

        // Act
        Func<Task> act = () => adrLinkManager.CreateAsync(model, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<AdrEntityNotFoundException<Adr>>()
            .WithMessage($"*{model.SourceAdrId}*");
    }

    /// <summary>
    /// Создание связи выдаёт ошибку: указываемый ADR не найден
    /// </summary>
    [Fact]
    public async Task CreateShouldThrowTargetNotFound()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var architect = TestEntityProvider.Shared.Create<User>();
        var sourceAdr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        await Context.AddRangeAsync(organization,
            architect,
            sourceAdr,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = architect.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.Architect;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<CreateAdrLinkModel>(x =>
        {
            x.SourceAdrId = sourceAdr.Id;
            x.UserId = architect.Id;
        });

        // Act
        Func<Task> act = () => adrLinkManager.CreateAsync(model, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<AdrEntityNotFoundException<Adr>>()
            .WithMessage($"*{model.TargetAdrId}*");
    }

    /// <summary>
    /// Создание связи выдаёт ошибку: ADR из разных организаций
    /// </summary>
    [Fact]
    public async Task CreateShouldThrowCrossOrganization()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var otherOrganization = TestEntityProvider.Shared.Create<Organization>();
        var architect = TestEntityProvider.Shared.Create<User>();
        var sourceAdr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        var targetAdr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = otherOrganization.Id);
        await Context.AddRangeAsync(organization,
            otherOrganization,
            architect,
            sourceAdr,
            targetAdr,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = architect.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.Architect;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<CreateAdrLinkModel>(x =>
        {
            x.SourceAdrId = sourceAdr.Id;
            x.TargetAdrId = targetAdr.Id;
            x.UserId = architect.Id;
        });

        // Act
        Func<Task> act = () => adrLinkManager.CreateAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrInvalidOperationException>();
    }

    /// <summary>
    /// Создание связи выдаёт ошибку: связь уже существует
    /// </summary>
    [Fact]
    public async Task CreateShouldThrowDuplicate()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var architect = TestEntityProvider.Shared.Create<User>();
        var sourceAdr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        var targetAdr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        var link = TestEntityProvider.Shared.Create<AdrLink>(x =>
        {
            x.SourceAdrId = sourceAdr.Id;
            x.TargetAdrId = targetAdr.Id;
            x.Type = Entities.Enums.AdrLinkType.RelatedTo;
        });
        await Context.AddRangeAsync(organization,
            architect,
            sourceAdr,
            targetAdr,
            link,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = architect.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.Architect;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<CreateAdrLinkModel>(x =>
        {
            x.SourceAdrId = sourceAdr.Id;
            x.TargetAdrId = targetAdr.Id;
            x.UserId = architect.Id;
            x.Type = ContractEnums.AdrLinkType.RelatedTo;
        });

        // Act
        Func<Task> act = () => adrLinkManager.CreateAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrInvalidOperationException>();
    }

    /// <summary>
    /// Создание связи выдаёт ошибку: не хватает прав
    /// </summary>
    [Fact]
    public async Task CreateShouldThrowDeny()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var user = TestEntityProvider.Shared.Create<User>();
        var sourceAdr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        var targetAdr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        await Context.AddRangeAsync(organization,
            user,
            sourceAdr,
            targetAdr,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = user.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.User;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<CreateAdrLinkModel>(x =>
        {
            x.SourceAdrId = sourceAdr.Id;
            x.TargetAdrId = targetAdr.Id;
            x.UserId = user.Id;
        });

        // Act
        Func<Task> act = () => adrLinkManager.CreateAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrAccessException>();
    }

    /// <summary>
    /// Получение связей возвращает связи в обе стороны
    /// </summary>
    [Fact]
    public async Task GetByAdrIdShouldReturnLinksBothDirections()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var user = TestEntityProvider.Shared.Create<User>();
        var adr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        var sourceAdr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        var targetAdr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        var link1 = TestEntityProvider.Shared.Create<AdrLink>(x =>
        {
            x.SourceAdrId = adr.Id;
            x.TargetAdrId = targetAdr.Id;
        });
        var link2 = TestEntityProvider.Shared.Create<AdrLink>(x =>
        {
            x.SourceAdrId = sourceAdr.Id;
            x.TargetAdrId = adr.Id;
        });
        var link3 = TestEntityProvider.Shared.Create<AdrLink>(x =>
        {
            x.SourceAdrId = sourceAdr.Id;
            x.TargetAdrId = targetAdr.Id;
        });
        var link4 = TestEntityProvider.Shared.Create<AdrLink>(x =>
        {
            x.SourceAdrId = adr.Id;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        await Context.AddRangeAsync(organization,
            user,
            adr,
            sourceAdr,
            targetAdr,
            link1,
            link2,
            link3,
            link4,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = user.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.User;
            }));
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await adrLinkManager.GetByAdrIdAsync(adr.Id, user.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == link1.Id)
            .And.ContainSingle(x => x.Id == link2.Id);
    }

    /// <summary>
    /// Удаление связи архитектором работает
    /// </summary>
    [Fact]
    public async Task DeleteShouldWork()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var architect = TestEntityProvider.Shared.Create<User>();
        var sourceAdr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        var targetAdr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        var link = TestEntityProvider.Shared.Create<AdrLink>(x =>
        {
            x.SourceAdrId = sourceAdr.Id;
            x.TargetAdrId = targetAdr.Id;
        });
        await Context.AddRangeAsync(organization,
            architect,
            sourceAdr,
            targetAdr,
            link,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = architect.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.Architect;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<DeleteAdrLinkModel>(x =>
        {
            x.AdrLinkId = link.Id;
            x.UserId = architect.Id;
        });

        // Act
        Func<Task> act = () => adrLinkManager.DeleteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        Context.Set<AdrLink>().First(x => x.Id == link.Id).DeletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Удаление связи выдаёт ошибку: связь не найдена
    /// </summary>
    [Fact]
    public async Task DeleteShouldThrowNotFound()
    {
        //Arrange
        var model = TestEntityProvider.Shared.Create<DeleteAdrLinkModel>();

        // Act
        Func<Task> act = () => adrLinkManager.DeleteAsync(model, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<AdrEntityNotFoundException<AdrLink>>()
            .WithMessage($"*{model.AdrLinkId}*");
    }
}
