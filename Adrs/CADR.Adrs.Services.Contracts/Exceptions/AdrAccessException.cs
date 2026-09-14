namespace CADR.Adrs.Services.Contracts.Exceptions;

/// <summary>
/// Ошибка доступа к действию
/// </summary>
public class AdrAccessException : AdrException
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrAccessException"/>
    /// с указанием сообщения об ошибке
    /// </summary>
    public AdrAccessException()
        : base("У вас нет прав для выполнения действия")
    {

    }
}
