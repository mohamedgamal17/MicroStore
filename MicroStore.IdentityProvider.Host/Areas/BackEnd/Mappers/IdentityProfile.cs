using AutoMapper;
using MicroStore.IdentityProvider.Host.Areas.BackEnd.Models.Roles;
using MicroStore.IdentityProvider.Host.Areas.BackEnd.Models.Users;
using MicroStore.IdentityProvider.Identity.Application.Dtos;

namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Mappers
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
