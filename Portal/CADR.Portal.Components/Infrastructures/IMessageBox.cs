using CADR.Portal.Components.Models.Enums;

namespace CADR.Portal.Components.Infrastructures;

/// <summary>
/// Отображает окно сообщения (диалоговое окно) с текстом для пользователя.
/// Это модальное окно, блокирующее другие действия в приложении, пока пользователь
/// не закроет его. IMessageBox может содержать текст, кнопки и
/// символы для отображения пользователю информации и инструкций
/// </summary>
public interface IMessageBox
{
    /// <summary>
    /// Отображает окно сообщения с заданным текстом
    /// </summary>
    Task<DialogResult> Show(string text, CancellationToken cancellationToken);

    /// <summary>
    /// Отображает окно сообщения с заданным текстом
    /// </summary>
    Task<DialogResult> Show(string text, string caption, CancellationToken cancellationToken);

    /// <summary>
    /// Отображает окно сообщения с заданным текстом
    /// </summary>
    Task<DialogResult> Show(string text,
        string caption,
        MessageBoxButtons buttons,
        CancellationToken cancellationToken);

    /// <summary>
    /// Отображает окно сообщения с заданным текстом
    /// </summary>
    Task<DialogResult> Show(string text,
        string caption,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        CancellationToken cancellationToken);
}
