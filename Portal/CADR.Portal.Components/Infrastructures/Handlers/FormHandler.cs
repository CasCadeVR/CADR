using CADR.Api.Client;

namespace CADR.Portal.Components.Infrastructures.Handlers;

/// <summary>
/// Обработчик вызовов
/// </summary>
public class FormHandler
{
    private Action<HandlerExceptionArgs>? handlerExceptions;
    private IValidationInterceptorHandler? validationInterceptorHandler;
    private Action? finallyAction;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="FormHandler"/>
    /// </summary>
    private FormHandler()
    {
        handlerExceptions = null;
        validationInterceptorHandler = null;
        finallyAction = null;
    }

    /// <summary>
    /// Создаёт <see cref="FormHandler"/>
    /// </summary>
    public static FormHandler Create() => new();

    /// <summary>
    /// Добавляет обработку ошибок
    /// </summary>
    public FormHandler WithExceptions(Action<HandlerExceptionArgs> exceptionAction)
    {
        handlerExceptions = exceptionAction;
        return this;
    }

    /// <summary>
    /// Добавляет обязательную обработку завершения целевого действия
    /// </summary>
    public FormHandler WithFinally(Action action)
    {
        finallyAction = action;
        return this;
    }

    /// <summary>
    /// Использовать обработку валидации
    /// </summary>
    public FormHandler WithValidation(IValidationInterceptorHandler handler)
    {
        validationInterceptorHandler = handler;
        return this;
    }

    /// <summary>
    /// Выполняет указанное асинхронное действие 
    /// </summary>
    public async Task<HandlerResult> ExecuteAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken)
        => await ExecuteAsync(async (token) =>
        {
            await action.Invoke(token);
            return true;
        }, cancellationToken);

    /// <summary>
    /// Выполняет указанное асинхронное действие и возвращает результат <see langword="TResult"/>
    /// </summary>
    public async Task<HandlerResult<TResult>> ExecuteAsync<TResult>(Func<CancellationToken, Task<TResult>> action, CancellationToken cancellationToken)
    {
        try
        {
            var result = await action.Invoke(cancellationToken);
            return HandlerResult<TResult>.Succeed(result);
        }
        catch (ApiException<ApiExceptionDetail> e)
        {
            var args = new HandlerExceptionArgs { Message = e.Result.Message, Source = e, };
            handlerExceptions?.Invoke(args);
        }
        catch (ApiException<ApiValidationExceptionDetail> e)
        {
            if (validationInterceptorHandler != null)
            {
                await validationInterceptorHandler.HandleAsync(e, cancellationToken);
            }
        }
        catch (Exception e)
        {
            var args = new HandlerExceptionArgs { Message = e.Message, Source = e, };
            handlerExceptions?.Invoke(args);
        }
        finally
        {
            finallyAction?.Invoke();
        }

        return HandlerResult<TResult>.Fail();
    }
}
