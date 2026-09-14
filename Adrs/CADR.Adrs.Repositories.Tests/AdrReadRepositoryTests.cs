using Ahatornn.TestGenerator;
using CADR.Administrations.Entities;
using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;

namespace CADR.Adrs.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="IAdrReadRepository"/>
/// </summary>
public class AdrReadRepositoryTests : CadrContextInMemory
{
    private readonly IAdrReadRepository adrReadRepository;
    private readonly TestEntityProvider entityProvider;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrReadRepositoryTests"/>
    /// </summary>
    public AdrReadRepositoryTests()
    {
        entityProvider = new TestEntityProviderBuilder()
            .AddPreset<Adr>(x =>
                x.Title = $"Adr{Guid.NewGuid():N}")
            .AddPreset<AdrFolder>(x =>
                x.Name = $"Folder{Guid.NewGuid():N}")
            .Build();
        adrReadRepository = new AdrReadRepository(Context);
    }

    /// <summary>
    /// Возвращает null если не находит ADR по Id
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnNull()
    {
        // Arrange
        var targetId = Guid.NewGuid();

        // Act
        var result = await adrReadRepository.GetActiveByIdAsync(targetId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает null если не находит ADR по Id т.к. он удалён
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnNullIfDeleted()
    {
        // Arrange
        var targetAdr = entityProvider.Create<Adr>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddAsync(targetAdr);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrReadRepository.GetActiveByIdAsync(targetAdr.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает ADR по Id
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnValue()
    {
        // Arrange
        var targetAdr = entityProvider.Create<Adr>();
        await Context.AddAsync(targetAdr);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrReadRepository.GetActiveByIdAsync(targetAdr.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(targetAdr, options => options
                .Excluding(o => o.Organization)
                .Excluding(o => o.ParentAdrFolder)
                .Excluding(o => o.Template)
                .Excluding(o => o.Sections)
                .Excluding(o => o.TargetLinks)
                .Excluding(o => o.Comments)
                .Excluding(o => o.Votes));
    }

    /// <summary>
    /// Возвращает пустой список ADR организации
    /// </summary>
    [Fact]
    public async Task GetByOrganizationIdShouldReturnEmpty()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();

        // Act
        var result = await adrReadRepository.GetByOrganizationIdAsync(targetOrganizationId, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Возвращает список ADR организации, исключая удалённые и ADR других организаций
    /// </summary>
    [Fact]
    public async Task GetByOrganizationIdShouldReturnValue()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();
        var otherOrganizationId = Guid.NewGuid();
        var targetAdr1 = entityProvider.Create<Adr>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.Number = 1;
        });
        var targetAdr2 = entityProvider.Create<Adr>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.Number = 2;
        });
        var deletedAdr = entityProvider.Create<Adr>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.Number = 3;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        var otherOrganizationAdr = entityProvider.Create<Adr>(x =>
        {
            x.OrganizationId = otherOrganizationId;
            x.Number = 1;
        });
        await Context.AddRangeAsync(targetAdr1, targetAdr2, deletedAdr, otherOrganizationAdr);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrReadRepository.GetByOrganizationIdAsync(targetOrganizationId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == targetAdr1.Id)
            .And.ContainSingle(x => x.Id == targetAdr2.Id);
    }

    /// <summary>
    /// Возвращает пустой список ADR папки
    /// </summary>
    [Fact]
    public async Task GetByFolderIdShouldReturnEmpty()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();
        var targetFolderId = Guid.NewGuid();

        // Act
        var result = await adrReadRepository.GetByFolderIdAsync(targetOrganizationId, targetFolderId, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Возвращает список ADR папки, исключая ADR других папок, удалённые и ADR других организаций
    /// </summary>
    [Fact]
    public async Task GetByFolderIdShouldReturnValue()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();
        var otherOrganizationId = Guid.NewGuid();
        var targetFolder = entityProvider.Create<AdrFolder>(x => x.OrganizationId = targetOrganizationId);
        var otherFolder = entityProvider.Create<AdrFolder>(x => x.OrganizationId = targetOrganizationId);
        var targetAdr = entityProvider.Create<Adr>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.ParentAdrFolderId = targetFolder.Id;
            x.Number = 1;
        });
        var deletedAdr = entityProvider.Create<Adr>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.ParentAdrFolderId = targetFolder.Id;
            x.Number = 2;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        var otherFolderAdr = entityProvider.Create<Adr>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.ParentAdrFolderId = otherFolder.Id;
            x.Number = 3;
        });
        var otherOrganizationAdr = entityProvider.Create<Adr>(x =>
        {
            x.OrganizationId = otherOrganizationId;
            x.ParentAdrFolderId = targetFolder.Id;
            x.Number = 1;
        });
        await Context.AddRangeAsync(targetFolder, otherFolder, targetAdr, deletedAdr, otherFolderAdr, otherOrganizationAdr);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrReadRepository.GetByFolderIdAsync(targetOrganizationId, targetFolder.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(1)
            .And.ContainSingle(x => x.Id == targetAdr.Id);
    }

    /// <summary>
    /// Возвращает список ADR без папки (в корне организации)
    /// </summary>
    [Fact]
    public async Task GetByFolderIdShouldReturnRootValue()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();
        var targetFolder = entityProvider.Create<AdrFolder>(x => x.OrganizationId = targetOrganizationId);
        var rootAdr = entityProvider.Create<Adr>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.Number = 1;
        });
        var folderAdr = entityProvider.Create<Adr>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.ParentAdrFolderId = targetFolder.Id;
            x.Number = 2;
        });
        await Context.AddRangeAsync(targetFolder, rootAdr, folderAdr);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrReadRepository.GetByFolderIdAsync(targetOrganizationId, null, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(1)
            .And.ContainSingle(x => x.Id == rootAdr.Id);
    }

    /// <summary>
    /// Возвращает список ADR автора в организации, исключая ADR других авторов, удалённые и ADR других организаций
    /// </summary>
    [Fact]
    public async Task GetByAuthorIdShouldReturnValue()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();
        var otherOrganizationId = Guid.NewGuid();
        var targetAuthorId = Guid.NewGuid();
        var otherAuthorId = Guid.NewGuid();
        var targetAdr = entityProvider.Create<Adr>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.AuthorId = targetAuthorId;
            x.Number = 1;
        });
        var deletedAdr = entityProvider.Create<Adr>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.AuthorId = targetAuthorId;
            x.Number = 2;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        var otherAuthorAdr = entityProvider.Create<Adr>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.AuthorId = otherAuthorId;
            x.Number = 3;
        });
        var otherOrganizationAdr = entityProvider.Create<Adr>(x =>
        {
            x.OrganizationId = otherOrganizationId;
            x.AuthorId = targetAuthorId;
            x.Number = 1;
        });
        await Context.AddRangeAsync(targetAdr, deletedAdr, otherAuthorAdr, otherOrganizationAdr);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrReadRepository.GetByAuthorIdAsync(targetOrganizationId, targetAuthorId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(1)
            .And.ContainSingle(x => x.Id == targetAdr.Id);
    }

    /// <summary>
    /// Возвращает 0 если в организации нет ADR
    /// </summary>
    [Fact]
    public async Task GetMaxNumberShouldReturnZeroForEmpty()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();

        // Act
        var result = await adrReadRepository.GetMaxNumberAsync(targetOrganizationId, CancellationToken.None);

        // Assert
        result.Should().Be(0);
    }

    /// <summary>
    /// Возвращает максимальный номер ADR организации, учитывая в том числе удалённые ADR,
    /// т.к. номера не должны использоваться повторно
    /// </summary>
    [Fact]
    public async Task GetMaxNumberShouldReturnMaxIncludingDeleted()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();
        var adr1 = entityProvider.Create<Adr>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.Number = 1;
        });
        var adr5 = entityProvider.Create<Adr>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.Number = 5;
        });
        var deletedAdr9 = entityProvider.Create<Adr>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.Number = 9;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        await Context.AddRangeAsync(adr1, adr5, deletedAdr9);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrReadRepository.GetMaxNumberAsync(targetOrganizationId, CancellationToken.None);

        // Assert
        result.Should().Be(9);
    }

    /// <summary>
    /// Проверяет, что ADR с указанным номером не существует в организации
    /// </summary>
    [Fact]
    public async Task IsActiveNumberExistsShouldReturnFalse()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();

        // Act
        var result = await adrReadRepository.IsActiveNumberExistsAsync(targetOrganizationId, 1, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Проверяет, что ADR с указанным номером не существует т.к. она удалена
    /// </summary>
    [Fact]
    public async Task IsActiveNumberExistsShouldReturnFalseIfDeleted()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();
        var deletedAdr = entityProvider.Create<Adr>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.Number = 1;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        await Context.AddAsync(deletedAdr);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrReadRepository.IsActiveNumberExistsAsync(targetOrganizationId, 1, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Проверяет, что ADR с указанным номером существует в организации
    /// </summary>
    [Fact]
    public async Task IsActiveNumberExistsShouldReturnTrue()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();
        var targetAdr = entityProvider.Create<Adr>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.Number = 1;
        });
        await Context.AddAsync(targetAdr);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrReadRepository.IsActiveNumberExistsAsync(targetOrganizationId, 1, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Возвращает число активных ADR в указанных папках, исключая удалённые
    /// </summary>
    [Fact]
    public async Task GetCountByFolderIdsShouldReturnCount()
    {
        // Arrange
        var targetFolder1 = entityProvider.Create<AdrFolder>();
        var targetFolder2 = entityProvider.Create<AdrFolder>();
        var folderAdr1 = entityProvider.Create<Adr>(x =>
        {
            x.ParentAdrFolderId = targetFolder1.Id;
            x.Number = 1;
        });
        var deletedFolderAdr = entityProvider.Create<Adr>(x =>
        {
            x.ParentAdrFolderId = targetFolder1.Id;
            x.Number = 2;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        var folderAdr2 = entityProvider.Create<Adr>(x =>
        {
            x.ParentAdrFolderId = targetFolder2.Id;
            x.Number = 3;
        });
        var rootAdr = entityProvider.Create<Adr>(x => x.Number = 4);
        await Context.AddRangeAsync(targetFolder1, targetFolder2, folderAdr1, deletedFolderAdr, folderAdr2, rootAdr);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrReadRepository.GetCountByFolderIdsAsync([targetFolder1.Id, targetFolder2.Id], CancellationToken.None);

        // Assert
        result.Should().Be(2);
    }
}
