using System.ComponentModel;

namespace CADR.Portal.Components.Enums.Button;

/// <summary>
/// Тип кнопки
/// </summary>
public enum ButtonTypes
{
    /// <summary>Обычная кнопка</summary>
    [Description("button")]
    Button,

    /// <summary>Кнопка для очистки введенных данных формы и возвращения значений в первоначальное состояние</summary>
    [Description("reset")]
    Reset,

    /// <summary>Кнопка для отправки данных формы на сервер</summary>
    [Description("submit")]
    Submit,
}
