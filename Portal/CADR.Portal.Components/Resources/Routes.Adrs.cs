namespace CADR.Portal.Components.Resources;

public static partial class Routes
{
    /// <summary>
    /// Константы роутов ADR
    /// </summary>
    public static class Adrs
    {
        /// <summary>
        /// Роуты для управления ADR организации
        /// </summary>
        public static class Organization
        {
            private const string Prefix = $"organization/{{organizationId:guid}}/adrs";
            private const string PrefixFormat = $"organization/{{0}}/adrs";

            /// <summary>
            /// Корневая папка ADR
            /// </summary>
            public const string List = $"{Prefix}";

            /// <summary>
            /// Корневая папка ADR. Константа для форматирования
            /// </summary>
            public const string ListFormat = $"{PrefixFormat}";

            /// <summary>
            /// Вложенная папка ADR
            /// </summary>
            public const string ListWithFolder = $"{Prefix}/folder/{{folderId:guid}}";

            /// <summary>
            /// Вложенная папка ADR. Константа для форматирования
            /// </summary>
            public const string ListWithFolderFormat = $"{PrefixFormat}/folder/{{1}}";

            /// <summary>
            /// Создание ADR
            /// </summary>
            public const string AdrForm = $"{Prefix}/form";

            /// <summary>
            /// Создание ADR. Константа для форматирования
            /// </summary>
            public const string AdrFormFormat = $"{PrefixFormat}/form";

            /// <summary>
            /// Редактирование ADR
            /// </summary>
            public const string AdrFormUpdate = $"{Prefix}/form/{{id:guid}}";

            /// <summary>
            /// Редактирование ADR. Константа для форматирования
            /// </summary>
            public const string AdrFormUpdateFormat = $"{PrefixFormat}/form/{{1}}";

            /// <summary>
            /// Просмотр ADR
            /// </summary>
            public const string AdrDetails = $"{Prefix}/{{adrId:guid}}";

            /// <summary>
            /// Просмотр ADR. Константа для форматирования
            /// </summary>
            public const string AdrDetailsFormat = $"{PrefixFormat}/{{1}}";
        }
    }
}
