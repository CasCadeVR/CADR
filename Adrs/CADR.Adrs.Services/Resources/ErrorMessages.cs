namespace CADR.Adrs.Services.Resources;

/// <summary>
/// Константы сообщений об ошибках
/// </summary>
public class ErrorMessages
{
    /// <summary>
    /// Папка с указанным именем уже существует
    /// </summary>
    public const string FolderAlreadyExist = "Папка с указанным именем уже существует";

    /// <summary>
    /// Шаблон с указанным именем уже существует
    /// </summary>
    public const string TemplateAlreadyExist = "Шаблон с указанным именем уже существует";

    /// <summary>
    /// ADR не может ссылаться сама на себя
    /// </summary>
    public const string SelfLinkForbidden = "ADR не может ссылаться сама на себя";

    /// <summary>
    /// Связь может быть установлена только между ADR одной организации
    /// </summary>
    public const string CrossOrganizationLinkForbidden = "Связь может быть установлена только между ADR одной организации";

    /// <summary>
    /// Указанная связь между ADR уже существует
    /// </summary>
    public const string LinkAlreadyExist = "Указанная связь между ADR уже существует";

    /// <summary>
    /// Родительская папка принадлежит другой организации
    /// </summary>
    public const string ParentFolderFromAnotherOrganization = "Родительская папка принадлежит другой организации";

    /// <summary>
    /// Нельзя переместить папку в саму себя
    /// </summary>
    public const string FolderMoveToItself = "Нельзя переместить папку в саму себя";

    /// <summary>
    /// Нельзя переместить папку в её вложенную папку
    /// </summary>
    public const string FolderMoveToDescendant = "Нельзя переместить папку в её вложенную папку";

    /// <summary>
    /// Нельзя голосовать за собственный ADR
    /// </summary>
    public const string CannotVoteOwnAdr = "Нельзя голосовать за собственный ADR";

    /// <summary>
    /// Встроенный шаблон нельзя изменить или удалить
    /// </summary>
    public const string TemplateIsBuiltIn = "Встроенный шаблон нельзя изменить или удалить";

    /// <summary>
    /// Шаблон недоступен для данной организации
    /// </summary>
    public const string TemplateNotAvailable = "Шаблон недоступен для данной организации";

    /// <summary>
    /// Недопустимый переход статуса ADR с {0} на {1}
    /// </summary>
    public const string InvalidStatusTransition = "Недопустимый переход статуса ADR с {0} на {1}";
}
