using Ahatornn.TestGenerator;
using CADR.Administrations.Entities;
using CADR.Api.Client;
using CADR.Api.Tests.Client;
using CADR.Common.Mvc.Models;
using CADR.Context;
using CADR.Context.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CADR.Api.Tests.Infrastructures;

/// <summary>
/// Фикстура с апи, авторизованным пользователем и замокированным IIssueReader и IntegrationPresenter
/// </summary>
public class MockedIntegrationFixture : IAsyncLifetime
{
    private readonly TestWebApplicationFactory factory;
    private CadrContext? specularContext;

    internal PersonalOptions PersonalOptions { get; } = new()
    {
        Identifier = Guid.NewGuid(),
        Email = $"Email{Guid.NewGuid():N}",
        Login = $"Login{Guid.NewGuid():N}",
        Name = $"Name{Guid.NewGuid():N}",
        SecurityStamp = Guid.NewGuid().ToString(),
        Params = new Dictionary<string, string>(),
    };

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="MockedIntegrationFixture"/>
    /// </summary>
    public MockedIntegrationFixture()
    {
        factory = new TestWebApplicationFactory();
    }

    internal CadrContext CadrContext
    {
        get
        {
            if (specularContext != null)
            {
                return specularContext;
            }

            var scope = factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            specularContext = scope.ServiceProvider.GetRequiredService<CadrContext>();
            return specularContext;
        }
    }

    internal IUnitOfWork UnitOfWork => CadrContext;

    internal ICadrApiClient CreateApiClient()
    {
        var configuration = factory.Services.GetRequiredService<IConfiguration>();
        var authSetting = configuration.GetSection(JwtSettingsModel.Key).Get<JwtSettingsModel>()!;
        var bearerTokenProvider = new BearerTokenProvider(authSetting, PersonalOptions);
        var client = factory.CreateClient();
        return new CadrApiTestClient(string.Empty, client, bearerTokenProvider);
    }

    public async Task InitializeAsync()
    {
        await CadrContext.Database.MigrateAsync();

        var user = TestEntityProvider.Shared.Create<User>(x =>
        {
            x.Id = PersonalOptions.Identifier;
            x.Email = PersonalOptions.Email;
            x.Name = PersonalOptions.Name;
            x.Login = PersonalOptions.Login;
            x.SecurityStamp = PersonalOptions.SecurityStamp;
            x.EmailLowerCase = x.Email.ToLower();
            x.LoginLowerCase = x.Login.ToLower();
        });

        CadrContext.Add(user);
        await UnitOfWork.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await CadrContext.Database.EnsureDeletedAsync();
        await CadrContext.Database.CloseConnectionAsync();
        await CadrContext.DisposeAsync();
        await factory.DisposeAsync();
    }
}
