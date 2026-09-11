using System.ComponentModel;

namespace CADR.Portal.Components.Enums;

/// <summary>
/// Определяет как браузер должен загружать изображение
/// </summary>
public enum ImageLoading
{
    /// <summary>
    /// Загружает изображение немедленно
    /// </summary>
    [Description("eager")]
    Eager,

    /// <summary>
    /// Откладывает загрузку изображения до тех пор, пока оно
    /// не достигнет расчетного расстояния от области просмотра
    /// </summary>
    [Description("lazy")]
    Lazy
}
