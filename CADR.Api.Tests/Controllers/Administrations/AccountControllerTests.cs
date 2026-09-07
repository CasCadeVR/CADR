using Ahatornn.TestGenerator;
using CADR.Administrations.Entities;
using CADR.Api.Client;
using CADR.Api.Tests.Client;
using CADR.Api.Tests.Infrastructures;
using CADR.Context;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Xunit;

namespace CADR.Api.Tests.Controllers.Administrations;

/// <summary>
/// Тесты сценариев работы с пользователем
/// </summary>
[Collection(nameof(CadrApiTestCollection))]
public class AccountControllerTests
{
    private readonly ICadrApiTestClient apiClient;
    private readonly CadrContext context;
    private readonly TestEntityProvider entityProvider;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AccountControllerTests"/>
    /// </summary>
    public AccountControllerTests(CadrApiFixture fixture)
    {
        entityProvider = new TestEntityProviderBuilder()
            .AddPreset<CreateUserApiRequest>(x =>
            {
                x.Password = x.PasswordAgain = Guid.NewGuid().ToString();
                x.Email = $"{Guid.NewGuid()}@example.com";
            })
            .Build();
        context = fixture.CadrContext;
        apiClient = fixture.CreateAnonymousApiClient();
    }

    /// <summary>
    /// Создаёт нового пользователя
    /// </summary>
    [Fact]
    public async Task CreateUserShouldWork()
    {
        // Arrange
        var registerModel = entityProvider.Create<CreateUserApiRequest>();

        // Act
        await apiClient.AccountCreateUserAsync(registerModel);

        // Assert
        var user = await context.Set<User>().FirstOrDefaultAsync(x => x.Login == registerModel.Login);
        user.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new
            {
                registerModel.Name,
                registerModel.Email,
                EmailConfirmed = true,
                Blocked = false,
                LoginLowerCase = registerModel.Login.ToLower(),
                EmailLowerCase = registerModel.Email.ToLower(),
                PasswordIsTemporary = false,
            });
    }

    /// <summary>
    /// Создаёт нового пользователя и авторизуется под ним
    /// </summary>
    [Fact]
    public async Task CreateUserAndLoginShouldWork()
    {
        // Arrange
        var password = Guid.NewGuid().ToString();
        var registerModel = entityProvider.Create<CreateUserApiRequest>(x => x.Password = x.PasswordAgain = password);
        var loginModel = new LoginApiRequest { Login = registerModel.Login, Password = password, IsRemember = false, };

        // Act
        await apiClient.AccountCreateUserAsync(registerModel);
        var tokenResult = await apiClient.AccountLoginAsync(loginModel);

        // Assert
        tokenResult.Should().NotBeNull();
        tokenResult.Token.Should().NotBeEmpty();
        tokenResult.RefreshToken.Should().BeEmpty();
    }

    /// <summary>
    /// Создаёт нового пользователя и авторизуется под ним с перевыпуском рефреш-токена
    /// </summary>
    [Fact]
    public async Task CreateUserAndRefreshShouldWork()
    {
        // Arrange
        var password = Guid.NewGuid().ToString();
        var registerModel = entityProvider.Create<CreateUserApiRequest>(x => x.Password = x.PasswordAgain = password);
        var loginModel = new LoginApiRequest { Login = registerModel.Login, Password = password, IsRemember = true, };

        // Act
        await apiClient.AccountCreateUserAsync(registerModel);
        var tokenResult = await apiClient.AccountLoginAsync(loginModel);
        await Task.Delay(1000);
        var refreshModel = new RefreshTokenApiRequest { RefreshToken = tokenResult.RefreshToken, };
        var newToken = await apiClient.AccountRefreshTokenAsync(refreshModel);

        // Assert
        tokenResult.Should().NotBeNull();
        tokenResult.Token.Should().NotBeEmpty();
        tokenResult.RefreshToken.Should().NotBeEmpty();

        newToken.Should().NotBeNull();
        newToken.Token.Should().NotBeEmpty();
        newToken.RefreshToken.Should().NotBeEmpty();

        newToken.Token.Should().NotBe(tokenResult.Token);
        newToken.RefreshToken.Should().NotBe(tokenResult.RefreshToken);
    }

    /// <summary>
    /// Создаёт нового пользователя и авторизуется под ним с созданием кук
    /// </summary>
    [Fact]
    public async Task LoginShouldSetCookies()
    {
        // Arrange
        var password = Guid.NewGuid().ToString();
        var registerModel = entityProvider.Create<CreateUserApiRequest>(x => x.Password = x.PasswordAgain = password);
        var loginModel = new LoginApiRequest { Login = registerModel.Login, Password = password, IsRemember = true, };

        // Act
        await apiClient.AccountCreateUserAsync(registerModel);
        await apiClient.AccountLoginAsync(loginModel);

        // Assert
        var setCookieHeaders = apiClient.GetCookieHeadersFromLastResponse();
        setCookieHeaders.Should().Contain(cookie => cookie.StartsWith("cadr_auth="));
        setCookieHeaders.Should().Contain(cookie => cookie.StartsWith("cadr_refresh="));
    }

    /// <summary>
    /// Создаёт нового пользователя и получает информацию о его клеймов
    /// </summary>
    [Fact]
    public async Task GetCurrentUserInfoShouldWorkWithCookies()
    {
        // Arrange
        var password = Guid.NewGuid().ToString();
        var registerModel = entityProvider.Create<CreateUserApiRequest>(x => x.Password = x.PasswordAgain = password);
        var loginModel = new LoginApiRequest { Login = registerModel.Login, Password = password, IsRemember = true, };

        // Act
        await apiClient.AccountCreateUserAsync(registerModel);
        await apiClient.AccountLoginAsync(loginModel);
        await Task.Delay(1000);

        var cookies = ExtractCookies(apiClient.GetCookieHeadersFromLastResponse());
        apiClient.SetCookies(cookies);
        var meResponse = await apiClient.AccountGetCurrentUserAsync();

        // Assert
        var claims = meResponse.Claims;
        claims[JwtRegisteredClaimNames.Name].Should().Be(registerModel.Name);
        claims[JwtRegisteredClaimNames.Email].Should().Be(registerModel.Email);
        claims[JwtRegisteredClaimNames.GivenName].Should().Be(registerModel.Login);
        claims[JwtRegisteredClaimNames.NameId].Should().NotBeNullOrWhiteSpace();
    }

    private static string ExtractCookies(IEnumerable<string> response)
    {
        var cookies = response
            .Select(cookie => cookie.Split(';')[0])
            .ToList();

        return string.Join("; ", cookies);
    }
}
