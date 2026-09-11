using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CADR.Common.Mvc.Attributes;

/// <summary>
/// Фильтр, который определяет тип значения и код состояния 201, возвращаемый действием
/// </summary>
public class ApiCreatedAttribute : ProducesResponseTypeAttribute
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ApiCreatedAttribute"/>
    /// </summary>
    public ApiCreatedAttribute()
        : base(StatusCodes.Status201Created)
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ApiCreatedAttribute"/> со значением поля <see cref="Type"/>
    /// </summary>
    public ApiCreatedAttribute(Type type)
        : base(type, StatusCodes.Status201Created)
    {
    }
}
