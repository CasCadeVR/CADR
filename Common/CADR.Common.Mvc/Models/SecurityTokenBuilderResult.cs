using System.Security.Claims;
using CADR.Common.Mvc.Builders;

namespace CADR.Common.Mvc.Models;

/// <summary>
/// Результат работы <see cref="SecurityTokenBuilder"/>
/// </summary>
public class SecurityTokenBuilderResult
{
    /// <summary>
    /// JWT
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Список <see cref="Claim"/>
    /// </summary>
    public IEnumerable<Claim> Claims { get; set; } = Array.Empty<Claim>();
}
