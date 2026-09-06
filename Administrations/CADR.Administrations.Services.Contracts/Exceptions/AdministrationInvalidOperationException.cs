namespace CADR.Administrations.Services.Contracts.Exceptions;

/// <summary>
/// Ошибка выполнения операции
/// </summary>
public class AdministrationInvalidOperationException : AdministrationException
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdministrationInvalidOperationException"/>
    /// с указанием сообщения об ошибке
    /// </summary>
    public AdministrationInvalidOperationException(string message)
        : base(message)
    {

    }
}
