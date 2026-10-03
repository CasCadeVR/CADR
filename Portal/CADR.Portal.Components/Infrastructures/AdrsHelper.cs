using CADR.Api.Client;
using CADR.Portal.Components.Enums;

namespace CADR.Portal.Components.Infrastructures;

/// <summary>
/// Помошник в работе с ADR
/// </summary>
public static class AdrsHelper
{
    /// <summary>
    /// Получить цвет для репутации
    /// </summary>
    public static ThemeColors GetScoreColor(int score)
        => score > 0 ? ThemeColors.Success : score < 0 ? ThemeColors.Danger : ThemeColors.Black;

    /// <summary>
    /// Получить описание статуса
    /// </summary>
    public static string GetStatusCaption(AdrStatusApi status)
        => status switch
        {
            AdrStatusApi.Draft => "Черновик",
            AdrStatusApi.Proposed => "Предложен",
            AdrStatusApi.Approved => "Одобрен",
            AdrStatusApi.Rejected => "Отклонён",
            AdrStatusApi.NeedsRevision => "Требует правок",
            _ => "Устарел",
        };

    /// <summary>
    /// Получить ассоциативный со статусом цвет
    /// </summary>
    public static ThemeColors GetStatusColor(AdrStatusApi status)
        => status switch
        {
            AdrStatusApi.Approved => ThemeColors.Success,
            AdrStatusApi.Rejected => ThemeColors.Danger,
            AdrStatusApi.NeedsRevision => ThemeColors.Warning,
            AdrStatusApi.Proposed => ThemeColors.Info,
            AdrStatusApi.Draft => ThemeColors.Secondary,
            _ => ThemeColors.Dark,
        };

    /// <summary>
    /// Получить описание связи
    /// </summary>
    public static string GetLinkTypeCaption(AdrLinkTypeApi linkType)
        => linkType switch
        {
            AdrLinkTypeApi.DeprecatedBy => "Устарел в пользу",
            AdrLinkTypeApi.Supersedes => "Заменяет",
            AdrLinkTypeApi.RelatedTo => "Связан с",
            _ => "Устарел",
        };

    /// <summary>
    /// Возвращает допустимые целевые статусы.
    /// Зеркало AdrManager.IsTransitionAllowed — при изменении правил API обновить здесь!
    /// </summary>
    public static IReadOnlyCollection<AdrStatusApi> GetAllowedTargetStatuses(AdrStatusApi current, UserRoleApi role, bool isAuthor)
    {
        var isAdmin = role == UserRoleApi.Admin;
        var isArchitect = role == UserRoleApi.Architect;
        return (current, isAuthor) switch
        {
            (AdrStatusApi.Draft, _) when isAdmin || isArchitect => [AdrStatusApi.Proposed],
            (AdrStatusApi.Proposed, _) when isAdmin => [AdrStatusApi.Approved, AdrStatusApi.Rejected],
            (AdrStatusApi.Approved, _) when isAdmin => [AdrStatusApi.NeedsRevision, AdrStatusApi.Deprecated],
            (AdrStatusApi.Approved, true) => [AdrStatusApi.Deprecated],
            (AdrStatusApi.NeedsRevision, _) when isAdmin => [AdrStatusApi.Approved, AdrStatusApi.Proposed, AdrStatusApi.Deprecated],
            (AdrStatusApi.NeedsRevision, _) when isArchitect => [AdrStatusApi.Proposed],
            (AdrStatusApi.NeedsRevision, true) => [AdrStatusApi.Deprecated],
            (AdrStatusApi.Rejected, _) when isAdmin => [AdrStatusApi.Proposed],
            _ => [],
        };
    }
}
