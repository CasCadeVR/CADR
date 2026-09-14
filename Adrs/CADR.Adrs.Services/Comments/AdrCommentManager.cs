using AutoMapper;
using CADR.Administrations.Repositories.Contracts;
using CADR.Administrations.Repositories.Contracts.Extensions;
using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Adrs.Services.Contracts.Exceptions;
using CADR.Adrs.Services.Contracts.Interfaces;
using CADR.Adrs.Services.Contracts.Models.Comments;
using CADR.Common.Core.Extensions;

namespace CADR.Adrs.Services.Comments;

/// <inheritdoc cref="IAdrCommentManager"/>
internal sealed class AdrCommentManager : IAdrCommentManager, IAdrsServiceAnchor
{
    private readonly IAdrUnitOfWork unitOfWork;
    private readonly IAdrReadRepository adrReadRepository;
    private readonly IAdrCommentReadRepository adrCommentReadRepository;
    private readonly IAdrCommentWriteRepository adrCommentWriteRepository;
    private readonly IUserOrganizationReadRepository userOrganizationReadRepository;
    private readonly IUserReadRepository userReadRepository;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrCommentManager"/>
    /// </summary>
    public AdrCommentManager(IAdrUnitOfWork adrUnitOfWork, IMapper mapper, IUserOrganizationReadRepository userOrganizationReadRepository, IUserReadRepository userReadRepository)
    {
        unitOfWork = adrUnitOfWork;
        adrReadRepository = adrUnitOfWork.AdrReadRepository;
        adrCommentReadRepository = adrUnitOfWork.AdrCommentReadRepository;
        adrCommentWriteRepository = adrUnitOfWork.AdrCommentWriteRepository;
        this.userOrganizationReadRepository = userOrganizationReadRepository;
        this.userReadRepository = userReadRepository;
        this.mapper = mapper;
    }

    async Task<AdrCommentModel> IAdrCommentManager.CreateAsync(CreateAdrCommentModel model, CancellationToken cancellationToken)
    {
        var adr = await GetAdrOrThrowAsync(model.AdrId, cancellationToken);
        await userOrganizationReadRepository.ThrowIfNotMemberAsync<AdrAccessException>(model.UserId, adr.OrganizationId, cancellationToken);

        var comment = new AdrComment
        {
            Id = Guid.NewGuid(),
            Text = model.Text,
            AdrId = model.AdrId,
            AuthorId = model.UserId,
        };
        adrCommentWriteRepository.Add(comment);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var result = mapper.Map<AdrCommentModel>(comment);
        await FillAuthorsAsync([result], cancellationToken);
        return result;
    }

    async Task<IEnumerable<AdrCommentModel>> IAdrCommentManager.GetByAdrIdAsync(Guid adrId, Guid userId, CancellationToken cancellationToken)
    {
        var adr = await GetAdrOrThrowAsync(adrId, cancellationToken);
        await userOrganizationReadRepository.ThrowIfNotMemberAsync<AdrAccessException>(userId, adr.OrganizationId, cancellationToken);

        var comments = await adrCommentReadRepository.GetByAdrIdAsync(adrId, cancellationToken);
        var result = mapper.Map<IEnumerable<AdrCommentModel>>(comments).ToReadOnlyCollection();
        await FillAuthorsAsync(result, cancellationToken);
        return result;
    }

    async Task<AdrCommentModel> IAdrCommentManager.UpdateAsync(UpdateAdrCommentModel model, CancellationToken cancellationToken)
    {
        var comment = await adrCommentReadRepository.GetActiveByIdAsync(model.Id, cancellationToken)
            .OrThrowIfNull(() => new AdrEntityNotFoundException<AdrComment>(model.Id));
        var adr = await GetAdrOrThrowAsync(comment!.AdrId, cancellationToken);
        await userOrganizationReadRepository.ThrowIfNotMemberAsync<AdrAccessException>(model.UserId, adr.OrganizationId, cancellationToken);

        if (comment.AuthorId != model.UserId)
        {
            throw new AdrAccessException();
        }

        comment.Text = model.Text;
        adrCommentWriteRepository.Update(comment);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var result = mapper.Map<AdrCommentModel>(comment);
        await FillAuthorsAsync([result], cancellationToken);
        return result;
    }

    async Task IAdrCommentManager.DeleteAsync(DeleteAdrCommentModel model, CancellationToken cancellationToken)
    {
        var comment = await adrCommentReadRepository.GetActiveByIdAsync(model.AdrCommentId, cancellationToken)
            .OrThrowIfNull(() => new AdrEntityNotFoundException<AdrComment>(model.AdrCommentId));
        var adr = await GetAdrOrThrowAsync(comment!.AdrId, cancellationToken);
        await userOrganizationReadRepository.ThrowIfNotMemberAsync<AdrAccessException>(model.UserId, adr.OrganizationId, cancellationToken);

        if (comment.AuthorId != model.UserId)
        {
            await userOrganizationReadRepository.ThrowIfNotAdminOrArchitectureAsync<AdrAccessException>(model.UserId, adr.OrganizationId, cancellationToken);
        }

        adrCommentWriteRepository.Delete(comment);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<Adr> GetAdrOrThrowAsync(Guid adrId, CancellationToken cancellationToken)
        => (await adrReadRepository.GetActiveByIdAsync(adrId, cancellationToken)
            .OrThrowIfNull(() => new AdrEntityNotFoundException<Adr>(adrId)))!;

    private async Task FillAuthorsAsync(IEnumerable<AdrCommentModel> models, CancellationToken cancellationToken)
    {
        foreach (var authorId in models.Select(x => x.AuthorId).Distinct())
        {
            var user = await userReadRepository.GetByIdAsync(authorId, cancellationToken);
            if (user == null)
            {
                continue;
            }

            foreach (var model in models.Where(x => x.AuthorId == authorId))
            {
                model.AuthorName = user.Name;
                model.AuthorLogin = user.Login;
            }
        }
    }
}
