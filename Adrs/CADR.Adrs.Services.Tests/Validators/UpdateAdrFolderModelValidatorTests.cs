using CADR.Adrs.Services.Contracts.Models.Folders;
using CADR.Adrs.Services.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace CADR.Adrs.Services.Tests.Validators;

/// <summary>
/// Тесты для <see cref="UpdateAdrFolderModelValidator"/>
/// </summary>
public class UpdateAdrFolderModelValidatorTests
{
    private readonly UpdateAdrFolderModelValidator validator;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UpdateAdrFolderModelValidatorTests"/>
    /// </summary>
    public UpdateAdrFolderModelValidatorTests()
    {
        validator = new UpdateAdrFolderModelValidator();
    }

    /// <summary>
    /// Тест на ошибки
    /// </summary>
    [Fact]
    public async Task ShouldHaveErrorMessage()
    {
        //Arrange
        var model = new UpdateAdrFolderModel();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.Id);
        result.ShouldHaveValidationErrorFor(x => x.OrganizationId);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    /// <summary>
    /// Тест на отсутствие ошибок
    /// </summary>
    [Fact]
    public async Task ShouldNotHaveErrorMessage()
    {
        //Arrange
        var model = new UpdateAdrFolderModel
        {
            UserId = Guid.NewGuid(),
            Id = Guid.NewGuid(),
            OrganizationId = Guid.NewGuid(),
            Name = $"Name{Guid.NewGuid()}",
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
        result.ShouldNotHaveValidationErrorFor(x => x.OrganizationId);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
}
