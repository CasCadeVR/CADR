using CADR.Portal.Components.Infrastructures.Handlers;
using CADR.Portal.Components.Models.Enums;

namespace CADR.Portal.Components.Infrastructures;

/// <inheritdoc cref="IMessageProvider"/>
public class MessageProvider : IMessageProvider, IMessageBox
{
    /// <inheritdoc cref="IMessageProvider"/>
    public event Func<MessageEventArgs, CancellationToken, Task<DialogResult>>? OnMessageReceived;

    /// <inheritdoc cref="IMessageBox"/>
    public Task<DialogResult> Show(string text, CancellationToken cancellationToken)
        => Show(text, "Информация", cancellationToken);

    /// <inheritdoc cref="IMessageBox"/>
    public Task<DialogResult> Show(string text, string caption, CancellationToken cancellationToken)
        => Show(text, caption, MessageBoxButtons.Ok, cancellationToken);

    /// <inheritdoc cref="IMessageBox"/>
    public Task<DialogResult> Show(string text, string caption, MessageBoxButtons buttons, CancellationToken cancellationToken)
        => Show(text, caption, MessageBoxButtons.Ok, MessageBoxIcon.Information, cancellationToken);

    /// <inheritdoc cref="IMessageBox"/>
    public Task<DialogResult> Show(string text,
        string caption,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        CancellationToken cancellationToken)
        => OnMessageReceived?.Invoke(new MessageEventArgs
        {
            Text = text,
            Caption = caption,
            Buttons = buttons,
            Icon = icon,
        }, cancellationToken)
           ?? Task.FromResult(DialogResult.Ok);
}
