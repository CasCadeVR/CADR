using Ahatornn.TestGenerator;
using AutoMapper;
using CADR.Administrations.Entities;
using CADR.Administrations.Entities.Enums;
using CADR.Adrs.Entities;
using CADR.Adrs.Services.AutoMappers;
using CADR.Adrs.Services.Contracts.Exceptions;
using CADR.Adrs.Services.Contracts.Interfaces;
using CADR.Adrs.Services.Contracts.Models.Templates;
using CADR.Adrs.Services.Templates;
using CADR.Adrs.Services.Tests.UnitOfWork;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;

namespace CADR.Adrs.Services.Tests;

/// <summary>
/// Тесты на <see cref="IAdrTemplateManager"/>
/// </summary>
public class AdrTemplateManagerTests : CadrContextInMemory
{
    private readonly IAdrTemplateManager adrTemplateManager;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrTemplateManagerTests"/>
    /// </summary>
    public AdrTemplateManagerTests()
    {
        var config = new MapperConfiguration(opts =>
        {
            opts.AddProfile(new AdrServiceProfile());
        });
        var unitOfWorkMock = new TestUnitOfWork(WriterContext, Context, Context);

        adrTemplateManager = new AdrTemplateManager(unitOfWorkMock.AdrUnitOfWork,
            config.CreateMapper(),
            unitOfWorkMock.UserOrganizationReadRepository);
    }

    /// <summary>
    /// Создание шаблона архитектором работает
    /// </summary>
    [Fact]
    public async Task CreateShouldWork()
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
        var model = TestEntityProvider.Shared.Create<CreateAdrTemplateModel>(x =>
        {
            x.OrganizationId = organization.Id;
            x.UserId = architect.Id;
            x.Sections = new[]
            {
                new CreateAdrTemplateSectionModel { Position = 1, Title = $"T1{Guid.NewGuid()}", Hint = $"H1{Guid.NewGuid()}", Placeholder = $"P1{Guid.NewGuid()}", },
                new CreateAdrTemplateSectionModel { Position = 2, Title = $"T2{Guid.NewGuid()}", Hint = $"H2{Guid.NewGuid()}", Placeholder = $"P2{Guid.NewGuid()}", },
            };
        });

