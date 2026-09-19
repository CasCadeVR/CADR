namespace CADR.Adrs.Api.Resources;

/// <summary>
/// Общие константы для модуля ADR
/// </summary>
public class AdrsConstants
{
    /// <summary>
    /// Префикс имени документации
    /// </summary>
    public const string DocPrefix = "adrs";

    /// <summary>
    /// Заголовок документации
    /// </summary>
    public const string DocName = "Adr API";

    /// <summary>
    /// Роут контроллера с версионированием по умолчанию
    /// </summary>
    public const string DefaultControllerRoute = "v{version:apiVersion}/[controller]";
}
