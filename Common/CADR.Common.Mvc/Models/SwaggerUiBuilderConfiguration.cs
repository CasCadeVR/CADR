using System.Reflection;
using Asp.Versioning.ApiExplorer;

namespace CADR.Common.Mvc.Models;

/// <summary>
/// Конфигурирование билдеров для конечных точек swagger документации
/// </summary>
public class SwaggerUiBuilderConfiguration
{
    /// <see cref="IApiVersionDescriptionProvider"/>
    public IApiVersionDescriptionProvider? ApiVersionDescriptionProvider { get; set; }

    /// <summary>
    /// <see cref="Assembly"/> для сканирования на предмет наличия атрибутов, задающих
    /// имя группы для построения документации в Swagger
    /// </summary>
    public Assembly? TargetAssembly { get; set; }

    /// <summary>
    /// Префикс имени документации
    /// </summary>
    public string DocPrefix { get; set; } = string.Empty;

    /// <summary>
    /// Заголовок документации приложения
    /// </summary>
    public string DocName { get; set; } = string.Empty;
}
