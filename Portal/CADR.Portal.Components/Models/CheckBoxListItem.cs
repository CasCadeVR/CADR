using CADR.Portal.Components.Controls;

namespace CADR.Portal.Components.Models;

/// <summary>
/// Элемент списка <see cref="CheckboxList{T}"/>
/// </summary>
internal class CheckBoxListItem<T>(T value)
{
    /// <summary>
    /// Значение объекта
    /// </summary>
    public T Value { get; set; } = value;

    /// <summary>
    /// Выбран ли элемент
    /// </summary>
    public bool Checked { get; set; }
}
