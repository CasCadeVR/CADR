using CADR.Portal.Components.Enums;
using CADR.Portal.Components.Models;
using CADR.Portal.Components.Resources;

namespace CADR.Administrations.Pages.Organization.Components;

/// <summary>
/// Метаданные таблицы организаций
/// </summary>
public static class OrganizationTabEntries
{
    /// <summary>
    /// Коллекция метаданных
    /// </summary>
    public static IReadOnlyCollection<TabEntry<OrganizationTableItem>> Entries = [
        new() {
            Text = "Пользователи",
            Url = Routes.Administration.Organization.Users,
            Icon = IconTypes.Person,
            EnumValue = OrganizationTableItem.Users,
        },
        new() {
            Text = "Приглашения",
            Url = Routes.Administration.Organization.Invites,
            Icon = IconTypes.EnvelopeOpened,
            EnumValue = OrganizationTableItem.Invites,
        },
    ];
}
