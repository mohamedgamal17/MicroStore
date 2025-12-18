using AutoMapper;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Dtos;
using MicroStore.IdentityProvider.Identity.Web.Areas.BackEnd.Models.Roles;
using MicroStore.IdentityProvider.Identity.Web.Areas.BackEnd.Models.Users;

namespace MicroStore.IdentityProvider.Identity.Web.Areas.BackEnd.Mappers
{
    public class IdentityProfile : Profile
    {
        public IdentityProfile()
        {
            CreateMap<IdentityUserDto, EditUserModel>()
                .ForMember(x => x.UserRoles, opt => opt.MapFrom(c => c.UserRoles.Select(r => r.Name)));

            CreateMap<IdentityRoleDto, EditRoleModel>();

        }
    }
}
