using AutoMapper;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Dtos;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Models;

namespace MicroStore.IdentityProvider.Identity.Domain.Shared.Mappers
{
    public class ApplicationIdentityRoleProfile : Profile
    {
        public ApplicationIdentityRoleProfile()
        {
            CreateMap<ApplicationIdentityRole, IdentityRoleDto>();

            CreateMap<IdentityRoleDto, RoleModel>();
        }
    }
}
