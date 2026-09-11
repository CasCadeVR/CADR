using AutoMapper;
using CADR.Administrations.Services.Contracts.Models.Invites;
using CADR.Administrations.Services.Contracts.Models.Organizations;
using CADR.Administrations.Services.Contracts.Models.User;

namespace CADR.Administrations.Services.AutoMappers;

/// <inheritdoc />
public class AdministrationServiceProfile : Profile
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdministrationServiceProfile"/>
    /// </summary>
    public AdministrationServiceProfile()
    {
        CreateMap<Entities.User, UserLoggedModel>(MemberList.Destination);

        CreateMap<Entities.Organization, OrganizationModel>(MemberList.Destination)
            .ForMember(x => x.UserIsAdmin, opt => opt.Ignore());

        CreateMap<Entities.UserInvite, InviteModel>(MemberList.Destination)
            .ForMember(x => x.OwnerId, opt => opt.Ignore())
            .ForMember(x => x.UserMail, opt => opt.Ignore());

        CreateMap<Entities.UserInvite, InviteForUserModel>(MemberList.Destination);
    }
}
