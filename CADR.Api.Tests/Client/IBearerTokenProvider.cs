namespace CADR.Api.Tests.Client;

/// <summary>
/// Поставщик токена доступа для тестовой АПИ
/// </summary>
public interface IBearerTokenProvider
{
    /// <summary>
    /// Проверяет наличие токена
    /// </summary>
    bool HasToken { get; }

    /// <summary>
    /// Токен доступа
    /// </summary>
    string Token { get; }
}
