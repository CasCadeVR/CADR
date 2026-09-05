using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CADR.Common.Mvc.Attributes;

/// <summary>
/// Фильтр, который определяет тип значения и код состояния 200, возвращаемый действием
/// </summary>
public class ApiOkAttribute : ProducesResponseTypeAttribute
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ApiOkAttribute"/>
    /// </summary>
    public ApiOkAttribute()
        : base(StatusCodes.Status200OK)
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ApiOkAttribute"/> со значением поля <see cref="Type"/>
    /// </summary>
    public ApiOkAttribute(Type type)
        : base(type, StatusCodes.Status200OK)
    {
    }
}
