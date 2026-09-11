using AutoMapper;
using CADR.Administrations.Pages.Infrastructures;
using CADR.Portal.Components.Infrastructures;
using CADR.Portal.Contracts.Interfaces;
using CADR.Portal.Contracts.Interfaces.JSEventHandlers;
using CADR.Portal.Infrastructures.JSInvokable;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace CADR.Portal.Infrastructures;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class ServicesExtensions
{
    /// <summary>
    /// Регистрирует <see cref="IPortalCadrApiClient"/>
    /// </summary>
    public static void AddCadrApiClient(this IServiceCollection services, IConfiguration configuration)
        => services.AddTransient<IPortalCadrApiClient>(x =>
        {
            var httpClientFactory = x.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient();
            var credentials = Enum.Parse<BrowserRequestCredentials>(configuration["BrowserRequestCredentials"]!, true);

            return new PortalCadrApiClient(configuration["ApiHost"]!, credentials, httpClient);
        });

    /// <summary>
    /// Регистрирует поставщика личности
    /// </summary>
    public static void AddIdentity(this IServiceCollection services)
    {
        services.AddSingleton<PortalIdentityProvider>();
        services.AddSingleton<IPortalIdentity>(x => x.GetRequiredService<PortalIdentityProvider>());
        services.AddTransient<ApiAuthenticationStateProvider>();
        services.AddTransient<AuthenticationStateProvider>(x => x.GetRequiredService<ApiAuthenticationStateProvider>());
    }

    /// <summary>
    /// Регистрирует обработчики событий JavaScript
    /// </summary>
    public static void AddJsEventHandlers(this IServiceCollection services)
    {
        services.AddTransient<IJsDropdownEventHandler, JsDropdownEventHandler>();
    }

    /// <summary>
    /// Добавляет все необходимые зависимости
    /// </summary>
    public static void AddDependencyInjection(this IServiceCollection services)
    {
        services.AddSingleton<MessageProvider>();
        services.AddSingleton<IMessageProvider>(x => x.GetRequiredService<MessageProvider>());
        services.AddSingleton<IMessageBox>(x => x.GetRequiredService<MessageProvider>());

        services.AddAutoMapper();

        services.AddAdministrationServices();
    }

    private static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddSingleton<AutoMapper.IConfigurationProvider>(x =>
        {
            return new MapperConfiguration(cfg =>
            {
                foreach (var profile in x.GetServices<Profile>())
                {
                    cfg.AddProfile(profile);
                }
            });
        });

        services.AddSingleton(x =>
        {
            var mapperConfiguration = x.GetRequiredService<AutoMapper.IConfigurationProvider>();
            return mapperConfiguration.CreateMapper();
        });
    }
}
