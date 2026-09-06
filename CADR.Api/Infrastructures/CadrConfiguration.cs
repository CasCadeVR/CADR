namespace CADR.Api.Infrastructures;

/// <summary>
/// Конфигурирование проекта
/// </summary>
public sealed partial class CadrConfiguration
{
    private readonly IConfiguration configuration;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CadrConfiguration"/>
    /// </summary>
    public CadrConfiguration(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
}
