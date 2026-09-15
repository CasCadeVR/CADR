using AutoMapper;
using CADR.Administrations.Repositories.Contracts;
using CADR.Administrations.Repositories.Contracts.Extensions;
using CADR.Adrs.Entities;
using CADR.Adrs.Repositories.Contracts;
using CADR.Adrs.Services.Contracts.Exceptions;
using CADR.Adrs.Services.Contracts.Interfaces;
using CADR.Adrs.Services.Contracts.Models.Templates;
using CADR.Adrs.Services.Resources;
using CADR.Common.Core.Extensions;

namespace CADR.Adrs.Services.Templates;

/// <inheritdoc cref="IAdrTemplateManager"/>
internal sealed class AdrTemplateManager : IAdrTemplateManager, IAdrsServiceAnchor
{
    private readonly IAdrUnitOfWork unitOfWork;
    private readonly IAdrTemplateReadRepository adrTemplateReadRepository;
    private readonly IAdrTemplateWriteRepository adrTemplateWriteRepository;
    private readonly IAdrTemplateSectionReadRepository adrTemplateSectionReadRepository;
    private readonly IAdrTemplateSectionWriteRepository adrTemplateSectionWriteRepository;
    private readonly IUserOrganizationReadRepository userOrganizationReadRepository;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrTemplateManager"/>
    /// </summary>
    public AdrTemplateManager(IAdrUnitOfWork adrUnitOfWork, IMapper mapper, IUserOrganizationReadRepository userOrganizationReadRepository)
    {
        unitOfWork = adrUnitOfWork;
        adrTemplateReadRepository = adrUnitOfWork.AdrTemplateReadRepository;
        adrTemplateWriteRepository = adrUnitOfWork.AdrTemplateWriteRepository;
        adrTemplateSectionReadRepository = adrUnitOfWork.AdrTemplateSectionReadRepository;
        adrTemplateSectionWriteRepository = adrUnitOfWork.AdrTemplateSectionWriteRepository;
        this.userOrganizationReadRepository = userOrganizationReadRepository;
        this.mapper = mapper;
    }

    async Task<AdrTemplateModel> IAdrTemplateManager.CreateAsync(CreateAdrTemplateModel model, CancellationToken cancellationToken)
    {
        await userOrganizationReadRepository.ThrowIfNotAdminOrArchitectureAsync<AdrAccessException>(model.UserId, model.OrganizationId, cancellationToken);

        var template = new AdrTemplate
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            OrganizationId = model.OrganizationId,
        };
        foreach (var sectionModel in model.Sections.OrderBy(x => x.Position))
        {
            var section = new AdrTemplateSection
            {
                Id = Guid.NewGuid(),
                Position = sectionModel.Position,
                Title = sectionModel.Title,
                Hint = sectionModel.Hint,
                Placeholder = sectionModel.Placeholder,
                TemplateId = template.Id,
            };

            template.Sections.Add(section);
            adrTemplateSectionWriteRepository.Add(section);
        }

        adrTemplateWriteRepository.Add(template);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<AdrTemplateModel>(template);
    }

    async Task<AdrTemplateModel> IAdrTemplateManager.GetByIdAsync(Guid templateId, Guid userId, CancellationToken cancellationToken)
    {
        var template = await adrTemplateReadRepository.GetActiveByIdAsync(templateId, cancellationToken)
            .OrThrowIfNull(() => new AdrEntityNotFoundException<AdrTemplate>(templateId));
        if (template!.OrganizationId.HasValue)
        {
            await userOrganizationReadRepository.ThrowIfNotMemberAsync<AdrAccessException>(userId, template.OrganizationId.Value, cancellationToken);
        }

        var result = mapper.Map<AdrTemplateModel>(template);
        await FillSectionsAsync(result, cancellationToken);
        return result;
    }

    async Task<IEnumerable<AdrTemplateModel>> IAdrTemplateManager.GetAvailableForOrganizationAsync(Guid organizationId, Guid userId, CancellationToken cancellationToken)
    {
        await userOrganizationReadRepository.ThrowIfNotMemberAsync<AdrAccessException>(userId, organizationId, cancellationToken);
        var templates = await adrTemplateReadRepository.GetAvailableForOrganizationAsync(organizationId, cancellationToken);
        var result = mapper.Map<IEnumerable<AdrTemplateModel>>(templates).ToReadOnlyCollection();
        foreach (var templateModel in result)
        {
            await FillSectionsAsync(templateModel, cancellationToken);
        }

        return result;
    }

    async Task<AdrTemplateModel> IAdrTemplateManager.UpdateAsync(UpdateAdrTemplateModel model, CancellationToken cancellationToken)
    {
        var template = await adrTemplateReadRepository.GetActiveByIdAsync(model.Id, cancellationToken)
            .OrThrowIfNull(() => new AdrEntityNotFoundException<AdrTemplate>(model.Id));
        if (template!.OrganizationId == null)
        {
            throw new AdrInvalidOperationException(ErrorMessages.TemplateIsBuiltIn);
        }

        await userOrganizationReadRepository.ThrowIfNotAdminOrArchitectureAsync<AdrAccessException>(model.UserId, template.OrganizationId.Value, cancellationToken);

        template.Name = model.Name;
        var existingSections = await adrTemplateSectionReadRepository.GetByTemplateIdAsync(template.Id, cancellationToken);
        foreach (var section in existingSections)
        {
            adrTemplateSectionWriteRepository.Delete(section);
        }

        foreach (var sectionModel in model.Sections.OrderBy(x => x.Position))
        {
            adrTemplateSectionWriteRepository.Add(new AdrTemplateSection
            {
                Id = Guid.NewGuid(),
                Position = sectionModel.Position,
                Title = sectionModel.Title,
                Hint = sectionModel.Hint,
                Placeholder = sectionModel.Placeholder,
                TemplateId = template.Id,
            });
        }

        adrTemplateWriteRepository.Update(template);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var result = mapper.Map<AdrTemplateModel>(template);
        await FillSectionsAsync(result, cancellationToken);
        return result;
    }

    async Task IAdrTemplateManager.DeleteAsync(DeleteAdrTemplateModel model, CancellationToken cancellationToken)
    {
        var template = await adrTemplateReadRepository.GetActiveByIdAsync(model.AdrTemplateId, cancellationToken)
            .OrThrowIfNull(() => new AdrEntityNotFoundException<AdrTemplate>(model.AdrTemplateId));
        if (template!.OrganizationId == null)
        {
            throw new AdrInvalidOperationException(ErrorMessages.TemplateIsBuiltIn);
        }

        await userOrganizationReadRepository.ThrowIfNotAdminOrArchitectureAsync<AdrAccessException>(model.UserId, template.OrganizationId.Value, cancellationToken);

        var sections = await adrTemplateSectionReadRepository.GetByTemplateIdAsync(template.Id, cancellationToken);
        foreach (var section in sections)
        {
            adrTemplateSectionWriteRepository.Delete(section);
        }

        adrTemplateWriteRepository.Delete(template);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task FillSectionsAsync(AdrTemplateModel model, CancellationToken cancellationToken)
    {
        var sections = await adrTemplateSectionReadRepository.GetByTemplateIdAsync(model.Id, cancellationToken);
        model.Sections = mapper.Map<IReadOnlyCollection<AdrTemplateSectionModel>>(sections.OrderBy(x => x.Position).ToReadOnlyCollection());
    }
}
