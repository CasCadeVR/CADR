using CADR.Portal.Components.Models.Enums;

namespace CADR.Portal.Components.Models;

/// <summary>
/// Модель всплывающего сообщения
/// </summary>
public struct Toast
{
    /// <inheritdoc cref="ToastTypes"/>
    public ToastTypes ToastType { get; }

    /// <summary>
    /// Сообщение
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Количество сообщений
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Toast"/>
    /// </summary>
    public Toast(ToastTypes toastType, string message, int count = 0)
    {
        ToastType = toastType;
        Message = message;
        Count = count;
    }

    /// <inheritdoc/>
    public readonly override int GetHashCode() => (ToastType, Message).GetHashCode();
}
