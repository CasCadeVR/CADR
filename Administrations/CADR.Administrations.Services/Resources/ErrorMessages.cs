namespace CADR.Administrations.Services.Resources;

/// <summary>
/// Константы сообщений об ошибках
/// </summary>
public class ErrorMessages
{
    /// <summary>
    /// Указанный логин уже существует
    /// </summary>
    public const string LoginAlreadyExist = "Указанный логин уже существует";

    /// <summary>
    /// Указанный почтовый адрес уже существует
    /// </summary>
    public const string EmailAlreadyExist = "Указанный почтовый адрес уже существует";

    /// <summary>
    /// Токен обновления не валиден
    /// </summary>
    public const string RefreshTokenIsInvalid = "Токен обновления не валиден";

    /// <summary>
    /// Указанное имя уже используется
    /// </summary>
    public const string OrganizationAlreadyExist = "Указанное имя уже используется";

    /// <summary>
    /// Указанная компетенция уже существует в организации
    /// </summary>
    public const string CompetenceAlreadyExist = "Указанная компетенция уже существует в организации";
}
