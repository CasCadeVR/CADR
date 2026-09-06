using CADR.Administrations.Repositories.Contracts;
using CADR.Api.Tests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CADR.Api.Tests;

public class UnitOfWorkTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;

    public UnitOfWorkTests(WebApplicationFactory<Program> factory)
    {
        this.factory = factory.WithWebHostBuilder(builder => builder.ConfigureTestAppConfiguration());
    }

    [Theory]
    [MemberData(nameof(AdministrationUnitOfWork))]
    public void UnitOfWorkShouldWork(Type type)
    {
        using var scope = factory.Services.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService(type);
        var properties = type.GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(unitOfWork);
            value.Should().NotBeNull();
        }
    }

    public static TheoryData<Type> AdministrationUnitOfWork => new(typeof(IAdministrationUnitOfWork));
}
