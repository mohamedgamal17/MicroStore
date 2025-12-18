using AutoMapper;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Dtos;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Models;

namespace MicroStore.IdentityProvider.Identity.Domain.Shared.Mappers
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationIdentityUser, IdentityUserDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.UserRoles, opt => opt.MapFrom(c => c.UserRoles))
                .ForMember(x => x.LockoutEnd, opt => opt.MapFrom(c => c.LockoutEnd));

            CreateMap<ApplicationIdentityUser, IdentityUserDto>();

            CreateMap<ApplicationIdentityUserRole, IdentityUserRoleDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.RoleId))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Role.Name));

            CreateMap<IdentityUserDto, UserModel>()
                .ForMember(x => x.UserRoles, opt => opt.MapFrom(c => c.UserRoles.Select(r => r.Name).ToList()));
        }
    }
}
