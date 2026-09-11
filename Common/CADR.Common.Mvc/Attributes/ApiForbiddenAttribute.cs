using CADR.Common.Mvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CADR.Common.Mvc.Attributes;

/// <summary>
/// Фильтр, который определяет тип значения и код состояния 403, возвращаемый действием
/// </summary>
/// <remarks>Доступ запрещён и привязан к логике приложения (например, у пользователя не хватает прав доступа к запрашиваемому ресурсу)</remarks>
public class ApiForbiddenAttribute : ProducesResponseTypeAttribute
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ApiForbiddenAttribute"/>
    /// </summary>
    public ApiForbiddenAttribute() : this(typeof(ApiExceptionDetail))
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ApiForbiddenAttribute"/> со
    /// значением поля <see cref="Type"/>
    /// </summary>
    public ApiForbiddenAttribute(Type type)
        : base(type, StatusCodes.Status403Forbidden)
    {
    }
}
