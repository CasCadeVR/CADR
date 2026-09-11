using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using CADR.Administrations.Services.Contracts.Models.User;
using CADR.Common.Mvc.Builders;
using CADR.Common.Mvc.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace CADR.Administrations.Api.Infrastructures;

/// <summary>
/// Вспомогательный класс аутентификации для jwt токена
/// </summary>
public class SecurityTokenManager
{
    /// <summary>
    /// Создаёт токен доступа
    /// </summary>
    public static SecurityTokenBuilderResult CreateToken(JwtSettingsModel authSetting,
        [NotNull] UserLoggedModel user,
        bool withRefreshToken)
    {
        var moment = DateTime.Now;
        var builderResult = SecurityTokenBuilder
            .Create(ConfigureTokenOptions(authSetting, moment, withRefreshToken
                    ? authSetting.LifeTimeSec
                    : authSetting.NotPersistentSecondValue))
            .AddPersonal(x =>
            {
                x.Login = user.Login;
                x.Identifier = user.Id;
                x.Name = user.Name;
                x.Email = user.Email;
                x.SecurityStamp = user.SecurityStamp;
                x.Params = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            })
            .Build();
        return builderResult;
    }

    /// <summary>
    /// Создаёт токен доступа на основе существующих клеймов
    /// </summary>
    public static SecurityTokenBuilderResult CreateTokenByClaims(JwtSettingsModel authSetting,
        string serializedClaims)
    {
        var moment = DateTime.Now;
        var claims = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string, string>>>(serializedClaims);
        var builderResult = SecurityTokenBuilder
            .Create(ConfigureTokenOptions(authSetting, moment, authSetting.LifeTimeSec))
            .AddClaims(claims!
            .Select(x => new Claim(x.Key, x.Value)))
            .Build();

        return builderResult;
    }

    /// <summary>
    /// Получает отметку безопасности из набора клеймов
    /// </summary>
    public static string GetSecurityStamp(IEnumerable<KeyValuePair<string, string>> claims)
        => claims.FirstOrDefault(x => x.Key == JwtRegisteredClaimNames.AtHash).Value;

    private static Action<SecurityTokenOptions> ConfigureTokenOptions(JwtSettingsModel authSetting,
        DateTime moment,
        int expiresSecond)
        => opt =>
        {
            opt.Audience = authSetting.Audience;
            opt.Issuer = authSetting.Issuer;
            opt.SecretKey = Base64UrlEncoder.DecodeBytes(authSetting.SecretKeyBase64);
            opt.SignKey = Base64UrlEncoder.DecodeBytes(authSetting.SignKeyBase64);
            opt.NotBefore = moment;
            opt.Expires = moment.AddSeconds(expiresSecond);
        };
}
