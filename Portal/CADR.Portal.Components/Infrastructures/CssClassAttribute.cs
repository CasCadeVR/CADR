namespace CADR.Portal.Components.Infrastructures;

/// <summary>
/// Определяет css значение для элемента
/// </summary>
internal class CssClassAttribute : Attribute
{
    /// <summary>
    /// Имя css класса или классов через пробел
    /// </summary>
    public string ClassName { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CssClassAttribute"/>
    /// </summary>
    public CssClassAttribute(string className)
    {
        ClassName = className;
    }
}
