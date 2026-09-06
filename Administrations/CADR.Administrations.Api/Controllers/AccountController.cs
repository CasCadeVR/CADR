using System.Security.Claims;
using AutoMapper;
using CADR.Administrations.Api.Infrastructures;
using CADR.Administrations.Api.Models.Users;
using CADR.Administrations.Api.Resources;
using CADR.Administrations.Services.Contracts.Interfaces;
using CADR.Administrations.Services.Contracts.Models.Token;
using CADR.Administrations.Services.Contracts.Models.User;
using CADR.Common.Core.Contracts;
using CADR.Common.Core.Extensions;
using CADR.Common.Mvc.Attributes;
using CADR.Common.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace CADR.Administrations.Api.Controllers;

/// <summary>
/// Работа с учётными записями
/// </summary>
[ApiController]
[Authorize]
[ApiExplorerSettings(GroupName = $"{AdministrationConstants.DocPrefix}v1")]
[Route(AdministrationConstants.DefaultControllerRoute)]
public class AccountController : ControllerBase
{
    private readonly IUserManager userManager;
    private readonly IAdministrationValidateService administrationValidateService;
    private readonly IRefreshTokenManager refreshTokenManager;
    private readonly IIdentityProvider identityProvider;
    private readonly IMapper mapper;
    private readonly IDataProtectionProvider protectionProvider;

    private JwtSettingsModel authSetting;

    private readonly static Dictionary<string, string> claimNameMap = new()
    {
        { ClaimTypes.Name, JwtRegisteredClaimNames.Name },
        { ClaimTypes.Email, JwtRegisteredClaimNames.Email },
        { ClaimTypes.GivenName, JwtRegisteredClaimNames.GivenName },
        { ClaimTypes.NameIdentifier, JwtRegisteredClaimNames.NameId }
    };

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AccountController"/>
    /// </summary>
    public AccountController(IUserManager userManager,
        IAdministrationValidateService administrationValidateService,
        IRefreshTokenManager refreshTokenManager,
        IIdentityProvider identityProvider,
        IConfiguration configuration,
        IMapper mapper,
        IDataProtectionProvider protectionProvider)
    {
        this.userManager = userManager;
        this.administrationValidateService = administrationValidateService;
        this.refreshTokenManager = refreshTokenManager;
        this.identityProvider = identityProvider;
        this.mapper = mapper;
        this.protectionProvider = protectionProvider;
        authSetting = configuration.GetJwtSettingsConfiguration();
    }

