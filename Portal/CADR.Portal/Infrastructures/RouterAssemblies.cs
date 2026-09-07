using System.Reflection;

namespace CADR.Portal.Infrastructures;

/// <summary>
/// Сборки проектов для роутов
/// </summary>
public static class RouterAssemblies
{
    /// <summary>
    /// Зарегистрированные проекты
    /// </summary>
    public static Assembly[] Registered
        =>
    [
        typeof(Administrations.Pages._Imports).Assembly,
    ];
}
