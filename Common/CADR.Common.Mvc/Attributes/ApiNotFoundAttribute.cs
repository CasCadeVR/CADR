using CADR.Common.Mvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CADR.Common.Mvc.Attributes;

/// <summary>
/// Фильтр, который определяет тип значения и код состояния 404, возвращаемый действием
/// </summary>
public class ApiNotFoundAttribute : ProducesResponseTypeAttribute
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ApiNotFoundAttribute"/>
    /// </summary>
    public ApiNotFoundAttribute() : this(typeof(ApiExceptionDetail))
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ApiNotFoundAttribute"/>
    /// </summary>
    public ApiNotFoundAttribute(Type type)
        : base(type, StatusCodes.Status404NotFound)
    {
    }
}
