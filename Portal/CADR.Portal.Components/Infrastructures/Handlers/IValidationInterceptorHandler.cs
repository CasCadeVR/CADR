using CADR.Api.Client;

namespace CADR.Portal.Components.Infrastructures.Handlers;

/// <summary>
/// Перехватчик обработки ошибок валидации
/// </summary>
public interface IValidationInterceptorHandler
{
    /// <summary>
    /// Обработка ошибок
    /// </summary>
    Task HandleAsync(ApiException<ApiValidationExceptionDetail> exception, CancellationToken cancellationToken);
}
