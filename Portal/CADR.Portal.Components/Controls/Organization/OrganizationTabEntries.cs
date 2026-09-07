using CADR.Portal.Components.Models;
using CADR.Portal.Components.Resources;

namespace CADR.Portal.Components.Controls.Organization;

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
            Icon = Enums.IconTypes.Person,
            EnumValue = OrganizationTableItem.Users,
        },
        new() {
            Text = "Приглашения",
            Url = Routes.Administration.Organization.Invites,
            Icon = Enums.IconTypes.EnvelopeOpened,
            EnumValue = OrganizationTableItem.Invites,
        },
    ];
}
