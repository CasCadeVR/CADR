using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CADR.Common.Mvc.Implementations;

/// <summary>
/// Базовый фильтр для обработки ошибок
/// </summary>
public abstract class BaseExceptionFilter : IExceptionFilter
{
    /// <inheritdoc cref="IExceptionFilter.OnException"/>
    public abstract void OnException(ExceptionContext context);

    /// <summary>
    /// Определяет контекст ответа
    /// </summary>
    static protected void SetDataToContext(ObjectResult data, ExceptionContext context)
    {
        context.ExceptionHandled = true;
        var response = context.HttpContext.Response;
        response.StatusCode = data.StatusCode ?? StatusCodes.Status400BadRequest;
        context.Result = data;
    }
}
