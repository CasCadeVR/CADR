namespace CADR.Common.Core.Contracts;

/// <summary>
/// Базовая функциональность идентификации пользователя
/// </summary>
public interface IIdentityProvider
{
    /// <summary>
    /// Возвращает имя текущего пользователя
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Клеймы
    /// </summary>
    IEnumerable<KeyValuePair<string, string>> Claims { get; }
}
