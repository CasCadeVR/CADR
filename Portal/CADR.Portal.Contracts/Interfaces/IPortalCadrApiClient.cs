using CADR.Api.Client;

namespace CADR.Portal.Contracts.Interfaces;

/// <summary>
/// Клиент для взаимодействия с API Cadr.
/// </summary>
public interface IPortalCadrApiClient : ICadrApiClient
{
    /// <summary>
    /// Задаёт значение, указывающее, что запросы к API Cadr не требуют авторизации.
    /// </summary>
    IPortalCadrApiClient AllowAnonymous(bool isAnonymous = true);
}
