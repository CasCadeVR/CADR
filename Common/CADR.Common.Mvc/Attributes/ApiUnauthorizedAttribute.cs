using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CADR.Common.Mvc.Attributes;

/// <summary>
/// Фильтр, который определяет тип значения и код состояния 401, возвращаемый действием
/// </summary>
public class ApiUnauthorizedAttribute : ProducesResponseTypeAttribute
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ApiUnauthorizedAttribute"/>
    /// </summary>
    public ApiUnauthorizedAttribute()
        : base(StatusCodes.Status401Unauthorized)
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ApiUnauthorizedAttribute"/> со
    /// значением поля <see cref="Type"/>
    /// </summary>
    public ApiUnauthorizedAttribute(Type type)
        : base(type, StatusCodes.Status401Unauthorized)
    {
    }
}
