namespace CADR.Portal.Contracts.Exceptions;

/// <summary>
/// Ошибка выполнения операции
/// </summary>
public class PortalInvalidOperationException : PortalException
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="PortalInvalidOperationException"/>
    /// с указанием сообщения об ошибке
    /// </summary>
    public PortalInvalidOperationException(string message)
        : base(message)
    {

    }
}
