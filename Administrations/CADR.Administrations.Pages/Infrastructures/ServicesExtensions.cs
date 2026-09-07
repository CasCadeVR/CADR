using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace CADR.Administrations.Pages.Infrastructures;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class ServicesExtensions
{
    /// <summary>
    /// Добавляет регистрацию сервисов модуля работы с пользователями
    /// </summary>
    public static void AddAdministrationServices(this IServiceCollection services)
    {
        services.AddSingleton<Profile, AdministrationsProfile>();
    }
}
