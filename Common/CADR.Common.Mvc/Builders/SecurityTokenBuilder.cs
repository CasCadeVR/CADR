using System.Security.Claims;
using CADR.Common.Mvc.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace CADR.Common.Mvc.Builders;

/// <summary>
/// Построитель для <see cref="SecurityToken"/>
/// </summary>
public class SecurityTokenBuilder
{
    private Action<PersonalOptions>? actionPersonalOptions;
    private Action<SecurityTokenOptions>? actionSecurityTokenOptions;
    private IEnumerable<Claim> personalClaim = [];

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="SecurityTokenBuilder"/>
    /// </summary>
    internal SecurityTokenBuilder() { }

    internal void SetActionSecurityTokenOptions(Action<SecurityTokenOptions> action)
    {
        actionSecurityTokenOptions = action;
    }

    /// <summary>
    /// Добавляет <see cref="PersonalOptions"/> для формирования <see cref="SecurityToken"/>
    /// </summary>
    public SecurityTokenBuilder AddPersonal(Action<PersonalOptions> configurePersonalOptions)
    {
        actionPersonalOptions = configurePersonalOptions;
        return this;
    }

    /// <summary>
    /// Добавляет список <see cref="Claim"/> для формирования токена
    /// </summary>
    /// <remarks>Отменяет действие <see cref="AddPersonal"/></remarks>
    public SecurityTokenBuilder AddClaims(IEnumerable<Claim> claims)
    {
        personalClaim = claims;
        return this;
    }

    /// <summary>
    /// Создаёт <see cref="SecurityTokenBuilder"/> с указанием <see cref="SecurityTokenOptions"/>
    /// </summary>
    public static SecurityTokenBuilder Create(Action<SecurityTokenOptions> configureTokenOptions)
    {
        var builder = new SecurityTokenBuilder();
        builder.SetActionSecurityTokenOptions(configureTokenOptions);
        return builder;
    }

    /// <summary>
    /// Создаёт серилизованный <see cref="SecurityToken"/>
    /// </summary>
    public SecurityTokenBuilderResult Build()
    {
        var configureOptions = new SecurityTokenOptions();
        actionSecurityTokenOptions?.Invoke(configureOptions);
        List<Claim> claims;
        if (!personalClaim.Any())
        {
            var personalOptions = new PersonalOptions();
            actionPersonalOptions?.Invoke(personalOptions);

            claims =
            [
                new (JwtRegisteredClaimNames.Name, personalOptions.Name),
                new (JwtRegisteredClaimNames.Email, personalOptions.Email),
                new (JwtRegisteredClaimNames.GivenName, personalOptions.Login),
                new (JwtRegisteredClaimNames.NameId, personalOptions.Identifier.ToString()),
                new (JwtRegisteredClaimNames.AtHash, personalOptions.SecurityStamp),
            ];

            if (personalOptions.Params?.Any() == true)
            {
                claims.AddRange(personalOptions.Params.Select(x => new Claim(x.Key, x.Value)));
            }
        }
        else
        {
            claims = new List<Claim>(personalClaim);
        }

        var signCred = new SigningCredentials(new SymmetricSecurityKey(configureOptions.SignKey),
            SecurityAlgorithms.HmacSha256);

        var encCred = new EncryptingCredentials(new SymmetricSecurityKey(configureOptions.SecretKey),
            JwtConstants.DirectKeyUseAlg,
            SecurityAlgorithms.Aes192CbcHmacSha384);

        var jwtHandler = new JsonWebTokenHandler();
        var jwtToken = jwtHandler.CreateToken(
            new SecurityTokenDescriptor()
            {
                Issuer = configureOptions.Issuer,
                Audience = configureOptions.Audience,
                NotBefore = configureOptions.NotBefore,
                Expires = configureOptions.Expires,
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = signCred,
                // Пока не можем шифровать токен, т.к. его надо дешифровать на клиенте
                // Надо понять, насколько секурно ключ шифрования хранить на клиенте
                //EncryptingCredentials = encCred,
            });

        return new SecurityTokenBuilderResult
        {
            Token = jwtToken,
            Claims = claims,
        };
    }
}
