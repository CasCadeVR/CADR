using AutoMapper;
using CADR.Administrations.Api.Models.Invites;
using CADR.Administrations.Api.Models.Organizations;
using CADR.Administrations.Api.Models.Users;
using CADR.Administrations.Services.Contracts.Models.Invites;
using CADR.Administrations.Services.Contracts.Models.Organizations;
using CADR.Administrations.Services.Contracts.Models.User;

namespace CADR.Administrations.Api.AutoMappers;

/// <inheritdoc />
public class AdministrationMapperProfile : Profile
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdministrationMapperProfile"/>
    /// </summary>
    public AdministrationMapperProfile()
    {
        CreateMap<CreateUserApiRequest, CreateUserModel>(MemberList.Destination);
        CreateMap<CreateOrganizationApiModel, CreateOrganizationModel>()
            .ForMember(x => x.UserId, opt => opt.Ignore())
            .ValidateMemberList(MemberList.Destination);
        CreateMap<OrganizationModel, OrganizationApiModel>(MemberList.Destination);
        CreateMap<OrganizationApiModel, UpdateOrganizationModel>()
            .ForMember(x => x.UserId, opt => opt.Ignore())
            .ValidateMemberList(MemberList.Destination);
        CreateMap<UserOrganizationModel, UserOrganizationApiModel>(MemberList.Destination);
        CreateMap<InviteApiModel, InviteModel>(MemberList.Destination)
            .ForMember(x => x.UserMail, opt => opt.MapFrom(x => x.Email))
            .ForMember(x => x.OwnerId, opt => opt.Ignore())
            .ForMember(x => x.OrganizationId, opt => opt.Ignore());
        CreateMap<InviteOrganizationModel, InviteOrganizationApiModel>(MemberList.Destination);
        CreateMap<ChangeUserRoleApiModel, ChangeUserRoleModel>(MemberList.Destination)
            .ForMember(x => x.UserId, opt => opt.Ignore());
        CreateMap<InviteForUserModel, InviteForUserApiResponse>(MemberList.Destination);
    }
}
