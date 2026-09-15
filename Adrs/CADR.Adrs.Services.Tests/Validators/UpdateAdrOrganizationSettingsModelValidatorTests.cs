using CADR.Adrs.Services.Contracts.Models.Settings;
using CADR.Adrs.Services.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace CADR.Adrs.Services.Tests.Validators;

/// <summary>
/// Тесты для <see cref="UpdateAdrOrganizationSettingsModelValidator"/>
/// </summary>
public class UpdateAdrOrganizationSettingsModelValidatorTests
{
    private readonly UpdateAdrOrganizationSettingsModelValidator validator;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UpdateAdrOrganizationSettingsModelValidatorTests"/>
    /// </summary>
    public UpdateAdrOrganizationSettingsModelValidatorTests()
    {
        validator = new UpdateAdrOrganizationSettingsModelValidator();
    }

    /// <summary>
    /// Тест на ошибки
    /// </summary>
    [Fact]
    public async Task ShouldHaveErrorMessage()
    {
        //Arrange
        var model = new UpdateAdrOrganizationSettingsModel
        {
            LikesRequiredForApproval = -1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.OrganizationId);
        result.ShouldHaveValidationErrorFor(x => x.LikesRequiredForApproval);
    }

    /// <summary>
    /// Тест на отсутствие ошибок
    /// </summary>
    [Fact]
    public async Task ShouldNotHaveErrorMessage()
    {
        //Arrange
        var model = new UpdateAdrOrganizationSettingsModel
        {
            UserId = Guid.NewGuid(),
            OrganizationId = Guid.NewGuid(),
            LikesRequiredForApproval = 1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.OrganizationId);
        result.ShouldNotHaveValidationErrorFor(x => x.LikesRequiredForApproval);
    }
}
