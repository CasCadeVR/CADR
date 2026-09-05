namespace Cadr.Context;

/// <summary>
/// Конфигурирование контекста
/// </summary>
public interface ICadrContextConfiguration
{
    /// <summary>
    /// Строка подключения
    /// </summary>
    string ConnectionString { get; }
}
