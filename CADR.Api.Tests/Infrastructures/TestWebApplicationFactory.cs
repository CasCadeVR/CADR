using CADR.Api.Tests.Helpers;
using CADR.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CADR.Api.Tests.Infrastructures;

/// <summary>
/// Хост для тестирования Api
/// </summary>
public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestAppConfiguration();
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<CadrContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddSingleton<DbContextOptions<CadrContext>>(provider =>
            {
                var configuration = provider.GetRequiredService<ICadrContextConfiguration>();
                var dbContextOptions = new DbContextOptions<CadrContext>(new Dictionary<Type, IDbContextOptionsExtension>());
                var optionsBuilder = new DbContextOptionsBuilder<CadrContext>(dbContextOptions)
                    .UseApplicationServiceProvider(provider)
                    .UseNpgsql(connectionString: string.Format(configuration.ConnectionString, Guid.NewGuid().ToString("N")));
                return optionsBuilder.Options;
            });
        });
    }
}
