using System.Security.Claims;
using CADR.Common.Core.Contracts;
using Microsoft.IdentityModel.JsonWebTokens;

namespace CADR.Api.Infrastructures;

/// <summary>
/// Поставщик опознания личности
/// </summary>
public class ApiIdentityProvider : IIdentityProvider
{
    private readonly IEnumerable<Claim> claims;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ApiIdentityProvider"/>
    /// </summary>
    public ApiIdentityProvider(IHttpContextAccessor httpContextAccessor)
    {
        claims = httpContextAccessor?.HttpContext?.User.Claims ?? Array.Empty<Claim>();
    }

    /// <inheritdoc cref="IIdentityProvider.Name"/>
    public string Name => claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name)?.Value ?? "Anonymous";

    /// <inheritdoc cref="IIdentityProvider.Id"/>
    public Guid Id => Guid.TryParse(claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value, out var value)
        ? value
        : Guid.Empty;

    /// <inheritdoc cref="IIdentityProvider.Claims"/>
    public IEnumerable<KeyValuePair<string, string>> Claims
        => claims.Select(x => new KeyValuePair<string, string>(x.Type, x.Value));
}
