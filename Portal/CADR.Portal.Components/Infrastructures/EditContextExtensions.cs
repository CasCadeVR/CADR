using CADR.Api.Client;
using CADR.Portal.Components.Infrastructures.Handlers;
using Microsoft.AspNetCore.Components.Forms;

namespace CADR.Portal.Components.Infrastructures;

/// <summary>
/// Методы расширения для <see cref="EditContext"/>
/// </summary>
public static class EditContextExtensions
{
    /// <summary>
    /// Добавляет обработку валидации полей формы
    /// </summary>
    public static IValidationInterceptorHandler UseValidationMessageStore(this EditContext context, ValidationMessageStore validationStore)
        => new ValidationInterceptorHandler(context, validationStore);
}

internal class ValidationInterceptorHandler : IValidationInterceptorHandler
{
    private readonly EditContext editContext;
    private readonly ValidationMessageStore validationMessageStore;

    internal ValidationInterceptorHandler(EditContext editContext, ValidationMessageStore validationMessageStore)
    {
        this.editContext = editContext;
        this.validationMessageStore = validationMessageStore;
    }

    public Task HandleAsync(ApiException<ApiValidationExceptionDetail> exception, CancellationToken cancellationToken)
    {
        if (validationMessageStore == null)
        {
            throw new ArgumentNullException(nameof(validationMessageStore));
        }

        if (!cancellationToken.IsCancellationRequested)
        {
            foreach (var resultError in exception.Result.Errors)
            {
                validationMessageStore.Add(editContext.Field(resultError.Field), resultError.Message);
            }
        }

        return Task.CompletedTask;
    }
}
