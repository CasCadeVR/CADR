using CADR.Api.Client;

namespace CADR.Api.Tests.Client;

/// <summary>
/// Клиент для интеграционных тестирований
/// </summary>
public interface ICadrApiTestClient : ICadrApiClient
{
    /// <summary>
    /// Получить Cookie из последнего ответа
    /// </summary>
    IEnumerable<string> GetCookieHeadersFromLastResponse();

    /// <summary>
    /// Установить Cookie в запрос
    /// </summary>
    void SetCookies(string cookies);
}
