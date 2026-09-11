using CADR.Portal.Contracts.Interfaces.JSEventHandlers;
using Microsoft.JSInterop;

namespace CADR.Portal.Infrastructures.JSInvokable;

/// <summary>
/// Обработчик JavaScript событий элемента Dropdown
/// </summary>
public sealed class JsDropdownEventHandler : IJsDropdownEventHandler, IDisposable
{
    private bool isInitialized;

    private readonly DotNetObjectReference<JsDropdownEventHandler> dotNetObjectReference;

    /// <inheritdoc/>
    public Func<Task>? OnOpenHandlerAsync { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="JsDropdownEventHandler"/>
    /// </summary>
    public JsDropdownEventHandler()
    {
        dotNetObjectReference = DotNetObjectReference.Create(this);
    }

    async Task IBaseJsEventHandler.InitializeAsync(string elementId, IJSRuntime jSRuntime)
    {
        if (!isInitialized)
        {
            await jSRuntime.InvokeVoidAsync("InitializeDropdownEventHandler", elementId, dotNetObjectReference);
            isInitialized = true;
        }
    }

    /// <summary>
    /// Внутренний обработчик события открытия Dropdown
    /// </summary>
    /// <remarks>Нельзя вызывать метод вне <see cref="JsDropdownEventHandler"/></remarks>
    [JSInvokable]
    public void InternalOnOpenHandler()
    {
        OnOpenHandlerAsync?.Invoke();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        dotNetObjectReference.Dispose();
    }
}
