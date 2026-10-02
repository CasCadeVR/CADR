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
            AdrLinkTypeApi.DeprecatedBy => "Устерел",
            AdrLinkTypeApi.Supersedes => "Заменяет",
            AdrLinkTypeApi.RelatedTo => "Связан",
            _ => "Устарел",
        };
}
