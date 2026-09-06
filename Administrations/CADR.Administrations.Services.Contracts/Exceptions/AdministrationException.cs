namespace CADR.Administrations.Services.Contracts.Exceptions;

/// <summary>
/// Базовый класс исключений администрирования
/// </summary>
public abstract class AdministrationException : Exception
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdministrationException"/> без параметров
    /// </summary>
    protected AdministrationException() { }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdministrationException"/> с указанием
    /// сообщения об ошибке
    /// </summary>
    protected AdministrationException(string message)
        : base(message) { }
}
