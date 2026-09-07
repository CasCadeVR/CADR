using CADR.Portal.Components.Models.Enums;

namespace CADR.Portal.Components.Infrastructures.Handlers;

/// <summary>
/// Аргументы события показа сообщения
/// </summary>
public class MessageEventArgs : EventArgs
{
    /// <summary>
    /// Создаёт <see cref="MessageEventArgs"/> со значениями по умолчанию
    /// </summary>
    public static MessageEventArgs Default
        => new()
        {
            Buttons = MessageBoxButtons.Ok,
            Icon = MessageBoxIcon.Information,
            Text = string.Empty,
            Caption = string.Empty,
        };

    /// <summary>
    /// Текст, отображаемый в окне сообщения
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Текст для отображения в строке заголовка окна сообщения
    /// </summary>
    public string Caption { get; set; } = string.Empty;

    /// <summary>
    /// Одно из значений <see cref="MessageBoxButtons"/>, указывающее, какие кнопки отображаются в окне сообщения
    /// </summary>
    public MessageBoxButtons Buttons { get; set; }

    /// <summary>
    /// Одно из значений <see cref="MessageBoxIcon"/>, указывающее, какой значок отображается в окне сообщения
    /// </summary>
    public MessageBoxIcon Icon { get; set; }
}
