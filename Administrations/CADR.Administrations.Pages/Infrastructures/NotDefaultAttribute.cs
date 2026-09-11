using System.ComponentModel.DataAnnotations;

namespace CADR.Administrations.Pages.Infrastructures;

/// <summary>
/// Значения не равны
/// </summary>
public class NotDefaultAttribute : ValidationAttribute
{
    /// <inheritdoc />
    public override bool IsValid(object? value) => !IsDefault(value);

    private static bool IsDefault(object? o)
    {
        if (o == null) // => ссылочный тип или nullable
        {
            return true;
        }
        var type = o.GetType();
        if (Nullable.GetUnderlyingType(type) != null) // nullable, не null
        {
            return false;
        }
        if (type.IsClass)
        {
            return false;
        }
        // => тип-значение, есть конструктор по умолчанию
        return Activator.CreateInstance(type)?.Equals(o) is true;
    }
}
