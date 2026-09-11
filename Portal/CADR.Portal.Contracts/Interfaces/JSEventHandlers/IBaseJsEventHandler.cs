using Microsoft.JSInterop;

namespace CADR.Portal.Contracts.Interfaces.JSEventHandlers;

/// <summary>
/// Базовый обработчик JavaScript событий элемента
/// </summary>
public interface IBaseJsEventHandler
{
    /// <summary>
    /// Инициализирует обработчики событий в JavaScript
    /// </summary>
    /// <remarks>Если не вызвать этот метод, то события не будут обрабатываться</remarks>
    public Task InitializeAsync(string elementId, IJSRuntime jSRuntime);
}
