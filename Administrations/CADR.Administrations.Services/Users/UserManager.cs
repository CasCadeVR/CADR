using AutoMapper;
using CADR.Administrations.Repositories.Contracts;
using CADR.Administrations.Services.Contracts.Exceptions;
using CADR.Administrations.Services.Contracts.Interfaces;
using CADR.Administrations.Services.Contracts.Models.User;
using CADR.Administrations.Services.Helpers;
using CADR.Common.Core.Contracts;
using CADR.Common.Core.Extensions;

namespace CADR.Administrations.Services.Users;

/// <inheritdoc cref="IUserManager"/>
internal sealed class UserManager : IUserManager, IAdministrationsServiceAnchor
{
    private readonly IUserReadRepository userReadRepository;
    private readonly IUserWriteRepository userWriteRepository;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IAdministrationUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserManager"/>
    /// </summary>
    public UserManager(IAdministrationUnitOfWork administrationUnitOfWork,
        IDateTimeProvider dateTimeProvider,
        IMapper mapper)
    {
        userReadRepository = administrationUnitOfWork.UserReadRepository;
        userWriteRepository = administrationUnitOfWork.UserWriteRepository;
        unitOfWork = administrationUnitOfWork;
        this.dateTimeProvider = dateTimeProvider;
        this.mapper = mapper;
    }

    async Task IUserManager.CreateUserAsync(CreateUserModel model, CancellationToken cancellationToken)
    {
        await userReadRepository.IsActiveLoginExistsAsync(model.Login.ToLower(), cancellationToken)
            .AndThrowIfTrue(() => new AdministrationInvalidOperationException($"Пользователь с логином {model.Login} уже существует"));

        var saltValue = SecurityHelper.GenerateSalt32();
        var passwordHash = SecurityHelper.HashPassword32(model.Password, saltValue);
        var user = new Entities.User
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            Email = model.Email.Trim(),
            EmailLowerCase = model.Email.Trim().ToLower(),
            EmailConfirmed = true,
            Login = model.Login.Trim(),
            LoginLowerCase = model.Login.Trim().ToLower(),
            SecurityStamp = Guid.NewGuid().ToString(),
            PasswordHash = passwordHash,
            PasswordSalt = saltValue,
        };
        userWriteRepository.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    async Task<UserLoggedModel> IUserManager.GetActiveByLoginAndPasswordAsync(LoginModel model, CancellationToken cancellationToken)
    {
        var message = "Пользователь с указанным логином и паролем не найден";
        var user = await userReadRepository.GetActiveByLoginAsync(model.Login.ToLower(), cancellationToken)
            .OrThrowIfNull(() => new AdministrationNotFoundException(message));

        if (user!.Blocked && (!user.BlockedAt.HasValue || user.BlockedAt > dateTimeProvider.UtcNow))
        {
            throw new AdministrationNotFoundException(message);
        }

        var passwordHash = SecurityHelper.HashPassword32(model.Password, user.PasswordSalt);
        if (passwordHash != user.PasswordHash)
        {
            throw new AdministrationNotFoundException(message);
        }

        if (user.Blocked)
        {
            user.Blocked = false;
            user.BlockedAt = null;
            userWriteRepository.Update(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return mapper.Map<UserLoggedModel>(user);
    }

    async Task IUserManager.DeleteUserAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await userReadRepository.GetByIdAsync(id, cancellationToken);
        if (user != null)
        {
            userWriteRepository.Delete(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    async Task<UserLoggedModel> IUserManager.ModifyUserAsync(UserModifyModel model, CancellationToken cancellationToken)
    {
        var user = await userReadRepository.GetActiveByIdAsync(model.Id, cancellationToken)
            .OrThrowIfNull(() => new AdministrationNotFoundException("Пользователь не найден"));

        user!.Name = model.Name.Trim();
        user.Email = model.Email.Trim();
        user.SecurityStamp = Guid.NewGuid().ToString("N");
        userWriteRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<UserLoggedModel>(user);
    }
}
