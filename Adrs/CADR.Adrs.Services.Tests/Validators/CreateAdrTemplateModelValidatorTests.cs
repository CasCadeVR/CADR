using CADR.Adrs.Repositories.Contracts;
using CADR.Adrs.Services.Contracts.Models.Templates;
using CADR.Adrs.Services.Validators;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace CADR.Adrs.Services.Tests.Validators;

/// <summary>
/// Тесты для <see cref="CreateAdrTemplateModelValidator"/>
/// </summary>
public class CreateAdrTemplateModelValidatorTests
{
    private readonly CreateAdrTemplateModelValidator validator;
    private readonly Mock<IAdrTemplateReadRepository> adrTemplateReadRepositoryMock;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CreateAdrTemplateModelValidatorTests"/>
    /// </summary>
    public CreateAdrTemplateModelValidatorTests()
    {
        adrTemplateReadRepositoryMock = new Mock<IAdrTemplateReadRepository>();
        validator = new CreateAdrTemplateModelValidator(adrTemplateReadRepositoryMock.Object);
    }

    /// <summary>
    /// Тест на ошибки
    /// </summary>
    [Fact]
    public async Task ShouldHaveErrorMessage()
    {
        //Arrange
        var model = new CreateAdrTemplateModel();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.OrganizationId);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    /// <summary>
    /// Тест на ошибку существования шаблона с таким именем
    /// </summary>
    [Fact]
    public async Task ShouldHaveNameExistsErrorMessage()
    {
        //Arrange
        var model = new CreateAdrTemplateModel
        {
            UserId = Guid.NewGuid(),
            OrganizationId = Guid.NewGuid(),
            Name = $"Name{Guid.NewGuid()}",
        };
        adrTemplateReadRepositoryMock.Setup(x => x.IsActiveNameExistsAsync(model.OrganizationId,
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
        var model = new CreateAdrTemplateModel
        {
            UserId = Guid.NewGuid(),
            OrganizationId = Guid.NewGuid(),
            Name = $"Name{Guid.NewGuid()}",
        };
        adrTemplateReadRepositoryMock.Setup(x => x.IsActiveNameExistsAsync(model.OrganizationId,
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
