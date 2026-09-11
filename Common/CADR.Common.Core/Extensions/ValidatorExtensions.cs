using CADR.Common.Core.Contracts.Pagination;
using FluentValidation;

namespace CADR.Common.Core.Extensions;

/// <summary>
/// Методы расширения для валидаторов
/// </summary>
public static class ValidatorExtensions
{
    /// <summary>
    /// Валидировать свойства, связанные с пагинацией
    /// </summary>
    public static void ValidatePagination<TModel, TFilter>(this AbstractValidator<TModel> validator)
        where TModel : PagedCollectionRequest<TFilter>
    {
        validator.RuleFor(x => x.PageNumber)
            .GreaterThan(0);
        validator.RuleFor(x => x.PageSize)
            .GreaterThan(0);
    }
}
