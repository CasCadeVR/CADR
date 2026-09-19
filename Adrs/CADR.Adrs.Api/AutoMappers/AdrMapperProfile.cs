using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using CADR.Adrs.Api.Models.Adrs;
using CADR.Adrs.Api.Models.Comments;
using CADR.Adrs.Api.Models.Folders;
using CADR.Adrs.Api.Models.Links;
using CADR.Adrs.Api.Models.Settings;
using CADR.Adrs.Api.Models.Templates;
using CADR.Adrs.Services.Contracts.Models.Adrs;
using CADR.Adrs.Services.Contracts.Models.Comments;
using CADR.Adrs.Services.Contracts.Models.Enums;
using CADR.Adrs.Services.Contracts.Models.Folders;
using CADR.Adrs.Services.Contracts.Models.Links;
using CADR.Adrs.Services.Contracts.Models.Settings;
using CADR.Adrs.Services.Contracts.Models.Templates;

namespace CADR.Adrs.Api.AutoMappers;

/// <inheritdoc />
public class AdrMapperProfile : Profile
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrMapperProfile"/>
    /// </summary>
    public AdrMapperProfile()
    {
        CreateMap<AdrSectionModel, AdrSectionApiModel>(MemberList.Destination);

        CreateMap<AdrModel, AdrApiModel>(MemberList.Destination)
            .ForMember(x => x.Sections, opt => opt.MapFrom(x => x.Sections));

        CreateMap<AdrFolderModel, AdrFolderApiModel>(MemberList.Destination);

        CreateMap<AdrCommentModel, AdrCommentApiModel>(MemberList.Destination);

        CreateMap<AdrLinkModel, AdrLinkApiModel>(MemberList.Destination);

        CreateMap<AdrTemplateSectionModel, AdrTemplateSectionApiModel>(MemberList.Destination);

        CreateMap<AdrTemplateModel, AdrTemplateApiModel>(MemberList.Destination)
            .ForMember(x => x.IsBuiltIn, opt => opt.MapFrom(x => x.OrganizationId == null))
            .ForMember(x => x.Sections, opt => opt.MapFrom(x => x.Sections));

        CreateMap<AdrOrganizationSettingsModel, AdrOrganizationSettingsApiModel>(MemberList.Destination);

        CreateMap<UpdateAdrOrganizationSettingsApiModel, UpdateAdrOrganizationSettingsModel>(MemberList.Destination);

        CreateMap<CreateAdrApiModel, CreateAdrModel>(MemberList.Destination);
        CreateMap<UpdateAdrApiModel, UpdateAdrModel>(MemberList.Destination);
        CreateMap<ChangeAdrStatusApiModel, ChangeAdrStatusModel>(MemberList.Destination);
        CreateMap<WithdrawVoteAdrApiModel, WithdrawVoteAdrModel>(MemberList.Destination);
        CreateMap<VoteAdrApiModel, VoteAdrModel>(MemberList.Destination);

        CreateMap<AdrSectionApiModel, AdrSectionModel>(MemberList.Destination);
        CreateMap<AdrFolderApiModel, AdrFolderModel>(MemberList.Destination);

        CreateMap<CreateAdrCommentApiModel, CreateAdrCommentModel>(MemberList.Destination);
        CreateMap<UpdateAdrCommentApiModel, UpdateAdrCommentModel>(MemberList.Destination);

        CreateMap<CreateAdrFolderApiModel, CreateAdrFolderModel>(MemberList.Destination);
        CreateMap<UpdateAdrFolderApiModel, UpdateAdrFolderModel>(MemberList.Destination);

        CreateMap<CreateAdrLinkApiModel, CreateAdrLinkModel>(MemberList.Destination);

        CreateMap<CreateAdrTemplateSectionApiModel, CreateAdrTemplateSectionModel>(MemberList.Destination);
        CreateMap<CreateAdrTemplateApiModel, CreateAdrTemplateModel>(MemberList.Destination);
        CreateMap<UpdateAdrTemplateApiModel, UpdateAdrTemplateModel>(MemberList.Destination)
            .ForMember(x => x.IsBuiltIn, opt => opt.MapFrom(x => x.OrganizationId == null));

        CreateMap<AdrStatus, Models.Enums.AdrStatusApi>()
            .ConvertUsingEnumMapping(opt => opt.MapByValue());
        CreateMap<Models.Enums.AdrStatusApi, AdrStatus>()
            .ConvertUsingEnumMapping(opt => opt.MapByValue());
        CreateMap<AdrLinkType, Models.Enums.AdrLinkTypeApi>()
            .ConvertUsingEnumMapping(opt => opt.MapByValue());
        CreateMap<Models.Enums.AdrLinkTypeApi, AdrLinkType>()
            .ConvertUsingEnumMapping(opt => opt.MapByValue());
        CreateMap<AdrVoteType, Models.Enums.AdrVoteTypeApi>()
            .ConvertUsingEnumMapping(opt => opt.MapByValue());
        CreateMap<Models.Enums.AdrVoteTypeApi, AdrVoteType>()
            .ConvertUsingEnumMapping(opt => opt.MapByValue());
    }
}
