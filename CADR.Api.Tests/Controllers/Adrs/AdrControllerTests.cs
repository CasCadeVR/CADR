using Ahatornn.TestGenerator;
using CADR.Administrations.Entities;
using CADR.Administrations.Entities.Enums;
using CADR.Api.Client;
using CADR.Api.Tests.Infrastructures;
using CADR.Common.Mvc.Models;
using FluentAssertions;
using Xunit;

namespace CADR.Api.Tests.Controllers.Adrs;

/// <summary>
/// Тесты сценариев работы с ADR
/// </summary>
[Collection(nameof(CadrApiAuthorizedUserTestCollection))]
public class AdrControllerTests
{
    private readonly CadrApiAuthorizedUserFixture fixture;
    private readonly ICadrApiClient apiClient;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrControllerTests"/>
    /// </summary>
    public AdrControllerTests(CadrApiAuthorizedUserFixture fixture)
    {
        this.fixture = fixture;
        apiClient = fixture.CreateApiClient();
    }

    /// <summary>
    /// Создаёт ADR и получает его по идентификатору
    /// </summary>
    [Fact]
    public async Task AdrCreateAndGetShouldReturnValue()
    {
        // Arrange
        var organization = await apiClient.OrganizationCreateAsync(new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        });
        var createModel = new CreateAdrApiModel
        {
            Title = $"Title{Guid.NewGuid():N}",
            Status = AdrStatusApi.Draft,
            OrganizationId = organization.Id,
        };

        // Act
        var createResult = await apiClient.AdrCreateAsync(createModel);
        var getResult = await apiClient.AdrGetAsync(createResult.Id);

