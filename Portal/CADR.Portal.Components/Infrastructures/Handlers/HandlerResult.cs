namespace CADR.Portal.Components.Infrastructures.Handlers;

/// <summary>
/// Результат выполнения обработчика вызова
/// </summary>
public class HandlerResult
{
    /// <summary>
    /// Содержит <see langword="true"/>, если в результате выполнения не было ошибок,
    /// в противном случае <see langword="false"/>
    /// </summary>
    public bool IsOk { get; set; }
}
