namespace CADR.Adrs.Services.Contracts.Exceptions;

/// <summary>
/// Базовый класс исключений администрирования
/// </summary>
public abstract class AdrException : Exception
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrException"/> без параметров
    /// </summary>
    protected AdrException() { }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrException"/> с указанием
    /// сообщения об ошибке
    /// </summary>
    protected AdrException(string message)
        : base(message) { }
}
