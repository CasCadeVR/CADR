using CADR.Adrs.Services.Contracts.Models.Enums;
using CADR.Adrs.Services.Contracts.Models.Links;
using CADR.Adrs.Services.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace CADR.Adrs.Services.Tests.Validators;

/// <summary>
/// Тесты для <see cref="CreateAdrLinkModelValidator"/>
/// </summary>
public class CreateAdrLinkModelValidatorTests
{
    private readonly CreateAdrLinkModelValidator validator;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CreateAdrLinkModelValidatorTests"/>
    /// </summary>
    public CreateAdrLinkModelValidatorTests()
    {
        validator = new CreateAdrLinkModelValidator();
    }

    /// <summary>
    /// Тест на ошибки
    /// </summary>
    [Fact]
    public async Task ShouldHaveErrorMessage()
    {
        //Arrange
        var model = new CreateAdrLinkModel();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.SourceAdrId);
        result.ShouldHaveValidationErrorFor(x => x.TargetAdrId);
    }

    /// <summary>
    /// Тест на ошибку некорректного типа связи
    /// </summary>
    [Fact]
    public async Task ShouldHaveTypeErrorMessage()
    {
        //Arrange
        var model = new CreateAdrLinkModel
        {
            UserId = Guid.NewGuid(),
            SourceAdrId = Guid.NewGuid(),
            TargetAdrId = Guid.NewGuid(),
            Type = (AdrLinkType)10000,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Type);
    }

    /// <summary>
    /// Тест на ошибку связи ADR с самим собой
    /// </summary>
    [Fact]
    public async Task ShouldHaveSelfLinkErrorMessage()
    {
        //Arrange
        var adrId = Guid.NewGuid();
        var model = new CreateAdrLinkModel
        {
            UserId = Guid.NewGuid(),
            SourceAdrId = adrId,
            TargetAdrId = adrId,
            Type = AdrLinkType.RelatedTo,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveAnyValidationError();
    }

    /// <summary>
    /// Тест на отсутствие ошибок
    /// </summary>
    [Fact]
    public async Task ShouldNotHaveErrorMessage()
    {
        //Arrange
        var model = new CreateAdrLinkModel
        {
            UserId = Guid.NewGuid(),
            SourceAdrId = Guid.NewGuid(),
            TargetAdrId = Guid.NewGuid(),
            Type = AdrLinkType.RelatedTo,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        result.ShouldNotHaveValidationErrorFor(x => x.SourceAdrId);
        result.ShouldNotHaveValidationErrorFor(x => x.TargetAdrId);
        result.ShouldNotHaveValidationErrorFor(x => x.Type);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
