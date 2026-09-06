using CADR.Common.Mvc.Builders;
using CADR.Common.Mvc.Models;
using Microsoft.IdentityModel.Tokens;

namespace CADR.Api.Tests.Client;

/// <inheritdoc />
public class BearerTokenProvider : IBearerTokenProvider
{
    private readonly string token;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="BearerTokenProvider"/>
    /// </summary>
    public BearerTokenProvider(JwtSettingsModel jwtSettings, PersonalOptions options)
    {
        var moment = DateTime.Now;
        var builderResult = SecurityTokenBuilder
            .Create(ConfigureTokenOptions(jwtSettings, moment, 10800))
            .AddPersonal(x =>
            {
                x.Login = options.Login;
                x.Identifier = options.Identifier;
                x.Name = options.Name;
                x.Email = options.Email;
                x.SecurityStamp = options.SecurityStamp;
                x.Params = options.Params;
            })
            .Build();
        token = builderResult.Token;
    }

    bool IBearerTokenProvider.HasToken => !string.IsNullOrEmpty(token);

    string IBearerTokenProvider.Token => token;

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
