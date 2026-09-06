using CADR.Administrations.Entities;
using CADR.Administrations.Repositories.Contracts;
using CADR.Administrations.Services.Contracts.Exceptions;
using CADR.Administrations.Services.Contracts.Interfaces;
using CADR.Administrations.Services.Contracts.Models.Token;
using CADR.Common.Core.Contracts;
using CADR.Common.Core.Extensions;

namespace CADR.Administrations.Services.Users;

/// <inheritdoc cref="IRefreshTokenManager"/>
internal sealed class RefreshTokenManager : IRefreshTokenManager, IAdministrationsServiceAnchor
{
    private readonly IRefreshTokenReadRepository refreshTokenReadRepository;
    private readonly IRefreshTokenWriteRepository refreshTokenWriteRepository;
    private readonly IAdministrationUnitOfWork unitOfWork;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IUserReadRepository userReadRepository;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="RefreshTokenManager"/>
    /// </summary>
    public RefreshTokenManager(IAdministrationUnitOfWork administrationUnitOfWork,
        IDateTimeProvider dateTimeProvider,
        IUserReadRepository userReadRepository)
    {
        refreshTokenReadRepository = administrationUnitOfWork.RefreshTokenReadRepository;
        refreshTokenWriteRepository = administrationUnitOfWork.RefreshTokenWriteRepository;
        unitOfWork = administrationUnitOfWork;
        this.dateTimeProvider = dateTimeProvider;
        this.userReadRepository = userReadRepository;
    }

    async Task<Guid> IRefreshTokenManager.CreateRefreshTokenAsync(CreateRefreshTokenModel model, CancellationToken cancellationToken)
    {
        var oldToken = await refreshTokenReadRepository.GetActualByUserIdAsync(model.UserId, cancellationToken);
        if (oldToken != null)
        {
            refreshTokenWriteRepository.Delete(oldToken);
        }

        var moment = dateTimeProvider.UtcNow;
        var token = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = model.UserId,
            AccessPayload = model.AccessPayload,
            CreatedAt = moment,
            SecurityStamp = model.SecurityStamp,
            Expires = moment.AddDays(model.ExpiredDays),
        };
        refreshTokenWriteRepository.Add(token);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return token.Id;
    }

    async Task<UpdateRefreshTokenModel> IRefreshTokenManager.UpdateRefreshTokenAsync(Guid tokenId, CancellationToken cancellationToken)
    {
        var currentToken = await refreshTokenReadRepository.GetActualByIdAsync(tokenId, cancellationToken)
            .OrThrowIfNull(() => new AdministrationEntityNotFoundException<RefreshToken>(tokenId));

        if (currentToken!.Expires <= dateTimeProvider.UtcNow)
        {
            refreshTokenWriteRepository.Delete(currentToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            throw new AdministrationInvalidOperationException("Срок действия токена окончен");
        }

        var user = await userReadRepository.GetByIdAsync(currentToken.UserId, cancellationToken);
        if (user == null ||
            user.SecurityStamp != currentToken.SecurityStamp)
        {
            refreshTokenWriteRepository.Delete(currentToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            throw new AdministrationInvalidOperationException("Невалидный токен. Отметка безопасности пользователя изменена");
        }

        refreshTokenWriteRepository.Delete(currentToken);
        var moment = dateTimeProvider.UtcNow;
        var token = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = currentToken.UserId,
            AccessPayload = currentToken.AccessPayload,
            CreatedAt = moment,
            SecurityStamp = currentToken.SecurityStamp,
            Expires = moment.AddDays((currentToken.Expires - currentToken.CreatedAt).TotalDays),
        };
        refreshTokenWriteRepository.Add(token);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdateRefreshTokenModel
        {
            ClaimPayload = token.AccessPayload,
            TokenId = token.Id,
        };
    }

    async Task IRefreshTokenManager.DeleteRefreshTokenByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var currentToken = await refreshTokenReadRepository.GetActualByUserIdAsync(userId, cancellationToken);
        if (currentToken != null)
        {
            refreshTokenWriteRepository.Delete(currentToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
