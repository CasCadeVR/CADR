using CADR.Administrations.Entities.Enums;

namespace CADR.Administrations.Services.Helpers;

static internal class RoleHelper
{
    /// <summary>
    /// Выбирает роль с минимальными правами из двух
    /// </summary>
    public static Role Min(Role role1, Role role2)
    {
        var roleValue1 = (int)role1;
        var roleValue2 = (int)role2;
        return (Role)Math.Min(roleValue1, roleValue2);
    }
}
