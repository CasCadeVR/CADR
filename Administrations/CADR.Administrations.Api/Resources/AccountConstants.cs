namespace CADR.Administrations.Api.Resources;

/// <summary>
/// Общие константы для администрирования
/// </summary>
public class AdministrationConstants
{
    /// <summary>
    /// Префикс имени документации
    /// </summary>
    public const string DocPrefix = "administration";

    /// <summary>
    /// Заголовок документации
    /// </summary>
    public const string DocName = "Administration API";

    /// <summary>
    /// Роут контроллера с версионированием по умолчанию
    /// </summary>
    public const string DefaultControllerRoute = "v{version:apiVersion}/[controller]";
}
