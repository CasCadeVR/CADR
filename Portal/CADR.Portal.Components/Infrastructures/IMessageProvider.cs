using CADR.Portal.Components.Infrastructures.Handlers;
using CADR.Portal.Components.Models.Enums;

namespace CADR.Portal.Components.Infrastructures;

/// <summary>
/// Поставщик диалоговых сообщений
/// </summary>
public interface IMessageProvider
{
    /// <summary>
    /// Событие отправки сообщения об показе диалогового окна
    /// </summary>
    event Func<MessageEventArgs, CancellationToken, Task<DialogResult>> OnMessageReceived;
}
