using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Cadr.Context.Extensions;

/// <summary>
/// Методы расширения для <see cref="ModelBuilder"/>
/// </summary>
public static class ModelBuilderExtension
{
    /// <summary>
    /// Сканирует сборку и применяет все найденные <see cref="IEntityTypeConfiguration{TEntity}" />
    /// </summary>
    public static void ApplyAllConfigurations(this ModelBuilder modelBuilder, Assembly assembly)
    {
        var typesToRegister = assembly
            .GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(gi => gi.IsGenericType &&
                           gi.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
            .ToList();

        foreach (var type in typesToRegister)
        {
            dynamic configurationInstance = Activator.CreateInstance(type)!;
            if (configurationInstance != null)
            {
                modelBuilder.ApplyConfiguration(configurationInstance);
            }
        }
    }
}
