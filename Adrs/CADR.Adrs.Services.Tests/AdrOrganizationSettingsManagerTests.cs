using Ahatornn.TestGenerator;
using AutoMapper;
using CADR.Administrations.Entities;
using CADR.Administrations.Entities.Enums;
using CADR.Adrs.Entities;
using CADR.Adrs.Services.AutoMappers;
using CADR.Adrs.Services.Contracts.Exceptions;
using CADR.Adrs.Services.Contracts.Interfaces;
using CADR.Adrs.Services.Contracts.Models.Settings;
using CADR.Adrs.Services.Settings;
using CADR.Adrs.Services.Tests.UnitOfWork;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;

namespace CADR.Adrs.Services.Tests;

/// <summary>
/// Тесты на <see cref="IAdrOrganizationSettingsManager"/>
/// </summary>
public class AdrOrganizationSettingsManagerTests : CadrContextInMemory
{
    private readonly IAdrOrganizationSettingsManager adrOrganizationSettingsManager;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrOrganizationSettingsManagerTests"/>
    /// </summary>
    public AdrOrganizationSettingsManagerTests()
    {
        var config = new MapperConfiguration(opts =>
        {
            opts.AddProfile(new AdrServiceProfile());
        });
        var unitOfWorkMock = new TestUnitOfWork(WriterContext, Context, Context);

        adrOrganizationSettingsManager = new AdrOrganizationSettingsManager(unitOfWorkMock.AdrUnitOfWork,
            config.CreateMapper(),
            unitOfWorkMock.UserOrganizationReadRepository);
    }

    /// <summary>
    /// Получение настроек возвращает значения по умолчанию, если настройки отсутствуют
    /// </summary>
    [Fact]
    public async Task GetShouldReturnDefaults()
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

        // Act
        var result = await adrOrganizationSettingsManager.GetByOrganizationIdAsync(organization.Id, user.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { OrganizationId = organization.Id, LikesRequiredForApproval = 1, });
    }

    /// <summary>
    /// Получение настроек возвращает существующие настройки
    /// </summary>
    [Fact]
    public async Task GetShouldReturnValue()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var user = TestEntityProvider.Shared.Create<User>();
        var settings = TestEntityProvider.Shared.Create<AdrOrganizationSettings>(x =>
        {
            x.OrganizationId = organization.Id;
            x.LikesRequiredForApproval = 5;
        });
        await Context.AddRangeAsync(organization,
            user,
            settings,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = user.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.User;
            }));
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await adrOrganizationSettingsManager.GetByOrganizationIdAsync(organization.Id, user.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { settings.Id, OrganizationId = organization.Id, LikesRequiredForApproval = 5, });
    }

    /// <summary>
    /// Получение настроек выдаёт ошибку: пользователь не состоит в организации
    /// </summary>
    [Fact]
    public async Task GetShouldThrowNotMember()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var user = TestEntityProvider.Shared.Create<User>();
        await Context.AddRangeAsync(organization, user);
        await UnitOfWork.SaveChangesAsync();

        // Act
        Func<Task> act = () => adrOrganizationSettingsManager.GetByOrganizationIdAsync(organization.Id, user.Id, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrAccessException>();
    }

    /// <summary>
    /// Обновление настроек администратором создаёт настройки, если их не было
    /// </summary>
    [Fact]
    public async Task UpdateShouldCreate()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var admin = TestEntityProvider.Shared.Create<User>();
        await Context.AddRangeAsync(organization,
            admin,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = admin.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.Admin;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<UpdateAdrOrganizationSettingsModel>(x =>
        {
            x.OrganizationId = organization.Id;
            x.UserId = admin.Id;
            x.LikesRequiredForApproval = 3;
        });

        // Act
        var result = await adrOrganizationSettingsManager.UpdateAsync(model, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { OrganizationId = organization.Id, LikesRequiredForApproval = 3, });
        Context.Set<AdrOrganizationSettings>().Should()
            .ContainSingle(x => x.OrganizationId == organization.Id && x.LikesRequiredForApproval == 3);
    }

    /// <summary>
    /// Обновление настроек администратором работает
    /// </summary>
    [Fact]
    public async Task UpdateShouldWork()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var admin = TestEntityProvider.Shared.Create<User>();
        var settings = TestEntityProvider.Shared.Create<AdrOrganizationSettings>(x =>
        {
            x.OrganizationId = organization.Id;
            x.LikesRequiredForApproval = 5;
        });
        await Context.AddRangeAsync(organization,
            admin,
            settings,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = admin.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.Admin;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<UpdateAdrOrganizationSettingsModel>(x =>
        {
            x.OrganizationId = organization.Id;
            x.UserId = admin.Id;
            x.LikesRequiredForApproval = 2;
        });

        // Act
        var result = await adrOrganizationSettingsManager.UpdateAsync(model, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { settings.Id, OrganizationId = organization.Id, LikesRequiredForApproval = 2, });
        Context.Set<AdrOrganizationSettings>().Should()
            .ContainSingle(x => x.Id == settings.Id && x.LikesRequiredForApproval == 2);
    }

    /// <summary>
    /// Обновление настроек выдаёт ошибку: не хватает прав
    /// </summary>
    [Theory]
    [InlineData(Role.User)]
    [InlineData(Role.Architect)]
    public async Task UpdateShouldThrowDeny(Role role)
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
                x.Role = role;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<UpdateAdrOrganizationSettingsModel>(x =>
        {
            x.OrganizationId = organization.Id;
            x.UserId = user.Id;
        });

        // Act
        Func<Task> act = () => adrOrganizationSettingsManager.UpdateAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrAccessException>();
    }
}
