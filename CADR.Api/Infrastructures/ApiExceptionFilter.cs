using CADR.Common.Mvc.Implementations;
using CADR.Common.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CADR.Api.Infrastructures;

/// <summary>
/// Фильтр для обработки общих ошибок
/// </summary>
public class ApiExceptionFilter : BaseExceptionFilter
{
    /// <inheritdoc />
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is UnauthorizedAccessException exception)
        {
            SetDataToContext(new UnauthorizedObjectResult(new ApiExceptionDetail
            {
                Message = exception.Message,
            }), context);
        }
    }
}
