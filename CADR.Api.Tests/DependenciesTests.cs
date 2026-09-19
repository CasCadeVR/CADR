using System.Reflection;
using CADR.Administrations.Api.Controllers;
using CADR.Adrs.Api.Controllers;
using CADR.Api.Tests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
namespace CADR.Api.Tests;

/// <summary>
/// Тесты зависимостей контроллеров
/// </summary>
public class DependenciesTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="DependenciesTests"/>
    /// </summary>
    public DependenciesTests(WebApplicationFactory<Program> factory)
    {
        this.factory = factory.WithWebHostBuilder(builder => builder.ConfigureTestAppConfiguration());
    }

    /// <summary>
    /// Проверка резолва зависимостей
    /// </summary>
    [Theory]
    [MemberData(nameof(AdministrationControllerCore))]
    public void ControllerCoreShouldBeResolved(Type controller)
    {
        // Arrange
        using var scope = factory.Services.CreateScope();

        // Act
        var instance = scope.ServiceProvider.GetRequiredService(controller);

        // Assert
        instance.Should().NotBeNull();
    }

    /// <summary>
    /// Проверка резолва зависимостей контроллеров ADR
    /// </summary>
    [Theory]
    [MemberData(nameof(AdrsControllerCore))]
    public void AdrsControllerCoreShouldBeResolved(Type controller)
    {
        // Arrange
        using var scope = factory.Services.CreateScope();

        // Act
        var instance = scope.ServiceProvider.GetRequiredService(controller);

        // Assert
        instance.Should().NotBeNull();
    }

    /// <summary>
    /// Коллекция контроллеров по администрированию
    /// </summary>
    public static TheoryData<Type> AdministrationControllerCore => GetControllers<AccountController>();

    /// <summary>
    /// Коллекция контроллеров по ADR
    /// </summary>
    public static TheoryData<Type> AdrsControllerCore => GetControllers<AdrController>();

    private static TheoryData<Type> GetControllers<TController>() =>
        new(Assembly.GetAssembly(typeof(TController))
            ?.DefinedTypes
            .Where(type => typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract));
}
