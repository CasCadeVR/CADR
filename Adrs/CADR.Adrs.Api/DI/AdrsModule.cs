using CADR.Adrs.Api.AutoMappers;
using CADR.Adrs.Repositories;
using CADR.Adrs.Services;
using CADR.Adrs.Services.AutoMappers;
using CADR.Common.Mvc.Extensions;
using CADR.Common.Mvc.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CADR.Adrs.Api.DI;

/// <summary>
/// Модуль регистрации зависимостей для администрирования
/// </summary>
public class AdrsModule : Module
{
    /// <inheritdoc />
    protected override void Load(IServiceCollection services)
    {
        services.RegisterAssemblyInterfacesAssignableTo<IAdrsRepositoryAnchor>(ServiceLifetime.Scoped);
        services.RegisterAssemblyInterfacesAssignableTo<IAdrsServiceAnchor>(ServiceLifetime.Scoped);
        services.RegisterAutoMapperProfile<AdrServiceProfile>();
        services.RegisterAutoMapperProfile<AdrMapperProfile>();
    }
}
