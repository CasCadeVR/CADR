using System.ComponentModel.DataAnnotations;
using CADR.Api.Client;

namespace CADR.Administrations.Pages.Models.Organization;

/// <summary>
/// Модель приглашения к организации
/// </summary>
public class OrganizationInviteModel
{
    /// <summary>
    /// Почтовый адрес пользователя
    /// </summary>
    [Required(ErrorMessage = "Укажите почтовый адрес пользователя")]
    [EmailAddress(ErrorMessage = "Укажите валидный адрес")]
    public string UserMail { get; set; } = string.Empty;

    /// <inheritdoc cref="UserRoleApi"/>
    public UserRoleApi UserRole { get; set; }
}
