namespace CADR.Administrations.Services.Contracts.Exceptions;

/// <summary>
/// Ошибка доступа к действию
/// </summary>
public class AdministrationAccessException : AdministrationException
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdministrationAccessException"/>
    /// с указанием сообщения об ошибке
    /// </summary>
    public AdministrationAccessException()
        : base("У вас нет прав для выполнения действия")
    {

    }
}
