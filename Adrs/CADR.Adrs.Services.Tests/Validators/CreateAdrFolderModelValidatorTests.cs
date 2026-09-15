using CADR.Adrs.Repositories.Contracts;
using CADR.Adrs.Services.Contracts.Models.Folders;
using CADR.Adrs.Services.Validators;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace CADR.Adrs.Services.Tests.Validators;

/// <summary>
/// Тесты для <see cref="CreateAdrFolderModelValidator"/>
/// </summary>
public class CreateAdrFolderModelValidatorTests
{
    private readonly CreateAdrFolderModelValidator validator;
    private readonly Mock<IAdrFolderReadRepository> adrFolderReadRepositoryMock;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CreateAdrFolderModelValidatorTests"/>
    /// </summary>
    public CreateAdrFolderModelValidatorTests()
    {
        adrFolderReadRepositoryMock = new Mock<IAdrFolderReadRepository>();
        validator = new CreateAdrFolderModelValidator(adrFolderReadRepositoryMock.Object);
    }

    /// <summary>
    /// Тест на ошибки
    /// </summary>
    [Fact]
    public async Task ShouldHaveErrorMessage()
    {
        //Arrange
        var model = new CreateAdrFolderModel();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.OrganizationId);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    /// <summary>
    /// Тест на ошибку существования папки с таким именем
    /// </summary>
    [Fact]
    public async Task ShouldHaveNameExistsErrorMessage()
    {
        //Arrange
        var model = new CreateAdrFolderModel
        {
            UserId = Guid.NewGuid(),
            OrganizationId = Guid.NewGuid(),
            Name = $"Name{Guid.NewGuid()}",
        };
        adrFolderReadRepositoryMock.Setup(x => x.IsActiveNameExistsAsync(model.OrganizationId,
                model.ParentAdrFolderId,
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveAnyValidationError();
    }

    /// <summary>
    /// Тест на отсутствие ошибок
    /// </summary>
    [Fact]
    public async Task ShouldNotHaveErrorMessage()
    {
        //Arrange
        var model = new CreateAdrFolderModel
        {
            UserId = Guid.NewGuid(),
            OrganizationId = Guid.NewGuid(),
            Name = $"Name{Guid.NewGuid()}",
        };
        adrFolderReadRepositoryMock.Setup(x => x.IsActiveNameExistsAsync(model.OrganizationId,
                model.ParentAdrFolderId,
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.OrganizationId);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
