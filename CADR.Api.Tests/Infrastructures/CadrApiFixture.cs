using CADR.Api.Tests.Client;
using CADR.Context;
using CADR.Context.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CADR.Api.Tests.Infrastructures;

/// <summary>
/// Фикстура для поднятия апишки Cadr
/// </summary>
public class CadrApiFixture : IAsyncLifetime
{
    private CadrContext? specularContext;
    readonly protected TestWebApplicationFactory Factory;

    public IUnitOfWork UnitOfWork => CadrContext;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CadrApiFixture"/>
    /// </summary>
    public CadrApiFixture()
    {
        Factory = new TestWebApplicationFactory();
    }

    internal CadrContext CadrContext
    {
        get
        {
            if (specularContext != null)
            {
                return specularContext;
            }

            var scope = Factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            specularContext = scope.ServiceProvider.GetRequiredService<CadrContext>();
            return specularContext;
        }
    }

    internal ICadrApiTestClient CreateAnonymousApiClient() => new CadrApiTestClient(string.Empty, Factory.CreateClient(), null);

    /// <inheritdoc cref="IAsyncLifetime.InitializeAsync"/>
    public virtual Task InitializeAsync() => CadrContext.Database.MigrateAsync();

    async Task IAsyncLifetime.DisposeAsync()
    {
        await CadrContext.Database.EnsureDeletedAsync();
        await CadrContext.Database.CloseConnectionAsync();
        await CadrContext.DisposeAsync();
        await Factory.DisposeAsync();
    }
}
