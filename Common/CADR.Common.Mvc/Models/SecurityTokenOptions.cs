using Microsoft.IdentityModel.Tokens;

namespace CADR.Common.Mvc.Models;

/// <summary>
/// Класс представляет информацию, необходимую для управления поведением
/// при создании <see cref="SecurityToken"/>
/// </summary>
public class SecurityTokenOptions
{
    /// <summary>
    /// Issuer
    /// </summary>
    public string? Issuer { get; set; }

    /// <summary>
    /// Audience
    /// </summary>
    public string? Audience { get; set; }

    /// <summary>
    /// Key uses in hash function like SHA256
    /// </summary>
    public byte[] SignKey { get; set; } = Array.Empty<byte>();

    /// <summary>
    /// 256 bits key uses for symmetric cryptography
    /// </summary>
    public byte[] SecretKey { get; set; } = Array.Empty<byte>();

    /// <summary>Gets or sets the value of the 'expiration' claim.</summary>
    public DateTime? Expires { get; set; }

    /// <summary>
    /// Gets or sets the notbefore time for the security token.
    /// </summary>
    public DateTime? NotBefore { get; set; }
}
