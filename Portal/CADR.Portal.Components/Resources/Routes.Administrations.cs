namespace CADR.Portal.Components.Resources;

public static partial class Routes
{
    /// <summary>
    /// Константы роутов администрирования
    /// </summary>
    public static class Administration
    {
        /// <summary>
        /// Роуты для логина, регистрации и т.п.
        /// </summary>
        public static class Account
        {
            private const string Prefix = "account";

            /// <summary>
            /// Авторизация пользователя
            /// </summary>
            public const string Login = $"{Prefix}/login";

            /// <summary>
            /// Регистрация пользователя
            /// </summary>
            public const string Registration = $"{Prefix}/register";

            /// <summary>
            /// Профиль пользователя
            /// </summary>
            public const string Profile = $"{Prefix}/profile";

            /// <summary>
            /// Профиль пользователя организации
            /// </summary>
            public const string ProfileWithOrganization = $"{Prefix}/profile/{{organizationId:guid}}";

            /// <summary>
            /// Профиль пользователя организации. Константа для форматирования
            /// </summary>
            public const string ProfileWithOrganizationFormat = $"{Prefix}/profile/{{0}}";
        }

        /// <summary>
        /// Роуты для управления организациями
        /// </summary>
        public static class Organization
        {
            private const string Prefix = "organization";

            /// <summary>
            /// Список организаций
            /// </summary>
            public const string List = $"{Prefix}/list";

            /// <summary>
            /// Создание и редактирование организации
            /// </summary>
            public const string Form = $"{Prefix}/form";

            /// <summary>
            /// Список пользователей организации
            /// </summary>
            public const string Users = $"{Prefix}/{{0}}/users";

            /// <summary>
            /// Профиль пользователя организации
            /// </summary>
            public const string UserProfile = $"{Prefix}/{{organizationId:guid}}/users/{{userId:guid}}/profile";

            /// <summary>
            /// Профиль пользователя организации. Константа для форматирования
            /// </summary>
            public const string UserProfileFormat = $"{Prefix}/{{0}}/users/{{1}}/profile";

            /// <summary>
            /// Список приглашений организации
            /// </summary>
            public const string Invites = $"{Prefix}/{{0}}/invites";

            /// <summary>
            /// Главная страница организации
            /// </summary>
            public const string Dashboard = $"{Prefix}/{{organizationId:guid}}/dashboard";

            /// <summary>
            /// Главная страница организации. Константа для форматирования
            /// </summary>
            public const string DashboardFormat = $"{Prefix}/{{0}}/dashboard";
        }
    }
}
