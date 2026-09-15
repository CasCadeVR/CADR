using CADR.Adrs.Services.Contracts.Models.Adrs;
using CADR.Adrs.Services.Contracts.Models.Enums;
using CADR.Adrs.Services.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace CADR.Adrs.Services.Tests.Validators;

/// <summary>
/// Тесты для <see cref="UpdateAdrModelValidator"/>
/// </summary>
public class UpdateAdrModelValidatorTests
{
    private readonly UpdateAdrModelValidator validator;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UpdateAdrModelValidatorTests"/>
    /// </summary>
    public UpdateAdrModelValidatorTests()
    {
        validator = new UpdateAdrModelValidator();
    }

    /// <summary>
    /// Тест на ошибки
    /// </summary>
    [Fact]
    public async Task ShouldHaveErrorMessage()
    {
        //Arrange
        var model = new UpdateAdrModel();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.Id);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    /// <summary>
    /// Тест на ошибку некорректного статуса
    /// </summary>
    [Fact]
    public async Task ShouldHaveStatusErrorMessage()
    {
        //Arrange
        var model = new UpdateAdrModel
        {
            UserId = Guid.NewGuid(),
            Id = Guid.NewGuid(),
            Title = $"Title{Guid.NewGuid()}",
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
        var model = new UpdateAdrModel
        {
            UserId = Guid.NewGuid(),
            Id = Guid.NewGuid(),
            Title = $"Title{Guid.NewGuid()}",
            Status = AdrStatus.Draft,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
        result.ShouldNotHaveValidationErrorFor(x => x.Status);
    }
}
