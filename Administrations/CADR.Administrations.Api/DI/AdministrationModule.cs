using CADR.Administrations.Api.AutoMappers;
using CADR.Administrations.Repositories;
using CADR.Administrations.Services;
using CADR.Administrations.Services.AutoMappers;
using CADR.Common.Mvc.Extensions;
using CADR.Common.Mvc.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CADR.Administrations.Api.DI;

/// <summary>
/// Модуль регистрации зависимостей для администрирования
/// </summary>
public class AdministrationModule : Module
{
    /// <inheritdoc />
    protected override void Load(IServiceCollection services)
    {
        services.RegisterAssemblyInterfacesAssignableTo<IAdministrationRepositoryAnchor>(ServiceLifetime.Scoped);
        services.RegisterAssemblyInterfacesAssignableTo<IAdministrationsServiceAnchor>(ServiceLifetime.Scoped);
        services.RegisterAutoMapperProfile<AdministrationServiceProfile>();
        services.RegisterAutoMapperProfile<AdministrationMapperProfile>();
    }
}
