namespace CADR.Portal.Contracts.Interfaces.JSEventHandlers;

/// <summary>
/// Обработчик JavaScript событий элемента Dropdown
/// </summary>
public interface IJsDropdownEventHandler : IBaseJsEventHandler
{
    /// <summary>
    /// Обработчик открытия dropdown
    /// </summary>
    public Func<Task>? OnOpenHandlerAsync { get; set; }
}
