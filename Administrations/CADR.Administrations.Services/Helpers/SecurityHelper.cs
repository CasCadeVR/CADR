using System.Security.Cryptography;

namespace CADR.Administrations.Services.Helpers;

/// <summary>
/// Хелпер по работе с криптографией
/// </summary>
public class SecurityHelper
{
    private const byte MaximumBits = 32;
    private const byte MaximumIterations = 6;

    /// <summary>
    /// Генерировать соль в 32 бита
    /// </summary>
    public static string GenerateSalt32() => GenerateSalt(MaximumBits);

    /// <summary>
    /// Генерировать соль заданного размера
    /// </summary>
    public static string GenerateSalt(int nSalt)
    {
        using var provider = RandomNumberGenerator.Create();
        var salt = new byte[nSalt];
        provider.GetNonZeroBytes(salt);

        return Convert.ToBase64String(salt);
    }

    /// <summary>
    /// Получить солированный хэш пароля в 32 бита
    /// </summary>
    public static string HashPassword32(string password, string salt)
        => HashPassword(password, salt, MaximumIterations, MaximumBits);

    /// <summary>
    /// Получить солированный хэш пароля
    /// </summary>
    public static string HashPassword(string password, string salt, int nIterations, int nHash)
    {
        var saltBytes = Convert.FromBase64String(salt);
        return Convert.ToBase64String(Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, nIterations, HashAlgorithmName.SHA256, nHash));
    }
}
