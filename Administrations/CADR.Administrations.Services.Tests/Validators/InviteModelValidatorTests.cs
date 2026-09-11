using CADR.Administrations.Services.Contracts.Models.Enums;
using CADR.Administrations.Services.Contracts.Models.Invites;
using CADR.Administrations.Services.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace CADR.Administrations.Services.Tests.Validators;

/// <summary>
/// Тесты для <see cref="InviteModelValidator"/>
/// </summary>
public class InviteModelValidatorTests
{
    private readonly InviteModelValidator validator;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="InviteModelValidatorTests"/>
    /// </summary>
    public InviteModelValidatorTests()
    {
        validator = new InviteModelValidator();
    }

    /// <summary>
    /// Тест на ошибки
    /// </summary>
    [Fact]
    public async Task ShouldHaveErrorMessage()
    {
        //Arrange
        var model = new InviteModel { Role = (UserRole)10000, };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserMail);
        result.ShouldHaveValidationErrorFor(x => x.Role);
    }

    /// <summary>
    /// Тест на отсутствие ошибок
    /// </summary>
    [Fact]
    public async Task ShouldNotHaveErrorMessage()
    {
        //Arrange
        var model = new InviteModel
        {
            UserMail = "login@server.domain",
            Role = UserRole.Admin,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserMail);
        result.ShouldNotHaveValidationErrorFor(x => x.Role);
    }
}
