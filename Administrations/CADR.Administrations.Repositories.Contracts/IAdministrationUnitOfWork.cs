using CADR.Context.Contracts;

namespace CADR.Administrations.Repositories.Contracts;

/// <summary>
/// <see cref="IUnitOfWork"/> для раздела администрирования
/// </summary>
public interface IAdministrationUnitOfWork : IUnitOfWork
{
    /// <inheritdoc cref="IOrganizationReadRepository"/>
    IOrganizationReadRepository OrganizationReadRepository { get; }

    /// <inheritdoc cref="IOrganizationWriteRepository"/>
    IOrganizationWriteRepository OrganizationWriteRepository { get; }

    /// <inheritdoc cref="IRefreshTokenReadRepository"/>
    IRefreshTokenReadRepository RefreshTokenReadRepository { get; }

    /// <inheritdoc cref="IRefreshTokenWriteRepository"/>
    IRefreshTokenWriteRepository RefreshTokenWriteRepository { get; }

    /// <inheritdoc cref="IUserOrganizationReadRepository"/>
    IUserOrganizationReadRepository UserOrganizationReadRepository { get; }

    /// <inheritdoc cref="IUserOrganizationWriteRepository"/>
    IUserOrganizationWriteRepository UserOrganizationWriteRepository { get; }

    /// <inheritdoc cref="IUserReadRepository"/>
    IUserReadRepository UserReadRepository { get; }

    /// <inheritdoc cref="IUserWriteRepository"/>
    IUserWriteRepository UserWriteRepository { get; }

    /// <inheritdoc cref="IUserInviteReadRepository"/>
    IUserInviteReadRepository UserInviteReadRepository { get; }

    /// <inheritdoc cref="IUserInviteWriteRepository"/>
    IUserInviteWriteRepository UserInviteWriteRepository { get; }
}
