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
        CreateMap<AdrSectionModel, AdrSectionApiModel>(MemberList.Destination)
             .ForMember(x => x.Hint, opt => opt.Ignore())
             .ForMember(x => x.Placeholder, opt => opt.Ignore());

        CreateMap<AdrModel, AdrApiModel>(MemberList.Destination)
            .ForMember(x => x.Score, opt => opt.Ignore())
            .ForMember(x => x.UserVote, opt => opt.Ignore())
            .ForMember(x => x.FolderPath, opt => opt.Ignore())
            .ForMember(x => x.Sections, opt => opt.MapFrom(x => x.Sections));

        CreateMap<AdrFolderModel, AdrFolderApiModel>(MemberList.Destination);

        CreateMap<AdrCommentModel, AdrCommentApiModel>(MemberList.Destination)
            .ForMember(x => x.AuthorName, opt => opt.Ignore())
            .ForMember(x => x.AuthorLogin, opt => opt.Ignore());

        CreateMap<AdrLinkModel, AdrLinkApiModel>(MemberList.Destination)
            .ForMember(x => x.TargetAdrNumber, opt => opt.Ignore())
            .ForMember(x => x.TargetAdrName, opt => opt.Ignore());

        CreateMap<AdrTemplateSectionModel, AdrTemplateSectionApiModel>(MemberList.Destination);

        CreateMap<AdrTemplateModel, AdrTemplateApiModel>(MemberList.Destination)
            .ForMember(x => x.IsBuiltIn, opt => opt.MapFrom(x => x.OrganizationId == null))
            .ForMember(x => x.Sections, opt => opt.MapFrom(x => x.Sections));

        CreateMap<AdrOrganizationSettingsModel, AdrOrganizationSettingsApiModel>(MemberList.Destination);

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
