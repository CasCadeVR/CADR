using CADR.Common.Repositories.Contracts;
using CADR.Administrations.Entities;

namespace CADR.Administrations.Repositories.Contracts;

/// <summary>
/// Репозиторий записи <see cref="UserInvite"/>
/// </summary>
public interface IUserInviteWriteRepository : IDbWriter<UserInvite> { }
