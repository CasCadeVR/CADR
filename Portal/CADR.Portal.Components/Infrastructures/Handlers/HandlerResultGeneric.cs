namespace CADR.Portal.Components.Infrastructures.Handlers;

/// <summary>
/// Результат выполнения обработчика вызова
/// </summary>
public class HandlerResult<TResult> : HandlerResult
{
    /// <summary>
    /// Содержит результат выполнения обработчика вызова
    /// </summary>
    public TResult? Result { get; set; }

    /// <summary>
    /// Создает <see cref="HandlerResult"/> с положительным результатом
    /// </summary>
    public static HandlerResult<TResult> Succeed(TResult result)
        => new() { IsOk = true, Result = result, };

    /// <summary>
    /// Создает <see cref="HandlerResult"/> с отрицательным результатом
    /// </summary>
    public static HandlerResult<TResult> Fail(TResult? result = default)
        => new() { IsOk = false, Result = result, };
}
