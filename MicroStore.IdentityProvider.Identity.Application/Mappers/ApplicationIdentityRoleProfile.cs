using AutoMapper;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Dtos;
using MicroStore.IdentityProvider.Identity.Application.Models;

namespace MicroStore.IdentityProvider.Identity.Application.Mappers
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
