using CADR.Administrations.Repositories.Contracts;
using CADR.Administrations.Services.Contracts.Models.User;
using CADR.Administrations.Services.Validators;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace CADR.Administrations.Services.Tests.Validators;

/// <summary>
/// Тесты для <see cref="CreateUserModelValidator"/>
/// </summary>
public class CreateUserModelValidatorTests
{
    private readonly CreateUserModelValidator validator;
    private readonly Mock<IUserReadRepository> userReadRepositoryMock;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CreateUserModelValidatorTests"/>
    /// </summary>
    public CreateUserModelValidatorTests()
    {
        userReadRepositoryMock = new Mock<IUserReadRepository>();
        validator = new CreateUserModelValidator(userReadRepositoryMock.Object);
    }

    /// <summary>
    /// Тест на ошибки
    /// </summary>
    [Fact]
    public async Task ShouldHaveErrorMessage()
    {
        //Arrange
        var model = new CreateUserModel();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Login);
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.Password);
        result.ShouldHaveValidationErrorFor(x => x.PasswordAgain);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    /// <summary>
    /// Тест на ошибки пароля
    /// </summary>
    [Fact]
    public async Task ShouldHavePasswordLengthErrorMessage()
    {
        //Arrange
        var model = new CreateUserModel
        {
            Login = $"Login{Guid.NewGuid()}",
            Email = $"Email{Guid.NewGuid()}",
            Name = $"Name{Guid.NewGuid()}",
            Password = "12345",
            PasswordAgain = $"PasswordAgain{Guid.NewGuid()}",
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
        result.ShouldHaveValidationErrorFor(x => x.PasswordAgain);
    }

    /// <summary>
    /// Тест на ошибки разницы пароля и повторного пароля
    /// </summary>
    [Fact]
    public async Task ShouldHaveLoginExistsErrorMessage()
    {
        //Arrange
        var pwd = $"Password{Guid.NewGuid()}";
        var model = new CreateUserModel
        {
            Login = $"Login{Guid.NewGuid()}",
            Email = "login@mail.domain",
            Name = $"Name{Guid.NewGuid()}",
            Password = pwd,
            PasswordAgain = pwd,
        };
        userReadRepositoryMock.Setup(x => x.IsActiveLoginExistsAsync(model.Login, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Login);
    }

    /// <summary>
    /// Тест на ошибки разницы пароля и повторного пароля
    /// </summary>
    [Fact]
    public async Task ShouldHaveEmailExistsErrorMessage()
    {
        //Arrange
        var pwd = $"Password{Guid.NewGuid()}";
        var model = new CreateUserModel
        {
            Login = $"Login{Guid.NewGuid()}",
            Email = "login@mail.domain",
            Name = $"Name{Guid.NewGuid()}",
            Password = pwd,
            PasswordAgain = pwd,
        };
        userReadRepositoryMock.Setup(x => x.IsActiveEmailExistsAsync(model.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    /// <summary>
    /// Тест на отсутствие ошибок
    /// </summary>
    [Fact]
    public async Task ShouldNotHaveErrorMessage()
    {
        //Arrange
        var pwd = $"Password{Guid.NewGuid()}";
        var model = new CreateUserModel
        {
            Login = $"Login{Guid.NewGuid()}",
            Email = "login@mail.domain",
            Name = $"Name{Guid.NewGuid()}",
            Password = pwd,
            PasswordAgain = pwd,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Login);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
        result.ShouldNotHaveValidationErrorFor(x => x.PasswordAgain);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
}
