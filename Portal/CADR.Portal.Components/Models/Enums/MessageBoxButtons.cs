namespace CADR.Portal.Components.Models.Enums;

/// <summary>
/// Задает константы, определяющие кнопки, которые нужно отображать
/// </summary>
public enum MessageBoxButtons
{
    /// <summary>
    /// Окно сообщения содержит кнопку "ОК".
    /// </summary>
    Ok = 0,

    /// <summary>
    /// Окно сообщения содержит кнопки "ОК" и "Отмена"
    /// </summary>
    OkCancel = 1,

    /// <summary>
    /// Окно сообщения содержит кнопки "Да", "Нет" и "Отмена"
    /// </summary>
    YesNoCancel = 2,

    /// <summary>
    /// Окно сообщения содержит кнопки "Да" и "Нет"
    /// </summary>
    YesNo = 3,
}
