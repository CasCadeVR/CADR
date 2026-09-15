using CADR.Adrs.Services.Contracts.Models.Comments;
using CADR.Adrs.Services.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace CADR.Adrs.Services.Tests.Validators;

/// <summary>
/// Тесты для <see cref="CreateAdrCommentModelValidator"/>
/// </summary>
public class CreateAdrCommentModelValidatorTests
{
    private readonly CreateAdrCommentModelValidator validator;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CreateAdrCommentModelValidatorTests"/>
    /// </summary>
    public CreateAdrCommentModelValidatorTests()
    {
        validator = new CreateAdrCommentModelValidator();
    }

    /// <summary>
    /// Тест на ошибки
    /// </summary>
    [Fact]
    public async Task ShouldHaveErrorMessage()
    {
        //Arrange
        var model = new CreateAdrCommentModel();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.AdrId);
        result.ShouldHaveValidationErrorFor(x => x.Text);
    }

    /// <summary>
    /// Тест на отсутствие ошибок
    /// </summary>
    [Fact]
    public async Task ShouldNotHaveErrorMessage()
    {
        //Arrange
        var model = new CreateAdrCommentModel
        {
            UserId = Guid.NewGuid(),
            AdrId = Guid.NewGuid(),
            Text = $"Text{Guid.NewGuid()}",
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.AdrId);
        result.ShouldNotHaveValidationErrorFor(x => x.Text);
    }
}
