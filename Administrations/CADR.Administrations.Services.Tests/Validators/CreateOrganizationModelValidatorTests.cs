using CADR.Administrations.Repositories.Contracts;
using CADR.Administrations.Services.Contracts.Models.Organizations;
using CADR.Administrations.Services.Validators;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace CADR.Administrations.Services.Tests.Validators;

/// <summary>
/// Тесты для <see cref="CreateOrganizationModelValidator"/>
/// </summary>
public class CreateOrganizationModelValidatorTests
{
    private readonly CreateOrganizationModelValidator validator;
    private readonly Mock<IOrganizationReadRepository> organizationReadRepositoryMock;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CreateOrganizationModelValidatorTests"/>
    /// </summary>
    public CreateOrganizationModelValidatorTests()
    {
        organizationReadRepositoryMock = new Mock<IOrganizationReadRepository>();
        validator = new CreateOrganizationModelValidator(organizationReadRepositoryMock.Object);
    }

    /// <summary>
    /// Тест на ошибки
    /// </summary>
    [Fact]
    public async Task ShouldHaveErrorMessage()
    {
        //Arrange
        var model = new CreateOrganizationModel();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    /// <summary>
    /// Тест на ошибки существования организации
    /// </summary>
    [Fact]
    public async Task ShouldHaveNameExistsErrorMessage()
    {
        //Arrange
        var model = new CreateOrganizationModel
        {
            Name = $"Name{Guid.NewGuid()}",
            Description = $"Description{Guid.NewGuid()}",
            UserId = Guid.NewGuid(),
        };
        organizationReadRepositoryMock.Setup(x => x.IsActiveNameExistsAsync(model.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    /// <summary>
    /// Тест на отсутствие ошибок
    /// </summary>
    [Fact]
    public async Task ShouldNotHaveErrorMessage()
    {
        //Arrange
        var model = new CreateOrganizationModel
        {
            Name = $"Name{Guid.NewGuid()}",
            Description = $"Description{Guid.NewGuid()}",
            UserId = Guid.NewGuid(),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }
}
