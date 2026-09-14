using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using CADR.Adrs.Services.Contracts.Models.Adrs;
using CADR.Adrs.Services.Contracts.Models.Comments;
using CADR.Adrs.Services.Contracts.Models.Enums;
using CADR.Adrs.Services.Contracts.Models.Folders;
using CADR.Adrs.Services.Contracts.Models.Links;
using CADR.Adrs.Services.Contracts.Models.Settings;
using CADR.Adrs.Services.Contracts.Models.Templates;

namespace CADR.Adrs.Services.AutoMappers;

/// <inheritdoc />
public class AdrServiceProfile : Profile
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrServiceProfile"/>
    /// </summary>
    public AdrServiceProfile()
    {
        CreateMap<Entities.AdrSection, AdrSectionModel>(MemberList.Destination);

        CreateMap<Entities.Adr, AdrModel>(MemberList.Destination)
            .ForMember(x => x.Score, opt => opt.Ignore())
            .ForMember(x => x.UserVote, opt => opt.Ignore())
            .ForMember(x => x.Sections, opt => opt.MapFrom(x => x.Sections));

        CreateMap<Entities.AdrFolder, AdrFolderModel>(MemberList.Destination);

        CreateMap<Entities.AdrComment, AdrCommentModel>(MemberList.Destination)
            .ForMember(x => x.AuthorName, opt => opt.Ignore())
            .ForMember(x => x.AuthorLogin, opt => opt.Ignore())
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => x.CreatedAt.UtcDateTime));

        CreateMap<Entities.AdrLink, AdrLinkModel>(MemberList.Destination)
            .ForMember(x => x.TargetAdrNumber, opt => opt.Ignore())
            .ForMember(x => x.TargetAdrName, opt => opt.Ignore());

        CreateMap<Entities.AdrTemplateSection, AdrTemplateSectionModel>(MemberList.Destination);

        CreateMap<Entities.AdrTemplate, AdrTemplateModel>(MemberList.Destination)
            .ForMember(x => x.IsBuiltIn, opt => opt.MapFrom(x => x.OrganizationId == null))
            .ForMember(x => x.Sections, opt => opt.MapFrom(x => x.Sections));

        CreateMap<Entities.AdrOrganizationSettings, AdrOrganizationSettingsModel>(MemberList.Destination);

        CreateMap<AdrStatus, Entities.Enums.AdrStatus>()
            .ConvertUsingEnumMapping(opt => opt.MapByValue());
        CreateMap<Entities.Enums.AdrStatus, AdrStatus>()
            .ConvertUsingEnumMapping(opt => opt.MapByValue());
        CreateMap<AdrLinkType, Entities.Enums.AdrLinkType>()
            .ConvertUsingEnumMapping(opt => opt.MapByValue());
        CreateMap<Entities.Enums.AdrLinkType, AdrLinkType>()
            .ConvertUsingEnumMapping(opt => opt.MapByValue());
        CreateMap<AdrVoteType, Entities.Enums.AdrVoteType>()
            .ConvertUsingEnumMapping(opt => opt.MapByValue());
        CreateMap<Entities.Enums.AdrVoteType, AdrVoteType>()
            .ConvertUsingEnumMapping(opt => opt.MapByValue());
    }
}
