using AutoMapper;
using IdentityModel;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Dtos;

namespace MicroStore.IdentityProvider.Identity.Application.Mappers
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationIdentityUser, IdentityUserDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.UserRoles, opt => opt.MapFrom(c => c.UserRoles))
                .ForMember(x => x.LockoutEnd, opt => opt.MapFrom(c => c.LockoutEnd));

            CreateMap<ApplicationIdentityUser, IdentityUserListDto>();

            CreateMap<ApplicationIdentityUserRole, IdentityUserRoleDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.RoleId))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Role.Name));
        }
    }
}
