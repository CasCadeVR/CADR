using CADR.Common.Core.Contracts.Models;

namespace CADR.Administrations.Services.Contracts.Exceptions;

/// <summary>
/// Ошибки валидации
/// </summary>
public class AdministrationValidationException : AdministrationException
{
    /// <summary>
    /// Ошибки
    /// </summary>
    public IEnumerable<InvalidateItemModel> Errors { get; }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdministrationValidationException"/>
    /// </summary>
    public AdministrationValidationException(IEnumerable<InvalidateItemModel> errors)
    {
        Errors = errors;
    }
}
