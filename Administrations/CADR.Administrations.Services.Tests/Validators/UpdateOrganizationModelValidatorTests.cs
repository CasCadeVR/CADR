using Ahatornn.TestGenerator;
using CADR.Administrations.Repositories.Contracts;
using CADR.Administrations.Services.Contracts.Models.Organizations;
using CADR.Administrations.Services.Validators;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace CADR.Administrations.Services.Tests.Validators;

/// <summary>
/// Тесты для <see cref="UpdateOrganizationModelValidator"/>
/// </summary>
public class UpdateOrganizationModelValidatorTests
{
    private readonly UpdateOrganizationModelValidator validator;
    private readonly Mock<IOrganizationReadRepository> organizationReadRepositoryMock;
    private readonly TestEntityProvider entityProvider;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UpdateOrganizationModelValidatorTests"/>
    /// </summary>
    public UpdateOrganizationModelValidatorTests()
    {
        entityProvider = new TestEntityProviderBuilder()
            .AddPreset<Entities.Organization>(x =>
            {
                var name = $"Name{Guid.NewGuid():N}";
                x.Name = name;
                x.NameLowerCase = name.ToLower();
            })
            .Build();
        organizationReadRepositoryMock = new Mock<IOrganizationReadRepository>();
        validator = new UpdateOrganizationModelValidator(organizationReadRepositoryMock.Object);
    }

    /// <summary>
    /// Тест на ошибки
    /// </summary>
    [Fact]
    public async Task ShouldHaveErrorMessage()
    {
        //Arrange
        var model = new UpdateOrganizationModel();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    /// <summary>
    /// Тест на ошибки существования организации
    /// </summary>
    [Fact]
    public async Task ShouldHaveNameExistsErrorMessage()
    {
        //Arrange
        var model = new UpdateOrganizationModel
        {
            Name = $"Name{Guid.NewGuid()}",
            Description = $"Description{Guid.NewGuid()}",
            UserId = Guid.NewGuid(),
            Id = Guid.NewGuid(),
        };
        organizationReadRepositoryMock.Setup(x => x.GetActiveByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(entityProvider.Create<Entities.Organization>());

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    /// <summary>
    /// Тест на ошибки существования организации
    /// </summary>
    [Fact]
    public async Task ShouldHaveNoErrors()
    {
        //Arrange
        var model = new UpdateOrganizationModel
        {
            Name = $"Name{Guid.NewGuid()}",
            Description = $"Description{Guid.NewGuid()}",
            UserId = Guid.NewGuid(),
            Id = Guid.NewGuid(),
        };
        organizationReadRepositoryMock.Setup(x => x.GetActiveByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(entityProvider.Create<Entities.Organization>(x => x.Id = model.Id));

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
}
