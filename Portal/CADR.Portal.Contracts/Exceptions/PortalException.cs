namespace CADR.Portal.Contracts.Exceptions;

/// <summary>
/// Базовый класс исключений клиента
/// </summary>
public class PortalException : Exception
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="PortalException"/> без параметров
    /// </summary>
    protected PortalException() { }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="PortalException"/> с указанием
    /// сообщения об ошибке
    /// </summary>
    protected PortalException(string message)
        : base(message) { }
}
