namespace CADR.Common.Mvc.Models;

/// <summary>
/// Конфигурирование билдеров для swagger документации
/// </summary>
public class SwaggerBuilderConfiguration : SwaggerUiBuilderConfiguration
{
    /// <summary>
    /// Короткое описание приложения
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
