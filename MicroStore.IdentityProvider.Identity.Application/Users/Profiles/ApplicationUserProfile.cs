using AutoMapper;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Users.Dtos;

namespace MicroStore.IdentityProvider.Identity.Application.Users.Profiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationIdentityUser, IdentityUserDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.UserRoles, opt => opt.MapFrom(c => c.UserRoles))
                .ForMember(x => x.UserClaims, opt => opt.MapFrom(c => c.UserClaims))
                .ForMember(x => x.LockoutEnd, opt => opt.MapFrom(c => c.LockoutEnd));

            CreateMap<ApplicationIdentityUser, IdentityUserListDto>();

            CreateMap<ApplicationIdentityUserRole, IdentityUserRoleDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.RoleId))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Role.Name));                
        }
    }
}
