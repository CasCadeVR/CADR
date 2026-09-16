using AutoMapper;
using CADR.Administrations.Repositories.Contracts;
using CADR.Administrations.Repositories.Contracts.Extensions;
using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Adrs.Services.Contracts.Exceptions;
using CADR.Adrs.Services.Contracts.Interfaces;
using CADR.Adrs.Services.Contracts.Models.Folders;
using CADR.Adrs.Services.Resources;
using CADR.Common.Core.Extensions;

namespace CADR.Adrs.Services.Folders;

/// <inheritdoc cref="IAdrFolderManager"/>
internal sealed class AdrFolderManager : IAdrFolderManager, IAdrsServiceAnchor
{
    private readonly IAdrUnitOfWork unitOfWork;
    private readonly IAdrReadRepository adrReadRepository;
    private readonly IAdrWriteRepository adrWriteRepository;
    private readonly IAdrFolderReadRepository adrFolderReadRepository;
    private readonly IAdrFolderWriteRepository adrFolderWriteRepository;
    private readonly IUserOrganizationReadRepository userOrganizationReadRepository;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrFolderManager"/>
    /// </summary>
    public AdrFolderManager(IAdrUnitOfWork adrUnitOfWork, IMapper mapper, IUserOrganizationReadRepository userOrganizationReadRepository)
    {
        unitOfWork = adrUnitOfWork;
        adrReadRepository = adrUnitOfWork.AdrReadRepository;
        adrWriteRepository = adrUnitOfWork.AdrWriteRepository;
        adrFolderReadRepository = adrUnitOfWork.AdrFolderReadRepository;
        adrFolderWriteRepository = adrUnitOfWork.AdrFolderWriteRepository;
        this.userOrganizationReadRepository = userOrganizationReadRepository;
        this.mapper = mapper;
    }

