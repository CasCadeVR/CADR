using CADR.Common.Mvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CADR.Common.Mvc.Attributes;

/// <summary>
/// Фильтр, который определяет тип значения и код состояния 422, возвращаемый действием
/// </summary>
/// <remarks>Ошибки валидации</remarks>
public class ApiValidationAttribute : ProducesResponseTypeAttribute
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ApiNotAcceptableAttribute"/>
    /// </summary>
    public ApiValidationAttribute() : base(typeof(ApiValidationExceptionDetail), StatusCodes.Status422UnprocessableEntity)
    {
    }
}
