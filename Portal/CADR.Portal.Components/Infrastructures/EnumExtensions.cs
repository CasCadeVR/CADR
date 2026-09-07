using System.ComponentModel;

namespace CADR.Portal.Components.Infrastructures;

static internal class EnumExtensions
{
    /// <summary>
    /// Получает значение css класса из перечисления
    /// </summary>
    public static string ToCssClass(this Enum? value)
    {
        var attr = GetAttribute<CssClassAttribute>(value);
        return attr?.ClassName ?? string.Empty;
    }

    public static string Description(this Enum? value)
    {
        var attr = GetAttribute<DescriptionAttribute>(value);
        return attr?.Description ?? string.Empty;
    }

    /// <summary>
    /// Проверяет значение перечисления и добавляет его css значение
    /// </summary>
    public static void TryAddCssValue(this Enum? value, IList<string> cssBuilder, string? prefix = null)
    {
        if (value != null)
        {
            cssBuilder.Add(string.IsNullOrEmpty(prefix)
            ? $"{value.ToCssClass()}"
            : $"{prefix}{value.ToCssClass()}");
        }
    }

    public static void TryAddCssValue<TEnum>(this TEnum value, IList<string> cssBuilder, string? prefix = null)
        where TEnum : Enum
    {
        var type = value.GetType();
        var attributes = type.GetCustomAttributes(typeof(FlagsAttribute), false);
        if (attributes.Length > 0)
        {
            foreach (var @enum in Enum.GetValues(typeof(TEnum)))
            {
                if (value.HasFlag((TEnum)@enum))
                {
                    ((Enum)@enum).TryAddCssValue(cssBuilder, prefix);
                }
            }
        }
        else
        {
            ((Enum)value).TryAddCssValue(cssBuilder, prefix);
        }
    }

    private static TAttribute? GetAttribute<TAttribute>(Enum? value)
    {
        if (value == null)
        {
            return default;
        }
        var type = value.GetType();
        var memInfo = type.GetMember(value.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(TAttribute), false);
        return attributes.OfType<TAttribute>()
            .FirstOrDefault();
    }
}
