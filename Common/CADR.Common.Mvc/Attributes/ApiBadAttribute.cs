using CADR.Common.Mvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CADR.Common.Mvc.Attributes;

/// <summary>
/// Фильтр, который определяет тип значения и код состояния 400, возвращаемый действием
/// </summary>
public class ApiBadAttribute : ProducesResponseTypeAttribute
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ApiBadAttribute"/>
    /// </summary>
    public ApiBadAttribute() : this(typeof(ApiExceptionDetail))
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ApiBadAttribute"/>
    /// </summary>
    public ApiBadAttribute(Type type)
        : base(type, StatusCodes.Status400BadRequest)
    {
    }
}
