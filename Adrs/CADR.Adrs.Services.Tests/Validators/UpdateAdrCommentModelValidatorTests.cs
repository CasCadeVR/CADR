using CADR.Adrs.Services.Contracts.Models.Comments;
using CADR.Adrs.Services.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace CADR.Adrs.Services.Tests.Validators;

/// <summary>
/// Тесты для <see cref="UpdateAdrCommentModelValidator"/>
/// </summary>
public class UpdateAdrCommentModelValidatorTests
{
    private readonly UpdateAdrCommentModelValidator validator;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UpdateAdrCommentModelValidatorTests"/>
    /// </summary>
    public UpdateAdrCommentModelValidatorTests()
    {
        validator = new UpdateAdrCommentModelValidator();
    }

    /// <summary>
    /// Тест на ошибки
    /// </summary>
    [Fact]
    public async Task ShouldHaveErrorMessage()
    {
        //Arrange
        var model = new UpdateAdrCommentModel();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.Id);
        result.ShouldHaveValidationErrorFor(x => x.Text);
    }

    /// <summary>
    /// Тест на отсутствие ошибок
    /// </summary>
    [Fact]
    public async Task ShouldNotHaveErrorMessage()
    {
        //Arrange
        var model = new UpdateAdrCommentModel
        {
            UserId = Guid.NewGuid(),
            Id = Guid.NewGuid(),
            Text = $"Text{Guid.NewGuid()}",
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
        result.ShouldNotHaveValidationErrorFor(x => x.Text);
    }
}