    async Task<AdrFolderModel> IAdrFolderManager.CreateAsync(CreateAdrFolderModel model, CancellationToken cancellationToken)
    {
        await userOrganizationReadRepository.ThrowIfNotAdminOrArchitectureAsync<AdrAccessException>(model.UserId, model.OrganizationId, cancellationToken);
        if (model.ParentAdrFolderId.HasValue)
        {
            var parent = await adrFolderReadRepository.GetActiveByIdAsync(model.ParentAdrFolderId.Value, cancellationToken)
                .OrThrowIfNull(() => new AdrEntityNotFoundException<AdrFolder>(model.ParentAdrFolderId.Value));
            if (parent!.OrganizationId != model.OrganizationId)
            {
                throw new AdrInvalidOperationException(ErrorMessages.ParentFolderFromAnotherOrganization);
            }
        }

        var folder = new AdrFolder
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            OrganizationId = model.OrganizationId,
            ParentAdrFolderId = model.ParentAdrFolderId,
        };
        adrFolderWriteRepository.Add(folder);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<AdrFolderModel>(folder);
    }

    async Task<IEnumerable<AdrFolderModel>> IAdrFolderManager.GetByOrganizationIdAsync(Guid organizationId, Guid userId, CancellationToken cancellationToken)
    {
        await userOrganizationReadRepository.ThrowIfNotMemberAsync<AdrAccessException>(userId, organizationId, cancellationToken);
        var folders = await adrFolderReadRepository.GetByOrganizationIdAsync(organizationId, cancellationToken);
        return mapper.Map<IEnumerable<AdrFolderModel>>(folders);
    }

    async Task<IEnumerable<AdrFolderModel>> IAdrFolderManager.GetPathAsync(Guid organizationId, Guid? folderId, Guid userId, CancellationToken cancellationToken)
    {
        await userOrganizationReadRepository.ThrowIfNotMemberAsync<AdrAccessException>(userId, organizationId, cancellationToken);
        if (!folderId.HasValue)
        {
            return [];
        }

        var organizationFolders = await adrFolderReadRepository.GetByOrganizationIdAsync(organizationId, cancellationToken);
        var folderById = organizationFolders.ToDictionary(x => x.Id);
        if (!folderById.TryGetValue(folderId.Value, out var folder))
        {
            throw new AdrEntityNotFoundException<AdrFolder>(folderId.Value);
        }

        var path = AdrFolderTree.GetPath(folder, folderById);
        return mapper.Map<IEnumerable<AdrFolderModel>>(path);
    }

    async Task<AdrFolderModel> IAdrFolderManager.UpdateAsync(UpdateAdrFolderModel model, CancellationToken cancellationToken)
    {
        var folder = await adrFolderReadRepository.GetActiveByIdAsync(model.Id, cancellationToken)
            .OrThrowIfNull(() => new AdrEntityNotFoundException<AdrFolder>(model.Id));
        await userOrganizationReadRepository.ThrowIfNotAdminOrArchitectureAsync<AdrAccessException>(model.UserId, folder!.OrganizationId, cancellationToken);

        if (folder.ParentAdrFolderId != model.ParentAdrFolderId)
        {
            if (model.ParentAdrFolderId == folder.Id)
            {
                throw new AdrInvalidOperationException(ErrorMessages.FolderMoveToItself);
            }

            if (model.ParentAdrFolderId.HasValue)
            {
                var parent = await adrFolderReadRepository.GetActiveByIdAsync(model.ParentAdrFolderId.Value, cancellationToken)
                    .OrThrowIfNull(() => new AdrEntityNotFoundException<AdrFolder>(model.ParentAdrFolderId.Value));
                if (parent!.OrganizationId != folder.OrganizationId)
                {
                    throw new AdrInvalidOperationException(ErrorMessages.ParentFolderFromAnotherOrganization);
                }

                var organizationFolders = await adrFolderReadRepository.GetByOrganizationIdAsync(folder.OrganizationId, cancellationToken);
                if (IsDescendant(folder.Id, model.ParentAdrFolderId, organizationFolders))
                {
                    throw new AdrInvalidOperationException(ErrorMessages.FolderMoveToDescendant);
                }
            }
        }

        folder.Name = model.Name;
        folder.ParentAdrFolderId = model.ParentAdrFolderId;
        adrFolderWriteRepository.Update(folder);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<AdrFolderModel>(folder);
    }

    async Task<int> IAdrFolderManager.GetAdrCountAsync(Guid organizationId, Guid folderId, Guid userId, CancellationToken cancellationToken)
    {
        var folder = await adrFolderReadRepository.GetActiveByIdAsync(folderId, cancellationToken)
            .OrThrowIfNull(() => new AdrEntityNotFoundException<AdrFolder>(folderId));
        await userOrganizationReadRepository.ThrowIfNotMemberAsync<AdrAccessException>(userId, folder!.OrganizationId, cancellationToken);

        var organizationFolders = await adrFolderReadRepository.GetByOrganizationIdAsync(folder.OrganizationId, cancellationToken);
        var subtreeFolderIds = CollectSubtreeFolderIds(folderId, organizationFolders);
        return await adrReadRepository.GetCountByFolderIdsAsync(subtreeFolderIds, cancellationToken);
    }

    async Task IAdrFolderManager.DeleteAsync(DeleteAdrFolderModel model, CancellationToken cancellationToken)
    {
        var folder = await adrFolderReadRepository.GetActiveByIdAsync(model.AdrFolderId, cancellationToken)
            .OrThrowIfNull(() => new AdrEntityNotFoundException<AdrFolder>(model.AdrFolderId));
        await userOrganizationReadRepository.ThrowIfNotAdminOrArchitectureAsync<AdrAccessException>(model.UserId, folder!.OrganizationId, cancellationToken);

        var organizationFolders = await adrFolderReadRepository.GetByOrganizationIdAsync(folder.OrganizationId, cancellationToken);
        var subtreeFolderIds = CollectSubtreeFolderIds(folder.Id, organizationFolders);
        var subtreeFolderIdsSet = subtreeFolderIds.ToHashSet();
        var adrs = await adrReadRepository.GetByOrganizationIdAsync(folder.OrganizationId, cancellationToken);

        foreach (var adr in adrs.Where(x => x.ParentAdrFolderId.HasValue && subtreeFolderIdsSet.Contains(x.ParentAdrFolderId.Value)))
        {
            adrWriteRepository.Delete(adr);
        }

        foreach (var folderToDelete in organizationFolders.Where(x => subtreeFolderIdsSet.Contains(x.Id)))
        {
            adrFolderWriteRepository.Delete(folderToDelete);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static IReadOnlyCollection<Guid> CollectSubtreeFolderIds(Guid rootFolderId, IReadOnlyCollection<AdrFolder> organizationFolders)
    {
        var childFolderIds = organizationFolders
            .Where(x => x.ParentAdrFolderId.HasValue)
            .GroupBy(x => x.ParentAdrFolderId!.Value)
            .ToDictionary(x => x.Key, x => x.Select(y => y.Id).ToList());
        var subtreeFolderIds = new List<Guid>();
        var stack = new Stack<Guid>();
        stack.Push(rootFolderId);
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            subtreeFolderIds.Add(current);
            if (childFolderIds.TryGetValue(current, out var children))
            {
                foreach (var childId in children)
                {
                    stack.Push(childId);
                }
            }
        }

        return subtreeFolderIds;
    }

    private static bool IsDescendant(Guid folderId, Guid? startParentFolderId, IReadOnlyCollection<AdrFolder> organizationFolders)
    {
        var folderById = organizationFolders.ToDictionary(x => x.Id);
        var current = startParentFolderId;
        while (current.HasValue)
        {
            if (current.Value == folderId)
            {
                return true;
            }

            current = folderById.TryGetValue(current.Value, out var folder) ? folder.ParentAdrFolderId : null;
        }

        return false;
    }
}