        // Act
        var result = await adrTemplateManager.CreateAsync(model, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new
            {
                model.Name,
                OrganizationId = (Guid?)model.OrganizationId,
                IsBuiltIn = false,
            });
        Context.Set<AdrTemplate>().Should().ContainSingle(x => x.Id == result.Id && x.OrganizationId == organization.Id);
        Context.Set<AdrTemplateSection>().Count(x => x.TemplateId == result.Id).Should().Be(2);
    }

    /// <summary>
    /// Создание шаблона выдаёт ошибку: не хватает прав
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
        var model = TestEntityProvider.Shared.Create<CreateAdrTemplateModel>(x =>
        {
            x.OrganizationId = organization.Id;
            x.UserId = user.Id;
        });

        // Act
        Func<Task> act = () => adrTemplateManager.CreateAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrAccessException>();
    }

    /// <summary>
    /// Получение встроенного шаблона работает без проверки членства в организации
    /// </summary>
    [Fact]
    public async Task GetByIdBuiltInShouldWork()
    {
        //Arrange
        var template = TestEntityProvider.Shared.Create<AdrTemplate>(x => x.OrganizationId = null);
        var section = TestEntityProvider.Shared.Create<AdrTemplateSection>(x => x.TemplateId = template.Id);
        await Context.AddRangeAsync(template, section);
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await adrTemplateManager.GetByIdAsync(template.Id, Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new
            {
                template.Id,
                template.Name,
                OrganizationId = (Guid?)null,
                IsBuiltIn = true,
            });
        result.Sections.Should().HaveCount(1);
    }

    /// <summary>
    /// Получение шаблона организации выдаёт ошибку: пользователь не состоит в организации
    /// </summary>
    [Fact]
    public async Task GetByIdShouldThrowNotMemberForOrgTemplate()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var template = TestEntityProvider.Shared.Create<AdrTemplate>(x => x.OrganizationId = organization.Id);
        await Context.AddRangeAsync(organization, template);
        await UnitOfWork.SaveChangesAsync();

        // Act
        Func<Task> act = () => adrTemplateManager.GetByIdAsync(template.Id, Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrAccessException>();
    }

    /// <summary>
    /// Получение доступных шаблонов возвращает глобальные и собственные шаблоны организации
    /// </summary>
    [Fact]
    public async Task GetAvailableShouldReturnGlobalAndOwn()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var otherOrganization = TestEntityProvider.Shared.Create<Organization>();
        var user = TestEntityProvider.Shared.Create<User>();
        var globalTemplate = TestEntityProvider.Shared.Create<AdrTemplate>(x => x.OrganizationId = null);
        var ownTemplate = TestEntityProvider.Shared.Create<AdrTemplate>(x => x.OrganizationId = organization.Id);
        var otherTemplate = TestEntityProvider.Shared.Create<AdrTemplate>(x => x.OrganizationId = otherOrganization.Id);
        var deletedTemplate = TestEntityProvider.Shared.Create<AdrTemplate>(x =>
        {
            x.OrganizationId = organization.Id;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        await Context.AddRangeAsync(organization,
            otherOrganization,
            user,
            globalTemplate,
            ownTemplate,
            otherTemplate,
            deletedTemplate,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = user.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.User;
            }));
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await adrTemplateManager.GetAvailableForOrganizationAsync(organization.Id, user.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == globalTemplate.Id && x.IsBuiltIn)
            .And.ContainSingle(x => x.Id == ownTemplate.Id && !x.IsBuiltIn);
    }

    /// <summary>
    /// Обновление встроенного шаблона выдаёт ошибку
    /// </summary>
    [Fact]
    public async Task UpdateBuiltInShouldThrow()
    {
        //Arrange
        var template = TestEntityProvider.Shared.Create<AdrTemplate>(x => x.OrganizationId = null);
        await Context.AddAsync(template);
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<UpdateAdrTemplateModel>(x =>
        {
            x.Id = template.Id;
            x.OrganizationId = null;
        });

        // Act
        Func<Task> act = () => adrTemplateManager.UpdateAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrInvalidOperationException>();
    }

    /// <summary>
    /// Обновление шаблона организации архитектором работает и заменяет разделы
    /// </summary>
    [Fact]
    public async Task UpdateShouldWork()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var architect = TestEntityProvider.Shared.Create<User>();
        var template = TestEntityProvider.Shared.Create<AdrTemplate>(x => x.OrganizationId = organization.Id);
        var oldSection = TestEntityProvider.Shared.Create<AdrTemplateSection>(x => x.TemplateId = template.Id);
        await Context.AddRangeAsync(organization,
            architect,
            template,
            oldSection,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = architect.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.Architect;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<UpdateAdrTemplateModel>(x =>
        {
            x.Id = template.Id;
            x.OrganizationId = organization.Id;
            x.UserId = architect.Id;
            x.Sections = new[]
            {
                new AdrTemplateSectionModel { Position = 1, Title = $"T1{Guid.NewGuid()}", Hint = $"H1{Guid.NewGuid()}", Placeholder = $"P1{Guid.NewGuid()}", TemplateId = template.Id, },
            };
        });

        // Act
        var result = await adrTemplateManager.UpdateAsync(model, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { model.Name, });
        result.Sections.Should().HaveCount(1);
        Context.Set<AdrTemplateSection>().First(x => x.Id == oldSection.Id).DeletedAt.Should().NotBeNull();
        Context.Set<AdrTemplateSection>().Count(x => x.TemplateId == template.Id && x.DeletedAt == null).Should().Be(1);
    }

    /// <summary>
    /// Удаление встроенного шаблона выдаёт ошибку
    /// </summary>
    [Fact]
    public async Task DeleteBuiltInShouldThrow()
    {
        //Arrange
        var template = TestEntityProvider.Shared.Create<AdrTemplate>(x => x.OrganizationId = null);
        await Context.AddAsync(template);
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<DeleteAdrTemplateModel>(x => x.AdrTemplateId = template.Id);

        // Act
        Func<Task> act = () => adrTemplateManager.DeleteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrInvalidOperationException>();
    }

    /// <summary>
    /// Удаление шаблона организации архитектором работает
    /// </summary>
    [Fact]
    public async Task DeleteShouldWork()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var architect = TestEntityProvider.Shared.Create<User>();
        var template = TestEntityProvider.Shared.Create<AdrTemplate>(x => x.OrganizationId = organization.Id);
        var section = TestEntityProvider.Shared.Create<AdrTemplateSection>(x => x.TemplateId = template.Id);
        await Context.AddRangeAsync(organization,
            architect,
            template,
            section,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = architect.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.Architect;
            }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<DeleteAdrTemplateModel>(x =>
        {
            x.AdrTemplateId = template.Id;
            x.UserId = architect.Id;
        });

        // Act
        Func<Task> act = () => adrTemplateManager.DeleteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        Context.Set<AdrTemplate>().First(x => x.Id == template.Id).DeletedAt.Should().NotBeNull();
        Context.Set<AdrTemplateSection>().First(x => x.Id == section.Id).DeletedAt.Should().NotBeNull();
    }
}