        // Assert
        createResult.Should().NotBeNull();
        createResult.Number.Should().Be(1);
        createResult.Title.Should().Be(createModel.Title);
        createResult.Status.Should().Be(AdrStatusApi.Draft);
        createResult.Score.Should().Be(0);
        createResult.OrganizationId.Should().Be(organization.Id);
        createResult.AuthorId.Should().Be(fixture.PersonalOptions.Identifier);
        createResult.AuthorName.Should().Be(fixture.PersonalOptions.Name);
        createResult.AuthorLogin.Should().Be(fixture.PersonalOptions.Login);
        createResult.ParentAdrFolderId.Should().BeNull();
        createResult.FolderPath.Should().BeNullOrEmpty();
        createResult.Sections.Should().BeNullOrEmpty();
        getResult.Should().NotBeNull();
        getResult.Id.Should().Be(createResult.Id);
        getResult.Title.Should().Be(createModel.Title);
        getResult.Status.Should().Be(AdrStatusApi.Draft);
        getResult.Score.Should().Be(0);
        getResult.AuthorName.Should().Be(fixture.PersonalOptions.Name);
        getResult.AuthorLogin.Should().Be(fixture.PersonalOptions.Login);
        getResult.FolderPath.Should().BeNullOrEmpty();
        getResult.Sections.Should().BeNullOrEmpty();
    }

    /// <summary>
    /// Создаёт ADR, обновляет его вместе с разделами и получает по идентификатору
    /// </summary>
    [Fact]
    public async Task AdrCreateUpdateAndGetShouldReturnValue()
    {
        // Arrange
        var organization = await apiClient.OrganizationCreateAsync(new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        });
        var created = await apiClient.AdrCreateAsync(new CreateAdrApiModel
        {
            Title = $"Title{Guid.NewGuid():N}",
            Status = AdrStatusApi.Draft,
            OrganizationId = organization.Id,
        });
        var updateModel = new UpdateAdrApiModel
        {
            Id = created.Id,
            Number = created.Number,
            Title = $"NewTitle{Guid.NewGuid():N}",
            Status = AdrStatusApi.Draft,
            OrganizationId = organization.Id,
            Sections = new List<AdrSectionApiModel>
            {
                new() { Position = 1, Title = $"Section1{Guid.NewGuid():N}", Content = $"Content1{Guid.NewGuid():N}" },
                new() { Position = 2, Title = $"Section2{Guid.NewGuid():N}", Content = $"Content2{Guid.NewGuid():N}" },
            },
        };

        // Act
        var updateResult = await apiClient.AdrUpdateAsync(updateModel);
        var getResult = await apiClient.AdrGetAsync(created.Id);

        // Assert
        updateResult.Should().NotBeNull();
        updateResult.Id.Should().Be(created.Id);
        updateResult.Title.Should().Be(updateModel.Title);
        updateResult.Sections.Should().BeEquivalentTo(updateModel.Sections, options => options
            .Excluding(x => x.Id)
            .Excluding(x => x.AdrId)
            .Excluding(x => x.Hint)
            .Excluding(x => x.Placeholder)
            .WithStrictOrdering());
        updateResult.Sections.Should().OnlyContain(x => x.AdrId == created.Id && x.Id != Guid.Empty);
        getResult.Should().NotBeNull();
        getResult.Title.Should().Be(updateModel.Title);
        getResult.Sections.Should().BeEquivalentTo(updateModel.Sections, options => options
            .Excluding(x => x.Id)
            .Excluding(x => x.AdrId)
            .Excluding(x => x.Hint)
            .Excluding(x => x.Placeholder)
            .WithStrictOrdering());
    }

    /// <summary>
    /// Создаёт ADR по шаблону и проверяет копирование разделов шаблона
    /// </summary>
    [Fact]
    public async Task AdrCreateFromTemplateShouldCopySections()
    {
        // Arrange
        var organization = await apiClient.OrganizationCreateAsync(new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        });
        var template = await apiClient.AdrTemplateCreateAsync(new CreateAdrTemplateApiModel
        {
            Name = $"Template{Guid.NewGuid():N}",
            OrganizationId = organization.Id,
            Sections = new List<CreateAdrTemplateSectionApiModel>
            {
                new() { Position = 1, Title = "Context", Hint = "ContextHint", Placeholder = "ContextPlaceholder" },
                new() { Position = 2, Title = "Decision", Hint = "DecisionHint", Placeholder = "DecisionPlaceholder" },
            },
        });
        var createModel = new CreateAdrApiModel
        {
            Title = $"Title{Guid.NewGuid():N}",
            Status = AdrStatusApi.Draft,
            OrganizationId = organization.Id,
            TemplateId = template.Id,
        };

        // Act
        var createResult = await apiClient.AdrCreateAsync(createModel);
        var getResult = await apiClient.AdrGetAsync(createResult.Id);

        // Assert
        createResult.TemplateId.Should().Be(template.Id);
        createResult.Sections.Should().HaveCount(2);
        createResult.Sections.Should().BeEquivalentTo(template.Sections, options => options
            .Excluding(x => x.Id)
            .Excluding(x => x.TemplateId)
            .WithStrictOrdering());
        createResult.Sections.Should().OnlyContain(x => x.Content == null);
        getResult.Sections.Should().BeEquivalentTo(template.Sections, options => options
            .Excluding(x => x.Id)
            .Excluding(x => x.TemplateId)
            .WithStrictOrdering());
    }

    /// <summary>
    /// Меняет статус ADR с черновика на предложенный
    /// </summary>
    [Fact]
    public async Task AdrChangeStatusShouldWork()
    {
        // Arrange
        var organization = await apiClient.OrganizationCreateAsync(new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        });
        var adr = await apiClient.AdrCreateAsync(new CreateAdrApiModel
        {
            Title = $"Title{Guid.NewGuid():N}",
            Status = AdrStatusApi.Draft,
            OrganizationId = organization.Id,
        });

        // Act
        await apiClient.AdrChangeStatusAsync(adr.Id, new ChangeAdrStatusApiModel
        {
            Status = AdrStatusApi.Proposed,
        });
        var getResult = await apiClient.AdrGetAsync(adr.Id);

        // Assert
        getResult.Status.Should().Be(AdrStatusApi.Proposed);
        getResult.Score.Should().Be(0);
    }

    /// <summary>
    /// Получает ограниченный список недавно добавленных ADR
    /// </summary>
    [Fact]
    public async Task AdrGetRecentShouldReturnLimitedList()
    {
        // Arrange
        var organization = await apiClient.OrganizationCreateAsync(new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        });
        var first = await apiClient.AdrCreateAsync(new CreateAdrApiModel
        {
            Title = $"Title{Guid.NewGuid():N}",
            Status = AdrStatusApi.Draft,
            OrganizationId = organization.Id,
        });
        var second = await apiClient.AdrCreateAsync(new CreateAdrApiModel
        {
            Title = $"Title{Guid.NewGuid():N}",
            Status = AdrStatusApi.Draft,
            OrganizationId = organization.Id,
        });

        // Act
        var recent = await apiClient.AdrGetRecentAsync(organization.Id, 1);

        // Assert
        recent.Should().ContainSingle();
        new[] { first.Id, second.Id }.Should().Contain(recent.Single().Id);
    }

    /// <summary>
    /// Получает ADR организации, отфильтрованные по статусам
    /// </summary>
    [Fact]
    public async Task AdrGetByStatusesShouldFilter()
    {
        // Arrange
        var organization = await apiClient.OrganizationCreateAsync(new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        });
        var draft = await apiClient.AdrCreateAsync(new CreateAdrApiModel
        {
            Title = $"Title{Guid.NewGuid():N}",
            Status = AdrStatusApi.Draft,
            OrganizationId = organization.Id,
        });
        var proposed = await apiClient.AdrCreateAsync(new CreateAdrApiModel
        {
            Title = $"Title{Guid.NewGuid():N}",
            Status = AdrStatusApi.Draft,
            OrganizationId = organization.Id,
        });
        await apiClient.AdrChangeStatusAsync(proposed.Id, new ChangeAdrStatusApiModel
        {
            Status = AdrStatusApi.Proposed,
        });

        // Act
        var proposedResult = await apiClient.AdrGetByStatusesAsync(organization.Id, new[] { AdrStatusApi.Proposed });
        var draftResult = await apiClient.AdrGetByStatusesAsync(organization.Id, new[] { AdrStatusApi.Draft });

        // Assert
        proposedResult.Should().ContainSingle(x => x.Id == proposed.Id);
        proposedResult.Should().NotContain(x => x.Id == draft.Id);
        draftResult.Should().ContainSingle(x => x.Id == draft.Id);
        draftResult.Should().NotContain(x => x.Id == proposed.Id);
    }

    /// <summary>
    /// Получает ADR указанного автора организации
    /// </summary>
    [Fact]
    public async Task AdrGetByAuthorShouldReturnOnlyAuthorAdrs()
    {
        // Arrange
        var organization = await apiClient.OrganizationCreateAsync(new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        });
        var ownAdr = await apiClient.AdrCreateAsync(new CreateAdrApiModel
        {
            Title = $"Title{Guid.NewGuid():N}",
            Status = AdrStatusApi.Draft,
            OrganizationId = organization.Id,
        });
        var secondOptions = await SeedSecondMemberAsync(organization.Id, Role.Architect);
        var secondClient = fixture.CreateApiClient(secondOptions);
        var secondAdr = await secondClient.AdrCreateAsync(new CreateAdrApiModel
        {
            Title = $"Title{Guid.NewGuid():N}",
            Status = AdrStatusApi.Draft,
            OrganizationId = organization.Id,
        });

        // Act
        var result = await apiClient.AdrGetByAuthorAsync(organization.Id, secondOptions.Identifier);

        // Assert
        result.Should().ContainSingle(x => x.Id == secondAdr.Id);
        result.Should().NotContain(x => x.Id == ownAdr.Id);
        result.Single().AuthorId.Should().Be(secondOptions.Identifier);
        result.Single().AuthorName.Should().Be(secondOptions.Name);
    }

    /// <summary>
    /// Создаёт ADR, удаляет его и проверяет пустой список организации
    /// </summary>
    [Fact]
    public async Task AdrCreateAndDeleteShouldRemoveFromOrganization()
    {
        // Arrange
        var organization = await apiClient.OrganizationCreateAsync(new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        });
        var adr = await apiClient.AdrCreateAsync(new CreateAdrApiModel
        {
            Title = $"Title{Guid.NewGuid():N}",
            Status = AdrStatusApi.Draft,
            OrganizationId = organization.Id,
        });

        // Act
        var beforeDelete = await apiClient.AdrGetByOrganizationAsync(organization.Id);
        await apiClient.AdrDeleteAsync(adr.Id);
        var afterDelete = await apiClient.AdrGetByOrganizationAsync(organization.Id);

        // Assert
        beforeDelete.Should().ContainSingle(x => x.Id == adr.Id);
        afterDelete.Should().BeEmpty();
    }

    /// <summary>
    /// Добавляет в контекст второго участника организации
    /// </summary>
    private async Task<PersonalOptions> SeedSecondMemberAsync(Guid organizationId, Role role)
    {
        var options = new PersonalOptions
        {
            Identifier = Guid.NewGuid(),
            Email = $"Email{Guid.NewGuid():N}",
            Login = $"Login{Guid.NewGuid():N}",
            Name = $"Name{Guid.NewGuid():N}",
            SecurityStamp = Guid.NewGuid().ToString(),
            Params = new Dictionary<string, string>(),
        };
        fixture.CadrContext.Add(TestEntityProvider.Shared.Create<User>(x =>
        {
            x.Id = options.Identifier;
            x.Email = options.Email;
            x.Login = options.Login;
            x.Name = options.Name;
            x.SecurityStamp = options.SecurityStamp;
            x.EmailLowerCase = x.Email.ToLower();
            x.LoginLowerCase = x.Login.ToLower();
        }));
        fixture.CadrContext.Add(TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.UserId = options.Identifier;
            x.OrganizationId = organizationId;
            x.Role = role;
        }));
        await fixture.UnitOfWork.SaveChangesAsync();
        return options;
    }
}
