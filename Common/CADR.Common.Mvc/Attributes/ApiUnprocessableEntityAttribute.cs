using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CADR.Common.Mvc.Attributes;

/// <summary>
/// Фильтр, который определяет тип значения и код состояния 422, возвращаемый действием
/// </summary>
public class ApiUnprocessableEntityAttribute : ProducesResponseTypeAttribute
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ApiUnprocessableEntityAttribute"/>
    /// </summary>
    public ApiUnprocessableEntityAttribute()
        : base(StatusCodes.Status422UnprocessableEntity)
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ApiUnprocessableEntityAttribute"/> со
    /// значением поля <see cref="Type"/>
    /// </summary>
    public ApiUnprocessableEntityAttribute(Type type)
        : base(type, StatusCodes.Status422UnprocessableEntity)
    {
    }
}
