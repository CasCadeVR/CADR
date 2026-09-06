using CADR.Administrations.Services.Contracts.Models.User;

namespace CADR.Administrations.Services.Contracts.Interfaces;

/// <summary>
/// Управление пользователями
/// </summary>
public interface IUserManager
{
    /// <summary>
    /// Создаёт нового пользователя
    /// </summary>
    Task CreateUserAsync(CreateUserModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Получает активного пользователя по паре логин - пароль
    /// </summary>
    Task<UserLoggedModel> GetActiveByLoginAndPasswordAsync(LoginModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет пользователя по идентификатору
    /// </summary>
    Task DeleteUserAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Изменяет параметры пользователя
    /// </summary>
    Task<UserLoggedModel> ModifyUserAsync(UserModifyModel model, CancellationToken cancellationToken);
}
