using Ahatornn.TestGenerator;
using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;

namespace CADR.Adrs.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="IAdrFolderReadRepository"/>
/// </summary>
public class AdrFolderReadRepositoryTests : CadrContextInMemory
{
    private readonly IAdrFolderReadRepository adrFolderReadRepository;
    private readonly TestEntityProvider entityProvider;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrFolderReadRepositoryTests"/>
    /// </summary>
    public AdrFolderReadRepositoryTests()
    {
        entityProvider = new TestEntityProviderBuilder()
            .AddPreset<AdrFolder>(x =>
                x.Name = $"Folder{Guid.NewGuid():N}")
            .Build();
        adrFolderReadRepository = new AdrFolderReadRepository(Context);
    }

    /// <summary>
    /// Возвращает null если не находит папку по Id
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnNull()
    {
        // Arrange
        var targetId = Guid.NewGuid();

        // Act
        var result = await adrFolderReadRepository.GetActiveByIdAsync(targetId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает null если не находит папку по Id т.к. она удалена
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnNullIfDeleted()
    {
        // Arrange
        var targetFolder = entityProvider.Create<AdrFolder>(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddAsync(targetFolder);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrFolderReadRepository.GetActiveByIdAsync(targetFolder.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает папку по Id
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnValue()
    {
        // Arrange
        var targetFolder = entityProvider.Create<AdrFolder>();
        await Context.AddAsync(targetFolder);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrFolderReadRepository.GetActiveByIdAsync(targetFolder.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(targetFolder, options => options
                .Excluding(o => o.Organization)
                .Excluding(o => o.ParentAdrFolder)
                .Excluding(o => o.ChildFolder)
                .Excluding(o => o.Adrs));
    }

    /// <summary>
    /// Возвращает пустой список папок организации
    /// </summary>
    [Fact]
    public async Task GetByOrganizationIdShouldReturnEmpty()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();

        // Act
        var result = await adrFolderReadRepository.GetByOrganizationIdAsync(targetOrganizationId, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Возвращает список папок организации, исключая удалённые и папки других организаций
    /// </summary>
    [Fact]
    public async Task GetByOrganizationIdShouldReturnValue()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();
        var otherOrganizationId = Guid.NewGuid();
        var targetFolder1 = entityProvider.Create<AdrFolder>(x => x.OrganizationId = targetOrganizationId);
        var targetFolder2 = entityProvider.Create<AdrFolder>(x => x.OrganizationId = targetOrganizationId);
        var deletedFolder = entityProvider.Create<AdrFolder>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        var otherOrganizationFolder = entityProvider.Create<AdrFolder>(x => x.OrganizationId = otherOrganizationId);
        await Context.AddRangeAsync(targetFolder1, targetFolder2, deletedFolder, otherOrganizationFolder);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrFolderReadRepository.GetByOrganizationIdAsync(targetOrganizationId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == targetFolder1.Id)
            .And.ContainSingle(x => x.Id == targetFolder2.Id);
    }

    /// <summary>
    /// Возвращает список папок организации отсортированный по имени
    /// </summary>
    [Fact]
    public async Task GetByOrganizationIdShouldReturnOrderedByName()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();
        var folderC = entityProvider.Create<AdrFolder>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.Name = "ccc";
        });
        var folderA = entityProvider.Create<AdrFolder>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.Name = "aaa";
        });
        var folderB = entityProvider.Create<AdrFolder>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.Name = "bbb";
        });
        await Context.AddRangeAsync(folderC, folderA, folderB);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrFolderReadRepository.GetByOrganizationIdAsync(targetOrganizationId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(3)
            .And.ContainInOrder(folderA, folderB, folderC);
    }

    /// <summary>
    /// Проверяет, что папка с указанным именем не существует
    /// </summary>
    [Fact]
    public async Task IsActiveNameExistsShouldReturnFalse()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();
        var targetParentFolderId = Guid.NewGuid();

        // Act
        var result = await adrFolderReadRepository.IsActiveNameExistsAsync(targetOrganizationId,
            targetParentFolderId,
            $"CheckIfFolderExistShouldReturnFalse{Guid.NewGuid():N}",
            CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Проверяет, что папка с указанным именем не существует т.к. она удалена
    /// </summary>
    [Fact]
    public async Task IsActiveNameExistsShouldReturnFalseIfDeleted()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();
        var targetParentFolderId = Guid.NewGuid();
        var deletedFolder = entityProvider.Create<AdrFolder>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.ParentAdrFolderId = targetParentFolderId;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        await Context.AddAsync(deletedFolder);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrFolderReadRepository.IsActiveNameExistsAsync(targetOrganizationId,
            targetParentFolderId,
            deletedFolder.Name,
            CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Проверяет, что папка с указанным именем существует у родителя
    /// </summary>
    [Fact]
    public async Task IsActiveNameExistsShouldReturnTrue()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();
        var targetParentFolderId = Guid.NewGuid();
        var targetFolder = entityProvider.Create<AdrFolder>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.ParentAdrFolderId = targetParentFolderId;
        });
        await Context.AddAsync(targetFolder);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrFolderReadRepository.IsActiveNameExistsAsync(targetOrganizationId,
            targetParentFolderId,
            targetFolder.Name,
            CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Проверяет, что имя папки сравнивается без учёта регистра
    /// </summary>
    [Fact]
    public async Task IsActiveNameExistsShouldReturnTrueIgnoreCase()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();
        var targetParentFolderId = Guid.NewGuid();
        var targetFolder = entityProvider.Create<AdrFolder>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.ParentAdrFolderId = targetParentFolderId;
        });
        await Context.AddAsync(targetFolder);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrFolderReadRepository.IsActiveNameExistsAsync(targetOrganizationId,
            targetParentFolderId,
            targetFolder.Name.ToUpper(),
            CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Проверяет, что папка с тем же именем, но у другого родителя, не считается дубликатом
    /// </summary>
    [Fact]
    public async Task IsActiveNameExistsShouldReturnFalseForOtherParent()
    {
        // Arrange
        var targetOrganizationId = Guid.NewGuid();
        var targetParentFolderId = Guid.NewGuid();
        var otherParentFolderId = Guid.NewGuid();
        var targetFolder = entityProvider.Create<AdrFolder>(x =>
        {
            x.OrganizationId = targetOrganizationId;
            x.ParentAdrFolderId = targetParentFolderId;
        });
        await Context.AddAsync(targetFolder);
        await Context.SaveChangesAsync();

        // Act
        var result = await adrFolderReadRepository.IsActiveNameExistsAsync(targetOrganizationId,
            otherParentFolderId,
            targetFolder.Name,
            CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }
}
