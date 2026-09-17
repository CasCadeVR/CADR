namespace CADR.Adrs.Services.Contracts.Exceptions;

/// <summary>
/// Запрашиваемый ресурс не найден
/// </summary>
public class AdrNotFoundException : AdrException
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrNotFoundException"/> с указанием
    /// сообщения об ошибке
    /// </summary>
    public AdrNotFoundException(string message)
        : base(message)
    { }
}
