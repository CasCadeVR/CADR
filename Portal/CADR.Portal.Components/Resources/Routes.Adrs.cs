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
            private const string Prefix = "organization";

            /// <summary>
            /// Корневая папка ADR
            /// </summary>
            public const string Root = $"{Prefix}";

            /// <summary>
            /// Создание и редактирование ADR
            /// </summary>
            public const string AdrForm = $"{Prefix}/{{0}}/form";

            /// <summary>
            /// Просмотр ADR
            /// </summary>
            public const string AdrDetails = $"{Prefix}/{{0}}";
        }
    }
}
