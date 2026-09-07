using AutoMapper;
using CADR.Administrations.Pages.Models.Account;
using CADR.Administrations.Pages.Models.Organization;
using CADR.Api.Client;

namespace CADR.Administrations.Pages.Infrastructures;

/// <summary>
/// Профиль для маппинга сущностей страниц
/// </summary>
public class AdministrationsProfile : Profile
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdministrationsProfile"/>
    /// </summary>
    public AdministrationsProfile()
    {
        CreateMap<LoginRequestModel, LoginApiRequest>(MemberList.Destination);
        CreateMap<RegisterRequestModel, CreateUserApiRequest>(MemberList.Destination);
        CreateMap<OrganizationRequestModel, CreateOrganizationApiModel>(MemberList.Destination);
        CreateMap<OrganizationRequestModel, OrganizationApiModel>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserIsAdmin, opt => opt.Ignore());
        CreateMap<OrganizationApiModel, OrganizationRequestModel>(MemberList.Destination);
        CreateMap<OrganizationInviteModel, InviteApiModel>(MemberList.Destination)
            .ForMember(x => x.Email, opt => opt.MapFrom(x => x.UserMail))
            .ForMember(x => x.Role, opt => opt.MapFrom(x => x.UserRole));
    }
}
