using CADR.Adrs.Services.Contracts.Exceptions;
using CADR.Common.Mvc.Implementations;
using CADR.Common.Mvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CADR.Adrs.Api.Infrastructures;

/// <summary>
/// Фильтр для обработки ошибок раздела ADR
/// </summary>
public class AdrsExceptionFilter : BaseExceptionFilter
{
    /// <inheritdoc />
    public override void OnException(ExceptionContext context)
    {
        var exception = context.Exception as AdrException;
        if (exception == null)
        {
            return;
        }

        switch (exception)
        {
            case AdrValidationException ex:
                SetDataToContext(
                    new BadRequestObjectResult(new ApiValidationExceptionDetail { Errors = ex.Errors, })
                    {
                        StatusCode = StatusCodes.Status422UnprocessableEntity
                    },
                    context);
                break;

            case AdrInvalidOperationException ex:
                SetDataToContext(
                    new BadRequestObjectResult(new ApiExceptionDetail { Message = ex.Message, })
                    {
                        StatusCode = StatusCodes.Status406NotAcceptable,
                    },
                    context);
                break;

            case AdrNotFoundException ex:
                SetDataToContext(new NotFoundObjectResult(new ApiExceptionDetail
                {
                    Message = ex.Message,
                }), context);
                break;

            case AdrAccessException ex:
                SetDataToContext(new BadRequestObjectResult(new ApiExceptionDetail { Message = ex.Message, })
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                }, context);
                break;

            default:
                SetDataToContext(new BadRequestObjectResult(new ApiExceptionDetail
                {
                    Message = exception.Message,
                }), context);
                break;
        }
    }
}
