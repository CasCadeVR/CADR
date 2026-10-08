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

            /// <summary>
            /// Настройки организации
            /// </summary>
            public const string Settings = $"{Prefix}/{{organizationId:guid}}/adr-settings";

            /// <summary>
            /// Настройки организации. Константа для форматирования
            /// </summary>
            public const string SettingsFormat = $"{Prefix}/{{0}}/adr-settings";

            /// <summary>
            /// Создание шаблона ADR
            /// </summary>
            public const string TemplateForm = $"{Prefix}/{{organizationId:guid}}/adr-settings/templates/form";

            /// <summary>
            /// Создание шаблона ADR. Константа для форматирования
            /// </summary>
            public const string TemplateFormFormat = $"{Prefix}/{{0}}/adr-settings/templates/form";

            /// <summary>
            /// Редактирование шаблона ADR
            /// </summary>
            public const string TemplateFormUpdate = $"{Prefix}/{{organizationId:guid}}/adr-settings/templates/form/{{templateId:guid}}";

            /// <summary>
            /// Редактирование шаблона ADR. Константа для форматирования
            /// </summary>
            public const string TemplateFormUpdateFormat = $"{Prefix}/{{0}}/adr-settings/templates/form/{{1}}";
        }
    }
}
