using CADR.Adrs.Services.Contracts.Models.Adrs;
using CADR.Adrs.Services.Contracts.Models.Enums;
using CADR.Adrs.Services.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace CADR.Adrs.Services.Tests.Validators;

/// <summary>
/// Тесты для <see cref="CreateAdrModelValidator"/>
/// </summary>
public class CreateAdrModelValidatorTests
{
    private readonly CreateAdrModelValidator validator;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CreateAdrModelValidatorTests"/>
    /// </summary>
    public CreateAdrModelValidatorTests()
    {
        validator = new CreateAdrModelValidator();
    }

    /// <summary>
    /// Тест на ошибки
    /// </summary>
    [Fact]
    public async Task ShouldHaveErrorMessage()
    {
        //Arrange
        var model = new CreateAdrModel();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
        result.ShouldHaveValidationErrorFor(x => x.OrganizationId);
    }

    /// <summary>
    /// Тест на ошибку некорректного статуса
    /// </summary>
    [Fact]
    public async Task ShouldHaveStatusErrorMessage()
    {
        //Arrange
        var model = new CreateAdrModel
        {
            Title = $"Title{Guid.NewGuid()}",
            OrganizationId = Guid.NewGuid(),
            Status = (AdrStatus)10000,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Status);
    }

    /// <summary>
    /// Тест на отсутствие ошибок
    /// </summary>
    [Fact]
    public async Task ShouldNotHaveErrorMessage()
    {
        //Arrange
        var model = new CreateAdrModel
        {
            Title = $"Title{Guid.NewGuid()}",
            OrganizationId = Guid.NewGuid(),
            Status = AdrStatus.Draft,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
        result.ShouldNotHaveValidationErrorFor(x => x.OrganizationId);
        result.ShouldNotHaveValidationErrorFor(x => x.Status);
    }
}