    /// <summary>
    /// Создаёт нового пользователя
    /// </summary>
    [AllowAnonymous]
    [HttpPost("register")]
    [ApiNoContent]
    [ApiNotAcceptable]
    [ApiConflict]
    [ApiValidation]
    [SwaggerOperation(OperationId = "AccountCreateUser")]
    public async Task<IActionResult> CreateUser(CreateUserApiRequest modelRequest, CancellationToken cancellationToken)
    {
        var model = mapper.Map<CreateUserModel>(modelRequest);
        await administrationValidateService.ValidateAsync(model, cancellationToken);
        await userManager.CreateUserAsync(model, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login")]
    [ApiOk(typeof(LoginApiResponse))]
    [ApiNotFound]
    [ApiConflict]
    [SwaggerOperation(OperationId = "AccountLogin")]
    public async Task<IActionResult> Login(LoginApiRequest request, CancellationToken cancellationToken)
    {
        var model = new LoginModel { Login = request.Login, Password = request.Password, };
        await administrationValidateService.ValidateAsync(model, cancellationToken);
        var user = await userManager.GetActiveByLoginAndPasswordAsync(model, cancellationToken);
        var result = await CreateLoginApiResponse(request.IsRemember, user, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Обновить токен доступа для авторизации по cookie
    /// </summary>
    [AllowAnonymous]
    [HttpPost("refresh-cookie")]
    [ApiOk]
    [ApiNotFound]
    [ApiUnprocessableEntity]
    [SwaggerOperation(OperationId = "AccountRefreshCookie")]
    public async Task<IActionResult> RefreshCookie(CancellationToken cancellationToken)
    {
        var refreshTokenCookie = GetRefreshTokenCookie();

        var response = await UpdateRefreshToken(refreshTokenCookie ?? string.Empty, cancellationToken);
        SetAccessTokenCookie(response.Token, withRefreshToken: true);
        SetRefreshTokenCookie(response.RefreshToken, authSetting.RefreshLifeTimeDays);

        return Ok();
    }

    /// <summary>
    /// Обновить токен доступа для авторизации по json токену
    /// </summary>
    [AllowAnonymous]
    [HttpPost("refresh-token")]
    [ApiOk(typeof(LoginApiResponse))]
    [ApiNotFound]
    [ApiUnprocessableEntity]
    [SwaggerOperation(OperationId = "AccountRefreshToken")]
    public async Task<IActionResult> RefreshToken(RefreshTokenApiRequest request, CancellationToken cancellationToken)
    {
        var response = await UpdateRefreshToken(request.RefreshToken, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Возвращает информацию о текущем авторизованном пользователе
    /// </summary>
    [HttpGet("me")]
    [ApiOk(typeof(UserProfileApiResponse))]
    [ApiUnauthorized]
    [SwaggerOperation(OperationId = "AccountGetCurrentUser")]
    public IActionResult GetCurrentUserInfo()
    {
        var response = new UserProfileApiResponse
        {
            Claims = identityProvider.Claims
                .Select(c => new KeyValuePair<string, string>(
                    claimNameMap.GetValueOrDefault(c.Key, c.Key),
                    c.Value))
                .ToDictionary(c => c.Key, c => c.Value)
        };

        return Ok(response);
    }

    /// <summary>
    /// Выход из системы
    /// </summary>
    [HttpPut("logoff")]
    [ApiNoContent]
    [ApiUnauthorized]
    [SwaggerOperation(OperationId = "AccountLogoff")]
    public async Task<IActionResult> Logoff(CancellationToken cancellationToken)
    {
        var userId = identityProvider.Id;
        await refreshTokenManager.DeleteRefreshTokenByUserIdAsync(userId, cancellationToken);
        DeleteCookies();
        return NoContent();
    }

    private async Task<LoginApiResponse> CreateLoginApiResponse(bool isRemember, UserLoggedModel user, CancellationToken cancellationToken)
    {
        var securityTokenBuilder = SecurityTokenManager.CreateToken(authSetting, user, isRemember);
        var result = new LoginApiResponse { Token = securityTokenBuilder.Token, RefreshToken = string.Empty, };
        SetAccessTokenCookie(securityTokenBuilder.Token, isRemember);

        if (isRemember)
        {
            var refreshTokenId = await refreshTokenManager.CreateRefreshTokenAsync(new CreateRefreshTokenModel
            {
                UserId = user.Id,
                SecurityStamp = user.SecurityStamp,
                ExpiredDays = authSetting.RefreshLifeTimeDays,
                AccessPayload = JsonConvert.SerializeObject(securityTokenBuilder.Claims
                .Select(x => new KeyValuePair<string, string>(x.Type, x.Value))),
            }, cancellationToken);

            var protector = protectionProvider.CreateProtector(typeof(AccountController).ToString());
            result.RefreshToken = Base64UrlEncoder.Encode(protector.Protect(refreshTokenId.ToByteArray()));
            SetRefreshTokenCookie(result.RefreshToken, authSetting.RefreshLifeTimeDays);
        }

        return result;
    }

    private async Task<LoginApiResponse> UpdateRefreshToken(string encryptedRefreshToken, CancellationToken cancellationToken)
    {
        var protector = protectionProvider.CreateProtector(typeof(AccountController).ToString());
        var value = protector.Unprotect(Base64UrlEncoder.DecodeBytes(encryptedRefreshToken));
        var model = await refreshTokenManager.UpdateRefreshTokenAsync(new Guid(value), cancellationToken);

        var securityTokenBuilder = SecurityTokenManager.CreateTokenByClaims(authSetting, model.ClaimPayload);
        var newRefreshToken = Base64UrlEncoder.Encode(protector.Protect(model.TokenId.ToByteArray()));

        return new LoginApiResponse
        {
            Token = securityTokenBuilder.Token,
            RefreshToken = newRefreshToken
        };
    }

    private void SetAccessTokenCookie(string token, bool withRefreshToken)
    {
        var options = ConfigureCookieOptions(authSetting, DateTimeOffset.UtcNow.AddSeconds(withRefreshToken
                ? authSetting.LifeTimeSec
                : authSetting.NotPersistentSecondValue));

        HttpContext.Response.Cookies.Append(authSetting.Cookie.AccessTokenName!, token, options);
    }

    private void SetRefreshTokenCookie(string refreshToken, int? lifeTimeDays)
    {
        var options = ConfigureCookieOptions(authSetting, lifeTimeDays.HasValue
            ? DateTimeOffset.UtcNow.AddDays(lifeTimeDays.Value)
            : null);

        HttpContext.Response.Cookies.Append(authSetting.Cookie.RefreshTokenName!, refreshToken, options);
    }

    private void DeleteCookies()
    {
        HttpContext.Response.Cookies.Delete(authSetting.Cookie.AccessTokenName!);
        HttpContext.Response.Cookies.Delete(authSetting.Cookie.RefreshTokenName!);
    }

    private string? GetRefreshTokenCookie()
        => HttpContext.Request.Cookies[authSetting.Cookie.RefreshTokenName!];

    private static CookieOptions ConfigureCookieOptions(JwtSettingsModel authSetting, DateTimeOffset? expires)
        => new()
        {
            HttpOnly = authSetting.Cookie.HttpOnly,
            Secure = authSetting.Cookie.Secure,
            SameSite = Enum.Parse<SameSiteMode>(authSetting.Cookie.SameSite!, true),
            Path = authSetting.Cookie.Path,
            Expires = expires
        };
}
