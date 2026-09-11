using CADR.Administrations.Services.Contracts.Exceptions;
using CADR.Common.Mvc.Implementations;
using CADR.Common.Mvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CADR.Administrations.Api.Infrastructures;

/// <summary>
/// Фильтр для обработки ошибок раздела администрирования
/// </summary>
public class AdministrationExceptionFilter : BaseExceptionFilter
{
    /// <inheritdoc />
    public override void OnException(ExceptionContext context)
    {
        var exception = context.Exception as AdministrationException;
        if (exception == null)
        {
            return;
        }

        switch (exception)
        {
            case AdministrationValidationException ex:
                SetDataToContext(
                    new BadRequestObjectResult(new ApiValidationExceptionDetail { Errors = ex.Errors, })
                    {
                        StatusCode = StatusCodes.Status422UnprocessableEntity
                    },
                    context);
                break;

            case AdministrationInvalidOperationException ex:
                SetDataToContext(
                    new BadRequestObjectResult(new ApiExceptionDetail { Message = ex.Message, })
                    {
                        StatusCode = StatusCodes.Status406NotAcceptable,
                    },
                    context);
                break;

            case AdministrationNotFoundException ex:
                SetDataToContext(new NotFoundObjectResult(new ApiExceptionDetail
                {
                    Message = ex.Message,
                }), context);
                break;

            case AdministrationAccessException ex:
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
