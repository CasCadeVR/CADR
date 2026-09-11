namespace CADR.Administrations.Services.Contracts.Exceptions;

/// <summary>
/// Запрашиваемый ресурс не найден
/// </summary>
public class AdministrationNotFoundException : AdministrationException
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdministrationNotFoundException"/> с указанием
    /// сообщения об ошибке
    /// </summary>
    public AdministrationNotFoundException(string message)
        : base(message)
    { }
}
