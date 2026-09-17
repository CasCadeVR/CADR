using CADR.Adrs.Services.Contracts.Models.Templates;
using CADR.Adrs.Services.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace CADR.Adrs.Services.Tests.Validators;

/// <summary>
/// Тесты для <see cref="UpdateAdrTemplateModelValidator"/>
/// </summary>
public class UpdateAdrTemplateModelValidatorTests
{
    private readonly UpdateAdrTemplateModelValidator validator;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UpdateAdrTemplateModelValidatorTests"/>
    /// </summary>
    public UpdateAdrTemplateModelValidatorTests()
    {
        validator = new UpdateAdrTemplateModelValidator();
    }

    /// <summary>
    /// Тест на ошибки
    /// </summary>
    [Fact]
    public async Task ShouldHaveErrorMessage()
    {
        //Arrange
        var model = new UpdateAdrTemplateModel();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.Id);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    /// <summary>
    /// Тест на отсутствие ошибок
    /// </summary>
    [Fact]
    public async Task ShouldNotHaveErrorMessage()
    {
        //Arrange
        var model = new UpdateAdrTemplateModel
        {
            UserId = Guid.NewGuid(),
            Id = Guid.NewGuid(),
            Name = $"Name{Guid.NewGuid()}",
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
}
