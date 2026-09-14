namespace CADR.Adrs.Services.Contracts.Exceptions;

/// <summary>
/// Ошибка выполнения операции
/// </summary>
public class AdrInvalidOperationException : AdrException
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrInvalidOperationException"/>
    /// с указанием сообщения об ошибке
    /// </summary>
    public AdrInvalidOperationException(string message)
        : base(message)
    {

    }
}
