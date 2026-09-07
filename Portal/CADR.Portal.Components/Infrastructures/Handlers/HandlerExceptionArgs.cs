namespace CADR.Portal.Components.Infrastructures.Handlers;

/// <summary>
/// Аргументы обработки ошибок
/// </summary>
public class HandlerExceptionArgs
{
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Исходная ошибка
    /// </summary>
    public Exception Source { get; set; } = null!;
}
