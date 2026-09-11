using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace CADR.Common.Mvc.Extensions;

/// <summary>
/// Методы расширения для <see cref="Assembly"/>
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    /// Получает список имён групп заданных в <see cref="ApiExplorerSettingsAttribute"/>
    /// у контроллеров в заданной сборке
    /// </summary>
    public static string[] GetApiExplorerSettingsGroupName(this Assembly? assembly)
        => assembly?.DefinedTypes
            .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
            .Where(type => !type.IsAbstract && type.GetCustomAttributes(typeof(ApiExplorerSettingsAttribute), true).Any())
            .SelectMany(type => type.GetCustomAttributes(typeof(ApiExplorerSettingsAttribute), true)
                .Cast<ApiExplorerSettingsAttribute>()
                .Select(x => x.GroupName ?? string.Empty))
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToArray() ?? [];
}
