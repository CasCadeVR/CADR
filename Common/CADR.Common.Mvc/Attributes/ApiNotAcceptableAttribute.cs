using CADR.Common.Mvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CADR.Common.Mvc.Attributes;

/// <summary>
/// Фильтр, который определяет тип значения и код состояния 406, возвращаемый действием
/// </summary>
/// <remarks>Атрибут исключений <see labgword="InvalidOperationException"/></remarks>
public class ApiNotAcceptableAttribute : ProducesResponseTypeAttribute
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ApiNotAcceptableAttribute"/>
    /// </summary>
    public ApiNotAcceptableAttribute() : this(typeof(ApiExceptionDetail))
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ApiNotAcceptableAttribute"/>
    /// </summary>
    public ApiNotAcceptableAttribute(Type type)
        : base(type, StatusCodes.Status406NotAcceptable)
    {
    }
}
