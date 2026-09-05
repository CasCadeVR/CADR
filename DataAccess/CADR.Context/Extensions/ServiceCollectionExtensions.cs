using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Cadr.Context.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавляет контекст данных
    /// </summary>
    public static void AddCadrContext(this IServiceCollection serviceCollection, bool isDevelopment)
    {
        serviceCollection.TryAddSingleton(provider =>
        {
            var configuration = provider.GetRequiredService<ICadrContextConfiguration>();
            var dbContextOptions = new DbContextOptions<CadrContext>(new Dictionary<Type, IDbContextOptionsExtension>());
            var optionsBuilder = new DbContextOptionsBuilder<CadrContext>(dbContextOptions)
                .UseApplicationServiceProvider(provider)
                .UseNpgsql(configuration.ConnectionString);
            if (isDevelopment)
            {
                optionsBuilder = optionsBuilder.LogTo(Console.WriteLine);
            }

            return optionsBuilder.Options;
        });

        serviceCollection.TryAddSingleton<DbContextOptions>(provider
            => provider.GetRequiredService<DbContextOptions<CadrContext>>());

        serviceCollection.TryAddScoped<CadrContext>();
        var interfaces = typeof(CadrContext).GetTypeInfo()
            .ImplementedInterfaces
            .Where(i => i != typeof(IDisposable) && (i.IsPublic));

        foreach (Type interfaceType in interfaces)
        {
            serviceCollection.TryAdd(new ServiceDescriptor(interfaceType,
                provider => provider.GetRequiredService<CadrContext>(),
                ServiceLifetime.Scoped));
        }
    }
}
