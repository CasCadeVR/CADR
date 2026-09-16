using Ahatornn.TestGenerator;
using CADR.Administrations.Entities;
using CADR.Administrations.Entities.Enums;
using CADR.Adrs.Entities;
using CADR.Adrs.Services.Contracts.Exceptions;
using CADR.Adrs.Services.Contracts.Interfaces;
using CADR.Adrs.Services.Export;
using CADR.Adrs.Services.Tests.UnitOfWork;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;
using EntityEnums = CADR.Adrs.Entities.Enums;

namespace CADR.Adrs.Services.Tests;

/// <summary>
/// Тесты на <see cref="IAdrExportService"/>
/// </summary>
public class AdrExportServiceTests : CadrContextInMemory
{
    private readonly IAdrExportService adrExportService;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrExportServiceTests"/>
    /// </summary>
    public AdrExportServiceTests()
    {
        var unitOfWorkMock = new TestUnitOfWork(WriterContext, Context, Context);

        adrExportService = new AdrExportService(unitOfWorkMock.AdrUnitOfWork,
            unitOfWorkMock.UserOrganizationReadRepository,
            unitOfWorkMock.UserReadRepository);
    }

    /// <summary>
    /// Экспорт ADR в Markdown работает
    /// </summary>
    [Fact]
    public async Task ExportShouldWork()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var user = TestEntityProvider.Shared.Create<User>();
        var adr = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = user.Id;
            x.Status = EntityEnums.AdrStatus.Draft;
            x.Number = 3;
        });
        var section1 = TestEntityProvider.Shared.Create<AdrSection>(x =>
        {
            x.AdrId = adr.Id;
            x.Position = 1;
        });
        var section2 = TestEntityProvider.Shared.Create<AdrSection>(x =>
        {
            x.AdrId = adr.Id;
            x.Position = 2;
        });
        await Context.AddRangeAsync(organization,
            user,
            adr,
            section1,
            section2,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = user.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.User;
            }));
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await adrExportService.ExportToMarkdownAsync(adr.Id, user.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNullOrWhiteSpace()
            .And.Contain($"# {adr.Number}. {adr.Title}")
            .And.Contain($"**Статус:** {adr.Status}")
            .And.Contain($"**Автор:** {user.Name}")
            .And.Contain($"## {section1.Title}")
            .And.Contain(section1.Content)
            .And.Contain($"## {section2.Title}")
            .And.Contain(section2.Content);
        result.IndexOf(section1.Title, StringComparison.Ordinal).Should().BeLessThan(result.IndexOf(section2.Title, StringComparison.Ordinal));
    }

    /// <summary>
    /// Экспорт ADR выдаёт ошибку: ADR не найден
    /// </summary>
    [Fact]
    public async Task ExportShouldThrowNotFound()
    {
        //Arrange
        var adrId = Guid.NewGuid();

        // Act
        Func<Task> act = () => adrExportService.ExportToMarkdownAsync(adrId, Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<AdrEntityNotFoundException<Adr>>()
            .WithMessage($"*{adrId}*");
    }

    /// <summary>
    /// Экспорт ADR выдаёт ошибку: пользователь не состоит в организации
    /// </summary>
    [Fact]
    public async Task ExportShouldThrowNotMember()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var adr = TestEntityProvider.Shared.Create<Adr>(x => x.OrganizationId = organization.Id);
        await Context.AddRangeAsync(organization, adr);
        await UnitOfWork.SaveChangesAsync();

        // Act
        Func<Task> act = () => adrExportService.ExportToMarkdownAsync(adr.Id, Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrAccessException>();
    }

    /// <summary>
    /// Экспорт утверждённых ADR организации в общий документ работает
    /// </summary>
    [Fact]
    public async Task ExportApprovedShouldWork()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var user = TestEntityProvider.Shared.Create<User>();
        var approved1 = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = user.Id;
            x.Status = EntityEnums.AdrStatus.Approved;
            x.Number = 2;
        });
        var approved2 = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = user.Id;
            x.Status = EntityEnums.AdrStatus.Approved;
            x.Number = 1;
        });
        var draft = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = user.Id;
            x.Status = EntityEnums.AdrStatus.Draft;
            x.Number = 3;
        });
        var draftSection = TestEntityProvider.Shared.Create<AdrSection>(x =>
        {
            x.AdrId = draft.Id;
            x.Position = 1;
        });
        await Context.AddRangeAsync(organization,
            user,
            approved1,
            approved2,
            draft,
            draftSection,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = user.Id;
                x.OrganizationId = organization.Id;
                x.Role = Role.User;
            }));
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await adrExportService.ExportApprovedToMarkdownAsync(organization.Id, user.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNullOrWhiteSpace()
            .And.Contain($"# {approved2.Number}. {approved2.Title}")
            .And.Contain($"# {approved1.Number}. {approved1.Title}")
            .And.Contain("---")
            .And.NotContain(draft.Title)
            .And.NotContain(draftSection.Content);
        result.IndexOf(approved2.Title, StringComparison.Ordinal).Should().BeLessThan(result.IndexOf(approved1.Title, StringComparison.Ordinal));
    }

    /// <summary>
    /// Экспорт утверждённых ADR выдаёт ошибку: пользователь не состоит в организации
    /// </summary>
    [Fact]
    public async Task ExportApprovedShouldThrowNotMember()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        await Context.AddAsync(organization);
        await UnitOfWork.SaveChangesAsync();

        // Act
        Func<Task> act = () => adrExportService.ExportApprovedToMarkdownAsync(organization.Id, Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrAccessException>();
    }
}
