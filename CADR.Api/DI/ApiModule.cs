using AutoMapper;
using CADR.Administrations.Api.DI;
using CADR.Api.Infrastructures;
using CADR.Api.Stubs;
using CADR.Common.Core.Implementations;
using CADR.Common.Mvc.Extensions;
using CADR.Common.Mvc.Models;

namespace CADR.Api.DI;

/// <inheritdoc />
public class ApiModule : Module
{
    /// <inheritdoc />
    protected override void Load(IServiceCollection services)
    {
        services.RegisterAsImplementedInterfaces<DateTimeProvider>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<ApiIdentityProvider>(ServiceLifetime.Scoped);
        services.RegisterAsImplementedInterfaces<DbWriterContext>(ServiceLifetime.Scoped);
        services.RegisterAsImplementedInterfaces<CadrConfiguration>(ServiceLifetime.Singleton);
        services.AddHttpContextAccessor();

        services.RegisterModule<AdministrationModule>();

        RegisterAutoMapper(services);
        RegisterStubs(services);
    }

    private static void RegisterStubs(IServiceCollection services)
    {
        services.RegisterAsImplementedInterfaces<NotifyServiceStub>(ServiceLifetime.Singleton);
    }

    private static void RegisterAutoMapper(IServiceCollection services)
    {
        services.AddSingleton<IMapper>(provider =>
        {
            var profiles = provider.GetServices<Profile>();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                foreach (var profile in profiles)
                {
                    mc.AddProfile(profile);
                }
            });
            var mapper = mapperConfig.CreateMapper();
            return mapper;
        });
    }
}
