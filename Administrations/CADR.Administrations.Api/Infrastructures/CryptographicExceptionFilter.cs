using System.Security.Cryptography;
using CADR.Administrations.Services.Resources;
using CADR.Common.Mvc.Implementations;
using CADR.Common.Mvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CADR.Administrations.Api.Infrastructures;

/// <summary>
/// Фильтр для обработки ошибок криптографии
/// </summary>
public class CryptographicExceptionFilter : BaseExceptionFilter
{
    /// <inheritdoc />
    public override void OnException(ExceptionContext context)
    {
        var exception = context.Exception as CryptographicException;
        if (exception == null)
        {
            return;
        }

        switch (context.Exception)
        {
            case CryptographicException ex:
                SetDataToContext(
                    new BadRequestObjectResult(new ApiExceptionDetail { Message = ErrorMessages.RefreshTokenIsInvalid, })
                    {
                        StatusCode = StatusCodes.Status422UnprocessableEntity
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
