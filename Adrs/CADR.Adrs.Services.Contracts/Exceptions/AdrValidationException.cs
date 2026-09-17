using CADR.Common.Core.Contracts.Models;

namespace CADR.Adrs.Services.Contracts.Exceptions;

/// <summary>
/// Ошибки валидации
/// </summary>
public class AdrValidationException : AdrException
{
    /// <summary>
    /// Ошибки
    /// </summary>
    public IEnumerable<InvalidateItemModel> Errors { get; }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrValidationException"/>
    /// </summary>
    public AdrValidationException(IEnumerable<InvalidateItemModel> errors)
    {
        Errors = errors;
    }
}
