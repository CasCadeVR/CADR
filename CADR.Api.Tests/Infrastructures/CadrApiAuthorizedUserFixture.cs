using Ahatornn.TestGenerator;
using CADR.Administrations.Entities;
using CADR.Api.Client;
using CADR.Api.Tests.Client;
using CADR.Common.Mvc.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CADR.Api.Tests.Infrastructures;

/// <summary>
/// Фикстура с апи и авторизованным пользователем
/// </summary>
public class CadrApiAuthorizedUserFixture : CadrApiFixture, IAsyncLifetime
{
    internal PersonalOptions PersonalOptions { get; } = new()
    {
        Identifier = Guid.NewGuid(),
        Email = $"Email{Guid.NewGuid():N}",
        Login = $"Login{Guid.NewGuid():N}",
        Name = $"Name{Guid.NewGuid():N}",
        SecurityStamp = Guid.NewGuid().ToString(),
        Params = new Dictionary<string, string>(),
    };

    /// <summary>
    /// Создает клиента апи для пользователя по указанному <see cref="PersonalOptions"/>
    /// </summary>
    internal ICadrApiClient CreateApiClient(PersonalOptions options)
    {
        var configuration = Factory.Services.GetRequiredService<IConfiguration>();
        var authSetting = configuration.GetSection(JwtSettingsModel.Key).Get<JwtSettingsModel>()!;
        var bearerTokenProvider = new BearerTokenProvider(authSetting, options);
        var client = Factory.CreateClient();
        return new CadrApiTestClient(string.Empty, client, bearerTokenProvider);
    }

    /// <summary>
    ///Создает клиента апи для пользователя по <see cref="PersonalOptions"/>
    ///</summary>
    internal ICadrApiClient CreateApiClient() => CreateApiClient(PersonalOptions);

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        var user = TestEntityProvider.Shared.Create<User>(x =>
        {
            x.Id = PersonalOptions.Identifier;
            x.Email = PersonalOptions.Email;
            x.Name = PersonalOptions.Name;
            x.Login = PersonalOptions.Login;
            x.SecurityStamp = PersonalOptions.SecurityStamp;
            x.EmailLowerCase = x.Email.ToLower();
            x.LoginLowerCase = x.Login.ToLower();
        });

        CadrContext.Add(user);
        await UnitOfWork.SaveChangesAsync();
    }
}
