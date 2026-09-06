using CADR.Administrations.Services.Contracts.Models.User;
using CADR.Administrations.Services.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace CADR.Administrations.Services.Tests.Validators;

/// <summary>
/// Тесты для <see cref="LoginModelValidator"/>
/// </summary>
public class LoginModelValidatorTests
{
    private readonly LoginModelValidator validator;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="LoginModelValidatorTests"/>
    /// </summary>
    public LoginModelValidatorTests()
    {
        validator = new LoginModelValidator();
    }

    /// <summary>
    /// Тест на ошибки
    /// </summary>
    [Fact]
    public async Task ShouldHaveErrorMessage()
    {
        //Arrange
        var model = new LoginModel();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Login);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    /// <summary>
    /// Тест на отсутствие ошибок
    /// </summary>
    [Fact]
    public async Task ShouldNotHaveErrorMessage()
    {
        //Arrange
        var model = new LoginModel
        {
            Login = $"Login{Guid.NewGuid()}",
            Password = $"Password{Guid.NewGuid()}",
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Login);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
}
